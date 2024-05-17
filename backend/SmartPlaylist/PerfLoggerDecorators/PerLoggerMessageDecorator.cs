using System.Threading.Tasks;
using SmartPlaylist.Infrastructure;
using SmartPlaylist.Infrastructure.MessageBus;

namespace SmartPlaylist.PerfLoggerDecorators
{
    public class PerLoggerMessageDecorator<T> : IMessageHandlerAsync<T> where T : IMessage
    {
        private readonly IMessageHandlerAsync<T> _decorated;

        public PerLoggerMessageDecorator(IMessageHandlerAsync<T> decorated)
        {
            _decorated = decorated;
        }

        public async Task HandleAsync(T message)
        {
            using (PerfLogger.Create(message.GetType().Name))
            {
                await _decorated.HandleAsync(message).ConfigureAwait(false);
            }
        }
    }
}