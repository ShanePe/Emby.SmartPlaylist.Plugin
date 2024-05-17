using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
namespace SmartPlaylist.Extensions
{
    public static class ListExtensions
    {
        public static List<T> ConcateList<T>(params IEnumerable<T>[] e)
        {
            List<T> ret = new List<T>();
            foreach (IEnumerable<T> ee in e)
                ret.AddRange(ee);
            return ret;
        }

        public static IEnumerable<IEnumerable<T>> Partition<T>(this IList<T> source, int size)
        {
            for (int i = 0; i < Math.Ceiling(source.Count / (double)size); i++)
                yield return source.Skip(size * i).Take(size);
        }

        public static IEnumerable<T> DequeueAll<T>(this ConcurrentQueue<T> queue)
        {
            List<T> items = new List<T>();
            while (queue.Count != 0)
            {
                T item = default(T);
                if (queue.TryDequeue(out item))
                    items.Add(item);
            }
            return items;
        }
    }
}