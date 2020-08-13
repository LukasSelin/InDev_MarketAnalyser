using System;
using System.Collections.Generic;
using System.Text;

namespace AlbionMarketChecker
{
    public static class Extensions
    {
        public static bool IsNotEmpty<T>(this List<T> list)
        {
            return 0 < list.Count;
        }

    }
}
