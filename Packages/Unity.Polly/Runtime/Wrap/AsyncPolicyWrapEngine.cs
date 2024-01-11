using Cysharp.Threading.Tasks;
using System;
using System.Threading;

namespace Polly.Wrap
{
    internal static class AsyncPolicyWrapEngine
    {
        internal static async UniTask<TResult> ImplementationAsync<TResult>(
           Func<Context, CancellationToken, UniTask<TResult>> func,
            Context context,
            CancellationToken cancellationToken,
            bool continueOnCapturedContext,
            IAsyncPolicy<TResult> outerPolicy,
            IAsyncPolicy<TResult> innerPolicy)
            => await outerPolicy.ExecuteAsync(
                async (ctx, ct) => await innerPolicy.ExecuteAsync(
                    func, 
                    ctx, 
                    ct, 
                    continueOnCapturedContext
                    ), 
                context, 
                cancellationToken, 
                continueOnCapturedContext
                );

        internal static async UniTask<TResult> ImplementationAsync<TResult>(
           Func<Context, CancellationToken, UniTask<TResult>> func,
            Context context,
            CancellationToken cancellationToken,
            bool continueOnCapturedContext,
            IAsyncPolicy<TResult> outerPolicy,
            IAsyncPolicy innerPolicy)
            => await outerPolicy.ExecuteAsync(
                async (ctx, ct) => await innerPolicy.ExecuteAsync<TResult>(
                    func,
                    ctx,
                    ct,
                    continueOnCapturedContext
                    ),
                context,
                cancellationToken,
                continueOnCapturedContext
                );

        internal static async UniTask<TResult> ImplementationAsync<TResult>(
            Func<Context, CancellationToken, UniTask<TResult>> func,
            Context context,
            CancellationToken cancellationToken,
            bool continueOnCapturedContext,
            IAsyncPolicy outerPolicy,
            IAsyncPolicy<TResult> innerPolicy)
            => await outerPolicy.ExecuteAsync<TResult>(
                async (ctx, ct) => await innerPolicy.ExecuteAsync(
                    func,
                    ctx,
                    ct,
                    continueOnCapturedContext
                    )   ,
                context,
                cancellationToken,
                continueOnCapturedContext
                );

        internal static async UniTask<TResult> ImplementationAsync<TResult>(
           Func<Context, CancellationToken, UniTask<TResult>> func,
           Context context,
           CancellationToken cancellationToken,
           bool continueOnCapturedContext,
           IAsyncPolicy outerPolicy,
           IAsyncPolicy innerPolicy)
            => await outerPolicy.ExecuteAsync<TResult>(
                async (ctx, ct) => await innerPolicy.ExecuteAsync<TResult>(
                    func,
                    ctx,
                    ct,
                    continueOnCapturedContext
                ),
                context,
                cancellationToken,
                continueOnCapturedContext
            );

        internal static async UniTask ImplementationAsync(
            Func<Context, CancellationToken, UniTask> action,
            Context context,
            CancellationToken cancellationToken,
            bool continueOnCapturedContext,
            IAsyncPolicy outerPolicy,
            IAsyncPolicy innerPolicy)
            => await outerPolicy.ExecuteAsync(
                async (ctx, ct) => await innerPolicy.ExecuteAsync(
                    action,
                    ctx,
                    ct,
                    continueOnCapturedContext
                    ),
                context,
                cancellationToken,
                continueOnCapturedContext
                );

    }
}
