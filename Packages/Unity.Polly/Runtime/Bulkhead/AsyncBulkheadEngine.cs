using Cysharp.Threading.Tasks;
using System;
using System.Threading;

namespace Polly.Bulkhead
{
    internal static class AsyncBulkheadEngine
    {
       internal static async UniTask<TResult> ImplementationAsync<TResult>(
            Func<Context, CancellationToken, UniTask<TResult>> action,
            Context context,
            Func<Context, UniTask> onBulkheadRejectedAsync,
            SemaphoreSlim maxParallelizationSemaphore,
            SemaphoreSlim maxQueuedActionsSemaphore,
            CancellationToken cancellationToken, 
            bool continueOnCapturedContext)
        {
            if (!await maxQueuedActionsSemaphore.WaitAsync(TimeSpan.Zero, cancellationToken).ConfigureAwait(continueOnCapturedContext))
            {
                await onBulkheadRejectedAsync(context);
                throw new BulkheadRejectedException();
            }
            try
            {
                await maxParallelizationSemaphore.WaitAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext);

                try 
                {
                    return await action(context, cancellationToken);
                }
                finally
                {
                    maxParallelizationSemaphore.Release();
                }
            }
            finally
            {
                maxQueuedActionsSemaphore.Release();
            }
        }
    }
}
