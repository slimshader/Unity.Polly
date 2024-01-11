using Cysharp.Threading.Tasks;

namespace Polly.Utilities
{
    /// <summary>
    /// UniTask helpers.
    /// </summary>
    public static class TaskHelper
    {
        /// <summary>
        /// Defines a completed UniTask for use as a completed, empty asynchronous delegate.
        /// </summary>
        public static UniTask EmptyTask =
#if NETSTANDARD1_1
            UniTask.FromResult(true)
#else
            UniTask.CompletedTask
#endif
            ;
    }
}
