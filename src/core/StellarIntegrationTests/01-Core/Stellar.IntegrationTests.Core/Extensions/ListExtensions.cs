using System;
using System.Collections.Generic;
using System.Linq;

namespace Stellar.IntegrationTests.Core.Extensions
{
    public static class ListExtensions
    {
        public static IEnumerable<T> Randomize<T>(this List<T> list)
        {
            return list.OrderBy(x => Guid.NewGuid());
        }

        public static T Random<T>(this List<T> list)
        {
            return list.OrderBy(x => Guid.NewGuid()).First();
        }
    }
}
