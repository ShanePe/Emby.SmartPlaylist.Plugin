using System.Threading.Tasks;
using SmartPlaylist.Infrastructure.MessageBus;
using SmartPlaylist.Services.SmartPlaylist;
using SmartPlaylist.Extensions;
using System.Linq;
using SmartPlaylist.Services;
using SmartPlaylist.Domain;
using MediaBrowser.Controller.Entities;
using System;
using System.Diagnostics;
using SmartPlaylist;

public class SortAllSmartPlaylistsCommandHandler : IMessageHandlerAsync<SortAllSmartPlaylistsCommand>
{
    public SortAllSmartPlaylistsCommandHandler(ISmartPlaylistProvider smartPlaylistProvider, IFolderRepository folderRepository,
                            IUserItemsProvider userItemsProvider, IFolderItemsUpdater collectionUpdater, IFolderItemsUpdater playlistUpdater,
                            ISmartPlaylistStore smartPlaylistStore)
    {
        SmartPlaylistProvider = smartPlaylistProvider;
        FolderRepository = folderRepository;
        UserItemsProvider = userItemsProvider;
        CollectionUpdater = collectionUpdater;
        PlaylistUpdater = playlistUpdater;
        SmartPlaylistStore = smartPlaylistStore;
    }

    public ISmartPlaylistProvider SmartPlaylistProvider { get; }
    public IFolderRepository FolderRepository { get; }
    public IUserItemsProvider UserItemsProvider { get; }
    public IFolderItemsUpdater CollectionUpdater { get; }
    public IFolderItemsUpdater PlaylistUpdater { get; }
    public ISmartPlaylistStore SmartPlaylistStore { get; }

    public async Task HandleAsync(SortAllSmartPlaylistsCommand message)
    {
        var smartPlaylists =
                await SmartPlaylistProvider.GetAllSortableSmartPlaylistsAsync().ConfigureAwait(false);

        smartPlaylists.ForEach((SmartPlaylist.Domain.SmartPlaylist smartPlaylist) =>
        {
            Stopwatch sw = new Stopwatch();

            try
            {
                sw.Start();
                smartPlaylist.SortJob.LastSyncDuration = 0;

                UserFolder folder = FolderRepository.GetUserPlaylistOrCollectionFolder(smartPlaylist);

                BaseItem[] currentItems = folder.GetItems();
                BaseItem[] sortedItems = Sorter.Sort(currentItems, smartPlaylist, smartPlaylist.SortJob.GetOrders());

                var updater = (smartPlaylist.SmartType == SmartPlaylist.Domain.SmartType.Collection ? CollectionUpdater : PlaylistUpdater);
                updater.ClearPlaylist(folder);
                updater.UpdateAsync(folder, sortedItems);

                smartPlaylist.SortJob.Status = $"Sorted {sortedItems.Length} Items successfully";
                smartPlaylist.SortJob.LastRan = DateTime.Now;
                smartPlaylist.SortJob.SyncCount++;

            }
            catch (Exception ex)
            {
                Plugin.Instance.Logger.Error($"Error sorting smart playlist: {ex.Message}", smartPlaylist);
                smartPlaylist.SortJob.Status = "Error: " + ex.Message;
            }
            finally
            {
                sw.Stop();
                smartPlaylist.SortJob.UpdateNextUpdate();
                smartPlaylist.SortJob.LastSyncDuration = sw.ElapsedMilliseconds;
                SmartPlaylistStore.Save(smartPlaylist.ToDto());
            }
        });
    }
}