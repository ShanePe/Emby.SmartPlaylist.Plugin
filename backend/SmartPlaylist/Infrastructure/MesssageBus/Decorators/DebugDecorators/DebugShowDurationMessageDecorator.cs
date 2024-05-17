using System.Diagnostics;
using System.Threading.Tasks;
using MediaBrowser.Controller.Session;

namespace SmartPlaylist.Infrastructure.MessageBus.Decorators.DebugDecorators
{
    public class DebugShowDurationMessageDecorator<T> : IMessageHandlerAsync<T> where T : IMessage
    {
        private readonly DebugMessageManager _debugMessageManager;
        private readonly IMessageHandlerAsync<T> _decorated;

        public DebugShowDurationMessageDecorator(IMessageHandlerAsync<T> decorated,
            ISessionManager sessionManager)
        {
            _decorated = decorated;
            _debugMessageManager = new DebugMessageManager(sessionManager);
        }

        public async Task HandleAsync(T message)
        {
            var sw = new Stopwatch();
            sw.Start();

            await _decorated.HandleAsync(message).ConfigureAwait(false);

            sw.Stop();

            await _debugMessageManager.ShowAsync($"{message.GetType().Name}:ActionDuration: {FormatElapsedTime(sw)}")
                .ConfigureAwait(false);
        }

        private static string FormatElapsedTime(Stopwatch sw)
        {
            return sw.Elapsed.ToString(@"m\:ss\.fff");
        }
    }
}