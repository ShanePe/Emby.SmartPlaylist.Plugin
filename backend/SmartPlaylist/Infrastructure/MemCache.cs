using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SmartPlaylist.Extensions;

namespace SmartPlaylist.Infrastructure
{
    public class MemCache
    {
        private readonly Dictionary<object, object> _items = new Dictionary<object, object>();
        private readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);
        private DateTimeOffset? _lastCacheUpdateTime;

        public async Task<T> GetOrCreateAsync<T>(object key, Func<Task<T>> crateFunc)
        {
            await _lock.WaitAsync(Const.DefaultSemaphoreSlimTimeout).ConfigureAwait(false);
            return _lock.SafeExecute(() =>
            {
                object item = null;
                var hasItem = false;
                hasItem = _items.TryGetValue(key, out item);
                if (!hasItem)
                {
                    item = crateFunc();
                    _items[key] = item;
                }

                return (T)item;
            });
        }

        public async Task<IEnumerable<object>> GetOrCreateManyAsync(
            Func<Task<Dictionary<object, object>>> createAllFunc, TimeSpan absoluteExpiration)
        {
            await _lock.WaitAsync(Const.DefaultSemaphoreSlimTimeout).ConfigureAwait(false);
            return await _lock.SafeExecute(async () =>
            {
                if (IsValidGetAllCache(absoluteExpiration)) return _items.Values;

                var items = await createAllFunc();
                items
                    .ToList()
                    .ForEach(x => _items[x.Key] = x.Value);

                _lastCacheUpdateTime = DateTimeOffset.Now;
                return items.Values;
            });
        }

        private bool IsValidGetAllCache(TimeSpan absoluteExpiration)
        {
            return _lastCacheUpdateTime.HasValue && _lastCacheUpdateTime + absoluteExpiration >= DateTimeOffset.Now;
        }

        public void Set(object key, object value)
        {
            _lock.Wait(TimeSpan.FromSeconds(30));
            _lock.SafeExecute(() => { _items[key] = value; });
        }

        public void Remove(object key)
        {
            _lock.Wait(TimeSpan.FromSeconds(30));
            _lock.SafeExecute(() => { _items.Remove(key); });
        }

        public void Invalidate()
        {
            _lock.Wait(TimeSpan.FromSeconds(30));
            _lock.SafeExecute(() =>
            {
                _lastCacheUpdateTime = null;
                _items.Clear();
            });
        }
    }
}