using System.Collections.Generic;
using System.Linq;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Library;
using MediaBrowser.Controller.Plugins;
using MediaBrowser.Model.Entities;
using SmartPlaylist.Comparers;
using SmartPlaylist.Handlers.Commands;
using SmartPlaylist.Infrastructure.MessageBus;
using SmartPlaylist.Infrastructure.Queue;

namespace SmartPlaylist
{
    public class EntryPoint : IServerEntryPoint
    {
        private readonly ILibraryManager _libraryManager;
        private readonly MessageBus _messageBus;
        private readonly AutoDequeueQueue<BaseItem> _updatedItemsQueue;
        private readonly IUserDataManager _userDataManager;

        public EntryPoint(IUserDataManager userDataManager,
            ILibraryManager libraryManager)
        {
            _userDataManager = userDataManager;
            _libraryManager = libraryManager;

            _messageBus = Plugin.Instance.MessageBus;
            _updatedItemsQueue = new AutoDequeueQueue<BaseItem>(OnDequeue, Const.UpdatedItemsQueueConfig);
        }


        public void Run()
        {
            _userDataManager.UserDataSaved += _userDataManager_UserDataSaved;
            _libraryManager.ItemAdded += libraryManager_ItemAdded;
            _libraryManager.ItemUpdated += libraryManager_ItemUpdated;
        }

        public void Dispose()
        {
            _userDataManager.UserDataSaved -= _userDataManager_UserDataSaved;
            _libraryManager.ItemAdded -= libraryManager_ItemAdded;
            _libraryManager.ItemUpdated -= libraryManager_ItemUpdated;
            _updatedItemsQueue.Dispose();
        }

        private void OnDequeue(IEnumerable<BaseItem> items)
        {
            items = items.Distinct(new BaseItemEqualByInternalId());
            _messageBus.Publish(new UpdateAllSmartPlaylistsCommand(items.ToArray()));
        }

        private void libraryManager_ItemAdded(object sender, ItemChangeEventArgs e)
        {
            if (Const.ListenForChangeItemTypes.Contains(e.Item.GetType()) &&
                !_updatedItemsQueue.Items.Any(x => x.InternalId == e.Item.InternalId))
                _updatedItemsQueue.Enqueue(e.Item);
        }


        private void libraryManager_ItemUpdated(object sender, ItemChangeEventArgs e)
        {
            if (Const.ListenForChangeItemTypes.Contains(e.Item.GetType()) &&
                !_updatedItemsQueue.Items.Any(x => x.InternalId == e.Item.InternalId))
                _updatedItemsQueue.Enqueue(e.Item);
        }

        private void _userDataManager_UserDataSaved(object sender, UserDataSaveEventArgs e)
        {
            if (e.SaveReason == UserDataSaveReason.PlaybackFinished ||
                e.SaveReason == UserDataSaveReason.UpdateUserRating || e.SaveReason == UserDataSaveReason.TogglePlayed)
                if (Const.ListenForChangeItemTypes.Contains(e.Item.GetType()))
                {
                    e.Item.IsFavorite = e.UserData.IsFavorite;
                    e.Item.PlayCount = e.UserData.PlayCount;
                    e.Item.LastPlayedDate = e.UserData.LastPlayedDate;
                    e.Item.Played = e.UserData.Played;
                    e.Item.CustomRating = e.UserData.Rating?.ToString() ?? "None";
                    _updatedItemsQueue.Enqueue(e.Item);
                }
        }
    }
}