using System;
using System.Collections.Generic;
using System.Linq;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.Profile.Simulation;

namespace SpaceMonkey.Scripts.Utilities
{
    public static class Extensions
    {
        private static readonly System.Random _sysRandom = new System.Random();

        public static T PickRandomElement<T>(this IEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            // Fast path: IList<T> allows O(1) indexing
            if (source is IList<T> list)
            {
                if (list.Count == 0) throw new InvalidOperationException("Sequence contains no elements.");
                return list[UnityEngine.Random.Range(0, list.Count)];
            }

            // Fallback: reservoir sampling for enumerables (O(n) with single pass)
            using var enumerator = source.GetEnumerator();
            if (!enumerator.MoveNext())
                throw new InvalidOperationException("Sequence contains no elements.");

            T chosen = enumerator.Current;
            int index = 1;

            while (enumerator.MoveNext())
            {
                index++;
                if (_sysRandom.Next(index) == 0)
                    chosen = enumerator.Current;
            }

            return chosen;
        }

        public static List<T> PickRandomElements<T>(this IEnumerable<T> source, int count)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            // Materialize to a list for indexing
            var list = source as IList<T> ?? source.ToList();

            if (list.Count == 0)
                throw new InvalidOperationException("Sequence contains no elements.");

            // Clamp count
            count = Math.Min(count, list.Count);

            // Fisher-Yates shuffle on a copy
            var temp = new List<T>(list);
            for (int i = 0; i < temp.Count; i++)
            {
                int j = UnityEngine.Random.Range(i, temp.Count); // UnityEngine RNG
                (temp[i], temp[j]) = (temp[j], temp[i]);
            }

            return temp.GetRange(0, count);
        }


        public static Dictionary<Product, int> GetTotalQuantitiesByProduct(this WeekInfo weekInfo)
        {
            return weekInfo.Orders
                .Where(o => o.Shipped)
                .SelectMany(o => o.Products)
                .GroupBy(p => p.Product)
                .ToDictionary(g => g.Key, g => g.Sum(o => o.Quantity));
        }
    }
}