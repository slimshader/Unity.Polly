using Cysharp.Threading.Tasks;
using Polly.Utilities;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Polly.Retry
{
    internal static class AsyncRetryEngine
    {
        internal static async UniTask<TResult> ImplementationAsync<TResult>(
            Func<Context, CancellationToken, UniTask<TResult>> action,
            Context context,
            CancellationToken cancellationToken,
            ExceptionPredicates shouldRetryExceptionPredicates,
            ResultPredicates<TResult> shouldRetryResultPredicates,
            Func<DelegateResult<TResult>, TimeSpan, int, Context, UniTask> onRetryAsync,
            int permittedRetryCount = Int32.MaxValue,
            IEnumerable<TimeSpan> sleepDurationsEnumerable = null,
            Func<int, DelegateResult<TResult>, Context, TimeSpan> sleepDurationProvider = null,
            bool continueOnCapturedContext = false)
        {
            int tryCount = 0;
            IEnumerator<TimeSpan> sleepDurationsEnumerator = sleepDurationsEnumerable?.GetEnumerator();

            try
            {
                while (true)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    bool canRetry;
                    DelegateResult<TResult> outcome;

                    try
                    {
                        TResult result = await action(context, cancellationToken);

                        if (!shouldRetryResultPredicates.AnyMatch(result))
                        {
                            return result;
                        }

                        canRetry = tryCount < permittedRetryCount && (sleepDurationsEnumerator == null || sleepDurationsEnumerator.MoveNext());

                        if (!canRetry)
                        {
                            return result;
                        }

                        outcome = new DelegateResult<TResult>(result);
                    }
                    catch (Exception ex)
                    {
                        Exception handledException = shouldRetryExceptionPredicates.FirstMatchOrDefault(ex);
                        if (handledException == null)
                        {
                            throw;
                        }

                        canRetry = tryCount < permittedRetryCount && (sleepDurationsEnumerator == null || sleepDurationsEnumerator.MoveNext());

                        if (!canRetry)
                        {
                            handledException.RethrowWithOriginalStackTraceIfDiffersFrom(ex);
                            throw;
                        }

                        outcome = new DelegateResult<TResult>(handledException);
                    }

                    if (tryCount < int.MaxValue) { tryCount++; }

                    TimeSpan waitDuration = sleepDurationsEnumerator?.Current ?? (sleepDurationProvider?.Invoke(tryCount, outcome, context) ?? TimeSpan.Zero);

                    await onRetryAsync(outcome, waitDuration, tryCount, context);

                    if (waitDuration > TimeSpan.Zero)
                    {
                        await SystemClock.SleepAsync(waitDuration, cancellationToken);
                    }
                }
            }
            finally
            {
                sleepDurationsEnumerator?.Dispose();
            }
        }
    }
}