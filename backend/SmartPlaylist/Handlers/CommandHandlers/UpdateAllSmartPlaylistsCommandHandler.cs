using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MediaBrowser.Controller.Entities;
using SmartPlaylist.Domain;
using SmartPlaylist.Handlers.Commands;
using SmartPlaylist.Infrastructure;
using SmartPlaylist.Infrastructure.MessageBus;
using SmartPlaylist.Services;
using SmartPlaylist.Services.SmartPlaylist;

namespace SmartPlaylist.Handlers.CommandHandlers
{
    public class UpdateAllSmartPlaylistsCommandHandler : IMessageHandlerAsync<UpdateAllSmartPlaylistsCommand>
    {
        private readonly MessageBus _messageBus;
        private readonly IFolderItemsUpdater _playlistItemsUpdater;
        private readonly IFolderItemsUpdater _collectionItemsUpdater;
        private readonly IFolderRepository _folderRepository;
        private readonly ISmartPlaylistProvider _smartPlaylistProvider;
        private readonly ISmartPlaylistStore _smartPlaylistStore;

        public UpdateAllSmartPlaylistsCommandHandler(MessageBus messageBus,
            ISmartPlaylistProvider smartPlaylistProvider, IFolderRepository folderRepository,
            IFolderItemsUpdater playlistItemsUpdater, IFolderItemsUpdater collectionItemsUpdater, ISmartPlaylistStore smartPlaylistStore)
        {
            _messageBus = messageBus;
            _smartPlaylistProvider = smartPlaylistProvider;
            _folderRepository = folderRepository;
            _playlistItemsUpdater = playlistItemsUpdater;
            _collectionItemsUpdater = collectionItemsUpdater;
            _smartPlaylistProvider = smartPlaylistProvider;
            _smartPlaylistStore = smartPlaylistStore;
        }

        public async Task HandleAsync(UpdateAllSmartPlaylistsCommand message)
        {
            var smartPlaylists =
                await _smartPlaylistProvider.GetAllUpdateableSmartPlaylistsAsync().ConfigureAwait(false);

            var smartPlaylistToUpdateWithNewItems = GetSmartPlaylistToUpdateWithNewItems(message, smartPlaylists);

            UpdateSmartPlaylistsWithAllUserItems(smartPlaylists.Except(smartPlaylistToUpdateWithNewItems));

            if (smartPlaylistToUpdateWithNewItems.Any())
                await UpdateSmartPlaylistsWithNewItemsAsync(message.Items, smartPlaylistToUpdateWithNewItems)
                    .ConfigureAwait(false);
        }

        private static Domain.SmartPlaylist[] GetSmartPlaylistToUpdateWithNewItems(
            UpdateAllSmartPlaylistsCommand message, Domain.SmartPlaylist[] smartPlaylists)
        {
            return message.HasItems
                ? smartPlaylists.Where(x => x.UpdateType == Domain.UpdateType.Live || (x.IsShuffleUpdateType && x.MonitorMode)).ToArray()
                : new Domain.SmartPlaylist[0];
        }

        private async Task UpdateSmartPlaylistsWithNewItemsAsync(BaseItem[] items,
            Domain.SmartPlaylist[] smartPlaylists)
        {
            foreach (var smartPlaylist in smartPlaylists) await GetTasks(smartPlaylist, items).ConfigureAwait(false);
        }

        private async Task GetTasks(Domain.SmartPlaylist smartPlaylist, BaseItem[] items)
        {
            SmartPlaylistUpdater updater = new SmartPlaylistUpdater(_folderRepository, _playlistItemsUpdater, _collectionItemsUpdater, _smartPlaylistStore, ExecutionModes.Scheduled);
            await updater.Update(smartPlaylist, items);
        }


        private void UpdateSmartPlaylistsWithAllUserItems(IEnumerable<Domain.SmartPlaylist> smartPlaylists)
        {
            smartPlaylists.ToList().ForEach(x => _messageBus.Publish(new UpdateSmartPlaylistCommand(x.Id, ExecutionModes.Scheduled)));
        }
    }
}