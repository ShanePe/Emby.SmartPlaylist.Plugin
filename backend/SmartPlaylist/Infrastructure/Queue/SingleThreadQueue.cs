using System;
using System.Collections.Concurrent;
using System.Threading;

namespace SmartPlaylist.Infrastructure.Queue
{
    public class SingleThreadQueue<T> : IDisposable
    {
        private readonly BlockingCollection<T> _items = new BlockingCollection<T>();
        private readonly Action<T> _onConsumeItem;

        public SingleThreadQueue(Action<T> onConsumeItem)
        {
            _onConsumeItem = onConsumeItem;
            var thread = new Thread(e => OnHandlerStart())
            { IsBackground = true };
            thread.Start();
        }

        public void Dispose()
        {
            _items.CompleteAdding();
            _items?.Dispose();
        }

        public void Enqueue(T item)
        {
            if (!_items.IsAddingCompleted)
                _items.Add(item);
        }

        private void OnHandlerStart()
        {
            foreach (var item in _items.GetConsumingEnumerable(CancellationToken.None))
                _onConsumeItem(item);
        }
    }
}