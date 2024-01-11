using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Polly.Utilities;

namespace Polly.Timeout
{
    internal static class AsyncTimeoutEngine
    {
        internal static async UniTask<TResult> ImplementationAsync<TResult>(
            Func<Context, CancellationToken, UniTask<TResult>> action, 
            Context context, 
            CancellationToken cancellationToken, 
            Func<Context, TimeSpan> timeoutProvider,
            TimeoutStrategy timeoutStrategy,
            Func<Context, TimeSpan, UniTask, Exception, UniTask> onTimeoutAsync, 
            bool continueOnCapturedContext)
        {
            cancellationToken.ThrowIfCancellationRequested();
            TimeSpan timeout = timeoutProvider(context);

            using (CancellationTokenSource timeoutCancellationTokenSource = new CancellationTokenSource())
            {
                using (CancellationTokenSource combinedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCancellationTokenSource.Token))
                {
                    UniTask<TResult> actionTask = default;
                    CancellationToken combinedToken = combinedTokenSource.Token;

                    try
                    {
                        if (timeoutStrategy == TimeoutStrategy.Optimistic)
                        {
                            SystemClock.CancelTokenAfter(timeoutCancellationTokenSource, timeout);
                            return await action(context, combinedToken);
                        }

                        // else: timeoutStrategy == TimeoutStrategy.Pessimistic

                        UniTask<TResult> timeoutTask = timeoutCancellationTokenSource.Token.AsTask<TResult>();

                        SystemClock.CancelTokenAfter(timeoutCancellationTokenSource, timeout);

                        actionTask = action(context, combinedToken);

                        var result = await UniTask.WhenAny(actionTask, timeoutTask);
                        return result.winArgumentIndex == 0 ? result.result1 : result.result2;

                    }
                    catch (Exception ex)
                    {
                        // Note that we cannot rely on testing (operationCanceledException.CancellationToken == combinedToken || operationCanceledException.CancellationToken == timeoutCancellationTokenSource.Token)
                        // as either of those tokens could have been onward combined with another token by executed code, and so may not be the token expressed on operationCanceledException.CancellationToken.
                        if (ex is OperationCanceledException && timeoutCancellationTokenSource.IsCancellationRequested)
                        {
                            await onTimeoutAsync(context, timeout, actionTask, ex);
                            throw new TimeoutRejectedException("The delegate executed asynchronously through TimeoutPolicy did not complete within the timeout.", ex);
                        }

                        throw;
                    }
                }
            }
        }

        private static UniTask<TResult> AsTask<TResult>(this CancellationToken cancellationToken)
        {
            var tcs = new UniTaskCompletionSource<TResult>();

            // A generalised version of this method would include a hotpath returning a canceled task (rather than setting up a registration) if (cancellationToken.IsCancellationRequested) on entry.  This is omitted, since we only start the timeout countdown in the token _after calling this method.

            IDisposable registration = null;
                registration = cancellationToken.Register(() =>
                {
                    tcs.TrySetCanceled();
                    registration?.Dispose();
                }, useSynchronizationContext: false);

            return tcs.Task;
        }
    }
}
