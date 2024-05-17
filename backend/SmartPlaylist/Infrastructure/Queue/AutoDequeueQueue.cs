using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using SmartPlaylist.Extensions;
using System.Threading;
using System.Threading.Tasks;

namespace SmartPlaylist.Infrastructure.Queue
{
    public class AutoDequeueQueue<T> : IDisposable
    {
        private readonly AutoDequeueQueueConfig _config;
        private readonly ConcurrentQueue<T> _items = new ConcurrentQueue<T>();
        //private readonly object _lock = new object();
        private SemaphoreSlim _semaphore = new SemaphoreSlim(1);
        private readonly Action<IEnumerable<T>> _onDequeue;
        private readonly TimeSpan _period = TimeSpan.FromHours(1);
        private Timer _absoluteTimer;
        private Timer _timer;

        public ConcurrentQueue<T> Items => _items;
        public AutoDequeueQueue(Action<IEnumerable<T>> onDequeue, AutoDequeueQueueConfig config)
        {
            _onDequeue = onDequeue;
            _config = config;
        }

        public void Dispose()
        {
            StopTimer();
        }

        public void Enqueue(T item)
        {
            try
            {
                _semaphore.Wait(1000);
                if (_items.Count < _config.MaxItemsLimit)
                {
                    StartOrUpdateTimer();
                    _items.Enqueue(item);
                }
                else
                {
                    StopTimer();
                    OnTimerCallback(null);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding item to live queue: {ex.Message}");
            }
            finally
            {
                _semaphore.Release();
            }

        }

        private void StartOrUpdateTimer()
        {
            if (_timer == null)
            {
                _timer = CreateTimer(_config.InactiveDequeueTime);
                _absoluteTimer = CreateTimer(_config.AbsoluteDequeueTime);
            }
            else
            {
                _timer.Change(_config.InactiveDequeueTime, _period);
            }
        }

        private Timer CreateTimer(TimeSpan dueTime)
        {
            return new Timer(OnTimerCallback, null, dueTime, _period);
        }

        private void OnTimerCallback(object state)
        {
            try
            {
                _semaphore.Wait(5000);
                Dequeue();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private void Dequeue()
        {
            StopTimer();
            var items = Items.DequeueAll();
            Task.Run(() => _onDequeue(items));
        }

        private void StopTimer()
        {
            _absoluteTimer?.Dispose();
            _timer?.Dispose();
            _timer = null;
            _absoluteTimer = null;
        }
    }
}