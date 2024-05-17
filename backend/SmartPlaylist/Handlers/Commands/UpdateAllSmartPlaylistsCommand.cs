using System.Linq;
using MediaBrowser.Controller.Entities;
using SmartPlaylist.Infrastructure.MessageBus;

namespace SmartPlaylist.Handlers.Commands
{
    public class UpdateAllSmartPlaylistsCommand : IMessage
    {
        public UpdateAllSmartPlaylistsCommand()
        {
            Items = new BaseItem[0];
        }

        public UpdateAllSmartPlaylistsCommand(BaseItem[] items)
        {
            Items = items;
        }

        public BaseItem[] Items { get; }

        public bool HasItems => Items.Any();
    }
}