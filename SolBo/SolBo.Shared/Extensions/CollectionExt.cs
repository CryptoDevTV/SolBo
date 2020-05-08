using System.Collections.Generic;
using System.Linq;

namespace SolBo.Shared.Extensions
{
    public static class CollectionExt
    {
        public static bool AnyAndNotNull<T>(this IEnumerable<T> source)
            => source != null && source.Any();
    }
}