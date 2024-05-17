using System;
using MediaBrowser.Model.Logging;

namespace SmartPlaylist.Infrastructure.MessageBus.Decorators
{
    public class SuppressExceptionDecorator<T> : IMessageHandler<T> where T : IMessage
    {
        private readonly IMessageHandler<T> _decorated;
        private readonly ILogger _logger;

        public SuppressExceptionDecorator(IMessageHandler<T> decorated, ILogger logger)
        {
            _decorated = decorated;
            _logger = logger;
        }

        public void Handle(T message)
        {
            try
            {
                _decorated.Handle(message);
            }
            catch (Exception e)
            {
                _logger.ErrorException(e.Message, e);
            }
        }
    }
}