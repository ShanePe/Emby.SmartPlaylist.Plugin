using System;
using System.Linq;
using System.Threading.Tasks;
using MediaBrowser.Controller.Session;

namespace SmartPlaylist.Infrastructure.MessageBus.Decorators.DebugDecorators
{
    public class DebugShowErrorMessageDecorator<T> : IMessageHandlerAsync<T> where T : IMessage
    {
        private readonly DebugMessageManager _debugMessageManager;
        private readonly IMessageHandlerAsync<T> _decorated;

        public DebugShowErrorMessageDecorator(IMessageHandlerAsync<T> decorated,
            ISessionManager sessionManager)
        {
            _decorated = decorated;
            _debugMessageManager = new DebugMessageManager(sessionManager);
        }

        public async Task HandleAsync(T message)
        {
            try
            {
                await _decorated.HandleAsync(message).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                await _debugMessageManager.ShowAsync($"[ERROR]: {e.ToString().Substring(0, 500)}").ConfigureAwait(false);
                throw;
            }
        }
    }
}