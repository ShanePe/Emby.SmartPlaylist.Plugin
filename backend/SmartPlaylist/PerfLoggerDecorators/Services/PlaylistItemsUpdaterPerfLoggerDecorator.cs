using System.Threading.Tasks;
using MediaBrowser.Controller.Entities;
using SmartPlaylist.Domain;
using SmartPlaylist.Infrastructure;
using SmartPlaylist.Services;

namespace SmartPlaylist.PerfLoggerDecorators.Services
{
    public class PlaylistItemsUpdaterPerfLoggerDecorator : IFolderItemsUpdater
    {
        private readonly IFolderItemsUpdater _decorated;

        public PlaylistItemsUpdaterPerfLoggerDecorator(IFolderItemsUpdater decorated)
        {
            _decorated = decorated;
        }

        public int ClearPlaylist(UserFolder folder)
        {
            using (PerfLogger.Create("ClearPlaylist",
                () => new { folder }))

            {
                return _decorated.ClearPlaylist(folder);
            }
        }

        public int RemoveItems(UserFolder folder, BaseItem[] currentItems, BaseItem[] newItems)
        {
            using (PerfLogger.Create("RemoveItems",
                () => new { playlistName = folder.SmartPlaylist.Name, newItemsCount = currentItems.Length }))

            {
                return _decorated.RemoveItems(folder, currentItems, newItems);
            }
        }

        public (long internalId, string message) UpdateAsync(UserFolder playlist, BaseItem[] newItems)
        {
            using (PerfLogger.Create("UpdatePlaylistItems",
                () => new { playlistName = playlist.SmartPlaylist.Name, newItemsCount = newItems.Length }))

            {
                return _decorated.UpdateAsync(playlist, newItems);
            }
        }
    }
}