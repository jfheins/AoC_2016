using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Day_11
{
    public static class HelperExtensions
    {

        public static IEnumerable<int> IndexWhere<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            return source.Select((value, index) => new {value, index})
                .Where(x => predicate(x.value))
                .Select(x => x.index);
        }

        public static IEnumerable<T> AsEnumerable<T>(this T item)
        {
            yield return item;
        }
    }
}
