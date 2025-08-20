using System;
using System.Collections.Generic;

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
    }
}