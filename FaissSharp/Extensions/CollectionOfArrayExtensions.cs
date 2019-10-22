using System.Collections.Generic;
using System.Linq;

namespace FaissSharp.Extensions
{
    public static class CollectionOfArrayExtensions
    {
        public static T[] Flatten<T>(this IEnumerable<T[]> data)
        {
            return data.SelectMany(v => v).ToArray();
        }
        public static T[] Flatten<T>(this IList<T[]> data)
        {
            return data.SelectMany(v => v).ToArray();
        }
    }
}