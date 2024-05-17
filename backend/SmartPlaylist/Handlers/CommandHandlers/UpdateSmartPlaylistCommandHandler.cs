using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Library;
using MediaBrowser.Controller.Playlists;
using SmartPlaylist.Domain;
using SmartPlaylist.Handlers.Commands;
using SmartPlaylist.Infrastructure;
using SmartPlaylist.Infrastructure.MessageBus;
using SmartPlaylist.Services;
using SmartPlaylist.Services.SmartPlaylist;

namespace SmartPlaylist.Handlers.CommandHandlers
{
    public class UpdateSmartPlaylistCommandHandler : IMessageHandlerAsync<UpdateSmartPlaylistCommand>
    {
        private readonly IFolderItemsUpdater _playlistItemsUpdater;
        private readonly IFolderItemsUpdater _collectionItemsUpdater;
        private readonly IFolderRepository _folderRepository;
        private readonly ISmartPlaylistProvider _smartPlaylistProvider;
        private readonly ISmartPlaylistStore _smartPlaylistStore;

        private readonly IUserItemsProvider _userItemsProvider;
        private readonly ILibraryManager _libraryManager;

        public UpdateSmartPlaylistCommandHandler(
            IUserItemsProvider userItemsProvider, ISmartPlaylistProvider smartPlaylistProvider,
            IFolderRepository folderRepository, IFolderItemsUpdater playlistItemsUpdater,
            ISmartPlaylistStore smartPlaylistStore, IFolderItemsUpdater collectionItemsUpdater,
            ILibraryManager libraryManager)
        {
            _userItemsProvider = userItemsProvider;
            _smartPlaylistProvider = smartPlaylistProvider;
            _folderRepository = folderRepository;
            _playlistItemsUpdater = playlistItemsUpdater;
            _smartPlaylistStore = smartPlaylistStore;
            _collectionItemsUpdater = collectionItemsUpdater;
            _libraryManager = libraryManager;
        }

        public async Task HandleAsync(UpdateSmartPlaylistCommand message)
        {
            SmartPlaylistUpdater updater = new SmartPlaylistUpdater(_folderRepository, _playlistItemsUpdater, _collectionItemsUpdater, _smartPlaylistStore, message.ExecutionMode, _userItemsProvider);
            await updater.Update(await _smartPlaylistProvider.GetSmartPlaylistAsync(message.SmartPlaylistId).ConfigureAwait(false));
        }
    }
}