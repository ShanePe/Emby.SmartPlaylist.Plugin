using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartPlaylist.Extensions
{
    public static class EnumerableExtensions
    {
        public static T[] Shuffle<T>(this T[] source)
        {
            return source.Shuffle(new Random()).ToArray();
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random rng)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (rng == null) throw new ArgumentNullException("rng");

            return source.ShuffleIterator(rng);
        }

        private static IEnumerable<T> ShuffleIterator<T>(
            this IEnumerable<T> source, Random rng)
        {
            var buffer = source.ToList();
            for (var i = 0; i < buffer.Count; i++)
            {
                var j = rng.Next(i, buffer.Count);
                yield return buffer[j];

                buffer[j] = buffer[i];
            }
        }

        public static IEnumerable<T> Flatten<T>(
            this IEnumerable<T> e,
            Func<T, IEnumerable<T>> f)
        {
            return e.SelectMany(c => f(c).Flatten(f)).Concat(e);
        }

        public static void ForEach<T>(this IEnumerable<T> sequence, Action<T> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            foreach (T item in sequence)
                action(item);
        }

        public static IEnumerable<T> Except<T>(this IEnumerable<T> setA, IEnumerable<T> setB, Func<T, T, bool> f)
        {
            return setA.Except(setB, new ExceptComparer<T>(f));
        }

        private class ExceptComparer<T> : IEqualityComparer<T>
        {
            Func<T, T, bool> _lambda;
            public ExceptComparer(Func<T, T, bool> lambda)
            {
                _lambda = lambda;
            }
            public bool Equals(T x, T y)
            {
                return _lambda(x, y);
            }

            public int GetHashCode(T obj)
            {
                return 0;
            }
        }
    }
}