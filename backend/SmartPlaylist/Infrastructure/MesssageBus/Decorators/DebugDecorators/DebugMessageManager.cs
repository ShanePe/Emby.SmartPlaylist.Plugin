using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Controller.Session;
using MediaBrowser.Model.Session;

namespace SmartPlaylist.Infrastructure.MessageBus.Decorators.DebugDecorators
{
    public class DebugMessageManager
    {
        private readonly ISessionManager _sessionManager;

        public DebugMessageManager(ISessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }

        public async Task ShowAsync(string text)
        {
            var command = new MessageCommand
            {
                Header = "head",
                TimeoutMs = (long?)TimeSpan.FromSeconds(10).TotalMilliseconds,
                Text = text
            };

            foreach (var session in _sessionManager.Sessions.ToList())
                await ShowMessage(session, command).ConfigureAwait(false);
        }

        private async Task ShowMessage(SessionInfo session, MessageCommand command)
        {
            await _sessionManager.SendMessageCommand(session.Id, session.Id, command, CancellationToken.None)
                .ConfigureAwait(false);
        }
    }
}