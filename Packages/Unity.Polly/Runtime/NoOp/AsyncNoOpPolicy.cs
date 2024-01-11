using Cysharp.Threading.Tasks;
using System;
using System.Diagnostics;
using System.Threading;

namespace Polly.NoOp
{
    /// <summary>
    /// A noop policy that can be applied to asynchronous delegates.
    /// </summary>
    public class AsyncNoOpPolicy : AsyncPolicy, INoOpPolicy
    {
        internal AsyncNoOpPolicy() 
        {
        }

        /// <inheritdoc/>
        [DebuggerStepThrough]
        protected override UniTask<TResult> ImplementationAsync<TResult>( Func<Context, CancellationToken,UniTask<TResult>> action, Context context, CancellationToken cancellationToken,
            bool continueOnCapturedContext)
            => NoOpEngine.ImplementationAsync(action, context, cancellationToken, continueOnCapturedContext);
    }

    /// <summary>
    /// A noop policy that can be applied to asynchronous delegates returning a value of type <typeparamref name="TResult"/>.
    /// </summary>
    public class AsyncNoOpPolicy<TResult> : AsyncPolicy<TResult>, INoOpPolicy<TResult>
    {
        internal AsyncNoOpPolicy() 
        {
        }

        /// <inheritdoc/>
        [DebuggerStepThrough]
        protected override UniTask<TResult> ImplementationAsync(Func<Context, CancellationToken, UniTask<TResult>> action, Context context, CancellationToken cancellationToken,
            bool continueOnCapturedContext)
            => NoOpEngine.ImplementationAsync(action, context, cancellationToken, continueOnCapturedContext);
    }
}