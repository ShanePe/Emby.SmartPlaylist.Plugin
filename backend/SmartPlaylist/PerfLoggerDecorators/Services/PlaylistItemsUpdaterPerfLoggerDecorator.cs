﻿using System.Threading.Tasks;
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

        public void RemoveItems(UserFolder folder, BaseItem[] currentItems, BaseItem[] newItems)
        {
            using (PerfLogger.Create("RemoveItems",
                () => new { playlistName = folder.SmartPlaylist.Name, newItemsCount = currentItems.Length }))

            {
                _decorated.RemoveItems(folder, currentItems, newItems);
            }
        }

        public async Task<(long internalId, string message)> UpdateAsync(UserFolder playlist, BaseItem[] newItems)
        {
            using (PerfLogger.Create("UpdatePlaylistItems",
                () => new { playlistName = playlist.SmartPlaylist.Name, newItemsCount = newItems.Length }))

            {
                return await _decorated.UpdateAsync(playlist, newItems).ConfigureAwait(false);
            }
        }
    }
}