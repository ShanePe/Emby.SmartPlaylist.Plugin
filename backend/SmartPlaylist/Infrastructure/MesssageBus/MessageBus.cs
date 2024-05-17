using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartPlaylist.Infrastructure.Queue;

namespace SmartPlaylist.Infrastructure.MessageBus
{
    public class MessageBus : IDisposable
    {
        private readonly IList<IMessageHandlerDescriptor> _handlers;
        private readonly SingleThreadQueue<IMessage> _queue;

        public MessageBus()
        {
            _handlers = new List<IMessageHandlerDescriptor>();
            _queue = new SingleThreadQueue<IMessage>(OnDequeue);
        }

        public void Dispose()
        {
            _queue.Dispose();
        }

        private void OnDequeue(IMessage message)
        {
            var handlers = _handlers.Where(x => x.ParameterType == message.GetType()).ToList();
            handlers.ForEach(handler => handler.Handle(message));
        }

        public void Subscribe<T>(IMessageHandler<T> handler) where T : IMessage
        {
            _handlers.Add(new MessageHandlerDescriptor<T>(handler, typeof(T)));
        }

        public void Subscribe<T>(IMessageHandlerAsync<T> handler) where T : IMessage
        {
            _handlers.Add(new MessageHandlerDescriptor<T>(handler, typeof(T)));
        }

        public void Publish(IMessage command)
        {
            _queue.Enqueue(command);
        }

        private class MessageHandlerDescriptor<T> : IMessageHandlerDescriptor where T : IMessage
        {
            private readonly Action<T> _handle;

            public MessageHandlerDescriptor(IMessageHandler<T> handler, Type type)
            {
                _handle = handler.Handle;
                ParameterType = type;
            }

            public MessageHandlerDescriptor(IMessageHandlerAsync<T> handler, Type type)
            {
                _handle = p =>
                    Task.Run(async () => await handler.HandleAsync(p).ConfigureAwait(false))
                        .Wait(TimeSpan.FromSeconds(30));
                ParameterType = type;
            }

            public Type ParameterType { get; }

            public void Handle(IMessage message)
            {
                _handle((T)message);
            }
        }

        private interface IMessageHandlerDescriptor
        {
            Type ParameterType { get; }
            void Handle(IMessage message);
        }
    }
}