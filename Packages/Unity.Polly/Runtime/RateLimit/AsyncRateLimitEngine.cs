using Cysharp.Threading.Tasks;
using System;
using System.Threading;

namespace Polly.RateLimit
{
    internal static class AsyncRateLimitEngine
    {
        internal static async UniTask<TResult> ImplementationAsync<TResult>(
            IRateLimiter rateLimiter,
            Func<TimeSpan, Context, TResult> retryAfterFactory,
            Func<Context, CancellationToken, UniTask<TResult>> action,
            Context context,
            CancellationToken cancellationToken,
            bool continueOnCapturedContext
            )
        {
            (bool permit, TimeSpan retryAfter) = rateLimiter.PermitExecution();

            if (permit)
            {
                return await action(context, cancellationToken);
            }

            if (retryAfterFactory != null)
            {
                return retryAfterFactory(retryAfter, context);
            }

            throw new RateLimitRejectedException(retryAfter);
        }
    }
}
