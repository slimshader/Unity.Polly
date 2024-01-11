using Cysharp.Threading.Tasks;
using System;
using System.Threading;

namespace Polly.Fallback
{
    internal class AsyncFallbackEngine
    {
        internal static async UniTask<TResult> ImplementationAsync<TResult>(
            Func<Context, CancellationToken, UniTask<TResult>> action,
            Context context,
            CancellationToken cancellationToken,
            ExceptionPredicates shouldHandleExceptionPredicates,
            ResultPredicates<TResult> shouldHandleResultPredicates,
            Func<DelegateResult<TResult>, Context, UniTask> onFallbackAsync,
            Func<DelegateResult<TResult>, Context, CancellationToken, UniTask<TResult>> fallbackAction,
            bool continueOnCapturedContext)
        {
            DelegateResult<TResult> delegateOutcome;

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                TResult result = await action(context, cancellationToken);

                if (!shouldHandleResultPredicates.AnyMatch(result))
                {
                    return result;
                }

                delegateOutcome = new DelegateResult<TResult>(result);
            }
            catch (Exception ex)
            {
                Exception handledException = shouldHandleExceptionPredicates.FirstMatchOrDefault(ex);
                if (handledException == null)
                {
                    throw;
                }

                delegateOutcome = new DelegateResult<TResult>(handledException);
            }

            await onFallbackAsync(delegateOutcome, context);

            return await fallbackAction(delegateOutcome, context, cancellationToken);
        }
    }
}
