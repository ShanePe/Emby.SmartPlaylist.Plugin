using System;
using System.Threading.Tasks;
using MediaBrowser.Model.Logging;

namespace SmartPlaylist.Infrastructure.MessageBus.Decorators
{
    public class SuppressAsyncExceptionDecorator<T> : IMessageHandlerAsync<T> where T : IMessage
    {
        private readonly IMessageHandlerAsync<T> _decorated;
        private readonly ILogger _logger;

        public SuppressAsyncExceptionDecorator(IMessageHandlerAsync<T> decorated, ILogger logger)
        {
            _decorated = decorated;
            _logger = logger;
        }

        public async Task HandleAsync(T message)
        {
            try
            {
                await _decorated.HandleAsync(message).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                _logger.ErrorException($"[SmartPlaylist][Error]: {e.Message}", e);
            }
        }
    }
}