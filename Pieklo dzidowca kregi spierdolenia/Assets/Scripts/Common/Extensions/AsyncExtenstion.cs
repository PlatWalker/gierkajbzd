using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

namespace jbzd.Common.Extensions
{
    /// <summary>
    /// Let's you use C# "await" for unity "AsyncOperation"
    /// </summary>
    public static class AsyncExtenstion
    {
        public static TaskAwaiter GetAwaiter(this AsyncOperation asyncOp)
        {
            var tcs = new TaskCompletionSource<AsyncOperation>();
            asyncOp.completed += operation => { tcs.SetResult(operation); };
            return ((Task)tcs.Task).GetAwaiter();
        }
    }
}