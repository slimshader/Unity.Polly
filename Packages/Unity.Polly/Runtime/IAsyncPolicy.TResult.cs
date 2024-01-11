using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Polly
{
    /// <summary>
    /// An interface defining all executions available on an asynchronous policy generic-typed for executions returning results of type <typeparamref name="TResult"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of the result of funcs executed through the Policy.</typeparam>
    public interface IAsyncPolicy<TResult> : IsPolicy
    {
        /// <summary>
        /// Sets the PolicyKey for this <see cref="IAsyncPolicy{TResult}"/> instance.
        /// <remarks>Must be called before the policy is first used.  Can only be set once.</remarks>
        /// </summary>
        /// <param name="policyKey">The unique, used-definable key to assign to this <see cref="IAsyncPolicy{TResult}"/> instance.</param>
        IAsyncPolicy<TResult> WithPolicyKey(string policyKey);

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <returns>The value returned by the action</returns>
        UniTask<TResult> ExecuteAsync(Func<UniTask<TResult>> action);

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="context">Context data that is passed to the exception policy.</param>
        /// <returns>The value returned by the action</returns>
        UniTask<TResult> ExecuteAsync(Func<Context, UniTask<TResult>> action, Context context);

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="contextData">Arbitrary data that is passed to the exception policy.</param>
        /// <returns>The value returned by the action</returns>
        UniTask<TResult> ExecuteAsync(Func<Context, UniTask<TResult>> action, IDictionary<string, object> contextData);

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="cancellationToken">A cancellation token which can be used to cancel the action.  When a retry policy is in use, also cancels any further retries.</param>
        /// <returns>The value returned by the action</returns>
        UniTask<TResult> ExecuteAsync(Func<CancellationToken, UniTask<TResult>> action, CancellationToken cancellationToken);

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="contextData">Arbitrary data that is passed to the exception policy.</param>
        /// <param name="cancellationToken">A cancellation token which can be used to cancel the action.  When a retry policy in use, also cancels any further retries.</param>
        /// <returns>The value returned by the action</returns>
        UniTask<TResult> ExecuteAsync(Func<Context, CancellationToken, UniTask<TResult>> action, IDictionary<string, object> contextData, CancellationToken cancellationToken);

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="context">Context data that is passed to the exception policy.</param>
        /// <param name="cancellationToken">A cancellation token which can be used to cancel the action.  When a retry policy is in use, also cancels any further retries.</param>
        /// <returns>The value returned by the action</returns>
        UniTask<TResult> ExecuteAsync(Func<Context, CancellationToken, UniTask<TResult>> action, Context context, CancellationToken cancellationToken);

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        /// <param name="cancellationToken">A cancellation token which can be used to cancel the action.  When a retry policy is in use, also cancels any further retries.</param>
        /// <returns>The value returned by the action</returns>
        /// <exception cref="InvalidOperationException">Please use asynchronous-defined policies when calling asynchronous ExecuteAsync (and similar) methods.</exception>
        UniTask<TResult> ExecuteAsync(Func<CancellationToken, UniTask<TResult>> action, CancellationToken cancellationToken, bool continueOnCapturedContext);

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="contextData">Arbitrary data that is passed to the exception policy.</param>
        /// <param name="cancellationToken">A cancellation token which can be used to cancel the action.  When a retry policy in use, also cancels any further retries.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        /// <returns>The value returned by the action</returns>
        /// <exception cref="ArgumentNullException">contextData</exception>
        UniTask<TResult> ExecuteAsync(Func<Context, CancellationToken, UniTask<TResult>> action, IDictionary<string, object> contextData, CancellationToken cancellationToken, bool continueOnCapturedContext);

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="context">Context data that is passed to the exception policy.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        /// <param name="cancellationToken">A cancellation token which can be used to cancel the action.  When a retry policy is in use, also cancels any further retries.</param>
        /// <returns>The value returned by the action</returns>
        /// <exception cref="InvalidOperationException">Please use asynchronous-defined policies when calling asynchronous ExecuteAsync (and similar) methods.</exception>
        UniTask<TResult> ExecuteAsync(Func<Context, CancellationToken, UniTask<TResult>> action, Context context, CancellationToken cancellationToken, bool continueOnCapturedContext);

        /// <summary>
        /// Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <returns>The captured result</returns>
        UniTask<PolicyResult<TResult>> ExecuteAndCaptureAsync(Func<UniTask<TResult>> action);

        /// <summary>
        /// Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="contextData">Arbitrary data that is passed to the exception policy.</param>
        /// <exception cref="ArgumentNullException">contextData</exception>
        /// <returns>The captured result</returns>
        UniTask<PolicyResult<TResult>> ExecuteAndCaptureAsync(Func<Context, UniTask<TResult>> action, IDictionary<string, object> contextData);

        /// <summary>
        /// Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="context">Context data that is passed to the exception policy.</param>
        /// <returns>The captured result</returns>
        UniTask<PolicyResult<TResult>> ExecuteAndCaptureAsync(Func<Context, UniTask<TResult>> action, Context context);

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="cancellationToken">A cancellation token which can be used to cancel the action.  When a retry policy in use, also cancels any further retries.</param>
        /// <returns>The captured result</returns>
        UniTask<PolicyResult<TResult>> ExecuteAndCaptureAsync(Func<CancellationToken, UniTask<TResult>> action, CancellationToken cancellationToken);

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="contextData">Arbitrary data that is passed to the exception policy.</param>
        /// <param name="cancellationToken">A cancellation token which can be used to cancel the action.  When a retry policy in use, also cancels any further retries.</param>
        /// <exception cref="ArgumentNullException">contextData</exception>
        /// <returns>The captured result</returns>
        UniTask<PolicyResult<TResult>> ExecuteAndCaptureAsync(Func<Context, CancellationToken, UniTask<TResult>> action, IDictionary<string, object> contextData, CancellationToken cancellationToken);

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="context">A cancellation token which can be used to cancel the action.  When a retry policy in use, also cancels any further retries.</param>
        /// <param name="cancellationToken">A cancellation token which can be used to cancel the action.  When a retry policy in use, also cancels any further retries.</param>
        /// <returns>The captured result</returns>
        UniTask<PolicyResult<TResult>> ExecuteAndCaptureAsync(Func<Context, CancellationToken, UniTask<TResult>> action, Context context, CancellationToken cancellationToken);

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="cancellationToken">A cancellation token which can be used to cancel the action.  When a retry policy in use, also cancels any further retries.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        /// <returns>The captured result</returns>
        /// <exception cref="InvalidOperationException">Please use asynchronous-defined policies when calling asynchronous ExecuteAsync (and similar) methods.</exception>
        UniTask<PolicyResult<TResult>> ExecuteAndCaptureAsync(Func<CancellationToken, UniTask<TResult>> action, CancellationToken cancellationToken, bool continueOnCapturedContext);

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="contextData">Arbitrary data that is passed to the exception policy.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        /// <param name="cancellationToken">A cancellation token which can be used to cancel the action.  When a retry policy in use, also cancels any further retries.</param>
        /// <returns>The captured result</returns>
        /// <exception cref="ArgumentNullException">contextData</exception>
        UniTask<PolicyResult<TResult>> ExecuteAndCaptureAsync(Func<Context, CancellationToken, UniTask<TResult>> action, IDictionary<string, object> contextData, CancellationToken cancellationToken, bool continueOnCapturedContext);

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="context">Context data that is passed to the exception policy.</param>
        /// <param name="cancellationToken">A cancellation token which can be used to cancel the action.  When a retry policy in use, also cancels any further retries.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        /// <returns>The captured result</returns>
        /// <exception cref="InvalidOperationException">Please use asynchronous-defined policies when calling asynchronous ExecuteAsync (and similar) methods.</exception>
        UniTask<PolicyResult<TResult>> ExecuteAndCaptureAsync(Func<Context, CancellationToken, UniTask<TResult>> action, Context context, CancellationToken cancellationToken, bool continueOnCapturedContext);
    }
}
