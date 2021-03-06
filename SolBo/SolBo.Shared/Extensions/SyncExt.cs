using System;
using System.Threading.Tasks;

namespace SolBo.Shared.Extensions
{
    public class SyncExt
    {
        public static TResult RunSync<TResult>(Func<Task<TResult>> method)
        {
            var task = method();
            return task.GetAwaiter().GetResult();
        }

        public static void RunSync(Func<Task> method)
        {
            var task = method();
            task.GetAwaiter().GetResult();
        }
    }
}