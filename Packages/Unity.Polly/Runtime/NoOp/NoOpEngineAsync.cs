using Cysharp.Threading.Tasks;
using System;
using System.Threading;

namespace Polly.NoOp
{
    internal static partial class NoOpEngine
    {
        internal static async UniTask<TResult> ImplementationAsync<TResult>(Func<Context, CancellationToken, UniTask<TResult>> action, Context context, CancellationToken cancellationToken, bool continueOnCapturedContext)
            =>  await action(context, cancellationToken);
    }
}
