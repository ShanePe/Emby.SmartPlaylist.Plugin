using System.Diagnostics;
using MediaBrowser.Controller.Entities;
using SmartPlaylist.Domain;
using SmartPlaylist.Services;
using System.Linq;
using SmartPlaylist.Infrastructure;
using System.Threading.Tasks;
using System;
using SmartPlaylist;
using SmartPlaylist.Services.SmartPlaylist;
using SmartPlaylist.Handlers.Commands;
using System.Threading;
using System.Collections.Generic;

namespace SmartPlaylist.Handlers.CommandHandlers
{
    public class SmartPlaylistUpdater
    {
        private readonly IFolderRepository _folderRepository;
        private readonly IFolderItemsUpdater _playlistItemsUpdater;
        private readonly IFolderItemsUpdater _collectionItemsUpdater;
        private readonly ISmartPlaylistStore _smartPlaylistStore;
        private readonly IUserItemsProvider _userItemsProvider;
        private readonly ExecutionModes _executionMode;
        public SmartPlaylistUpdater(IFolderRepository folderRepository, IFolderItemsUpdater playlistItemsUpdater, IFolderItemsUpdater collectionItemsUpdater, ISmartPlaylistStore smartPlaylistStore, ExecutionModes executionMode) :
        this(folderRepository, playlistItemsUpdater, collectionItemsUpdater, smartPlaylistStore, executionMode, null)
        { }

        public SmartPlaylistUpdater(IFolderRepository folderRepository, IFolderItemsUpdater playlistItemsUpdater, IFolderItemsUpdater collectionItemsUpdater, ISmartPlaylistStore smartPlaylistStore, ExecutionModes executionMode, IUserItemsProvider userItemsProvider)
        {
            _folderRepository = folderRepository;
            _playlistItemsUpdater = playlistItemsUpdater;
            _collectionItemsUpdater = collectionItemsUpdater;
            _smartPlaylistStore = smartPlaylistStore;
            _userItemsProvider = userItemsProvider;
            _executionMode = executionMode;
        }

        public BaseItem[] GetItemsToProcess(SmartPlaylist.Domain.SmartPlaylist smartPlaylist, BaseItem[] newItems, BaseItem[] folderItems)
        {
            BaseItem[] ret = null;
            if (newItems == null)
            {
                ret = folderItems;
            }
            else if (folderItems == null)
            {
                return newItems;
            }
            else if (smartPlaylist.UpdateType == UpdateType.Live || (smartPlaylist.IsShuffleUpdateType && smartPlaylist.MonitorMode && !smartPlaylist.IsShuffleDue()))
            {
                ret = newItems;
            }
            else
            {
                ret = folderItems.Union(newItems, new BaseItemComparer()).ToArray();
            }

            return ret == null ? new BaseItem[] { } : ret;
        }

        public async Task Update(SmartPlaylist.Domain.SmartPlaylist smartPlaylist)
        {
            await Update(smartPlaylist, null);
        }
        public async Task Update(SmartPlaylist.Domain.SmartPlaylist smartPlaylist, BaseItem[] items)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            smartPlaylist.LastSyncDuration = 0;
            smartPlaylist.Status = smartPlaylist.Enabled ? "Complete" : "Disabled";
            if (!smartPlaylist.Enabled)
                return;
            try
            {
                smartPlaylist.Log($"Execution triggered by '{_executionMode}'");
                (UserFolder user, BaseItem[] items) folder = await _folderRepository.GetBaseItemsForSmartPlayList(smartPlaylist, _userItemsProvider);
                BaseItem[] processItems = GetItemsToProcess(smartPlaylist, items, folder.items);

                smartPlaylist.Log($"Dealing with {processItems.Length} media items from source.");
                smartPlaylist.Log($"Query: {smartPlaylist.ExplainRules()}");
                BaseItem[] newItems;
                using (PerfLogger.Create("FilterPlaylistItems",
                    () => new { playlistName = folder.user.SmartPlaylist.Name, itemsCount = processItems.Length }))
                {
                    newItems = smartPlaylist.FilterPlaylistItems(folder.user, processItems).ToArray();
                }

                var update = (smartPlaylist.SmartType == SmartPlaylist.Domain.SmartType.Collection ? _collectionItemsUpdater : _playlistItemsUpdater)
                    .UpdateAsync(folder.user, newItems);

                smartPlaylist.Status = update.message;

                if (smartPlaylist.InternalId != update.internalId)
                {
                    if (smartPlaylist.InternalId > 0)
                        _folderRepository.Remove(smartPlaylist);

                    smartPlaylist.InternalId = update.internalId;
                }
                smartPlaylist.LastSync = DateTime.Now;
                smartPlaylist.SyncCount++;

                if (smartPlaylist.IsShuffleUpdateType || smartPlaylist.IsScheduledType)
                    smartPlaylist.UpdateLastShuffleTime();
            }
            catch (Exception ex)
            {
                smartPlaylist.Status = $"Error {ex.Message}";
                Logger.Instance?.LogError(ex);
            }
            finally
            {
                sw.Stop();
                try
                {
                    smartPlaylist.LastSyncDuration = sw.ElapsedMilliseconds;
                    _smartPlaylistStore.Save(smartPlaylist.ToDto());
                    smartPlaylist.Log("Complete");
                    await _smartPlaylistStore.WriteToLogAsync(smartPlaylist);
                }
                catch (Exception ex)
                {
                    Logger.Instance?.LogError(ex);
                }
            }
        }
    }

    public class BaseItemComparer : IEqualityComparer<BaseItem>
    {
        public bool Equals(BaseItem x, BaseItem y)
        {
            return x.Id.CompareTo(y.Id) == 0;
        }

        public int GetHashCode(BaseItem obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}