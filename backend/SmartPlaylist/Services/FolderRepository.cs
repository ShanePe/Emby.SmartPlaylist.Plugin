using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Library;
using MediaBrowser.Controller.Playlists;
using SmartPlaylist.Domain;
using SmartPlaylist.Extensions;
using SmartPlaylist.Handlers.Commands;
using SmartPlaylist.Services.SmartPlaylist;

namespace SmartPlaylist.Services
{
    public abstract class IFolderRepository
    {

        internal readonly ILibraryManager _libraryManager;
        internal readonly IUserManager _userManager;
        internal readonly IFolderItemsUpdater _collectionItemUpdater;
        internal readonly IFolderItemsUpdater _playListItemsUpdater;
        internal readonly ISmartPlaylistStore _playlistStore;

        public IFolderRepository(IUserManager userManager, ILibraryManager libraryManager,
        IFolderItemsUpdater collectionItemUpdater, IFolderItemsUpdater playListItemsUpdater, ISmartPlaylistStore smartPlaylistStore)
        {
            _userManager = userManager;
            _libraryManager = libraryManager;
            _collectionItemUpdater = collectionItemUpdater;
            _playListItemsUpdater = playListItemsUpdater;
            _playlistStore = smartPlaylistStore;
        }

        public IFolderRepository() { }

        internal User GetUser(Guid userId)
        {
            return _userManager.GetUserById(userId);
        }

        public abstract UserFolder GetUserPlaylistOrCollectionFolder(Domain.SmartPlaylist smartPlaylist);
        public abstract UserFolder FindCollection(Domain.SmartPlaylist smartPlaylist);
        public abstract UserFolder FindCollection(Domain.SmartPlaylist smartPlaylist, string collectionName);
        public abstract Folder FindCollectionFolder(Domain.SmartPlaylist smartPlaylist, string collectionName);
        public abstract UserFolder FindPlaylist(Domain.SmartPlaylist smartPlaylist);
        public abstract UserFolder FindPlaylist(Domain.SmartPlaylist smartPlaylist, string playlistName);
        public abstract Playlist FindPlaylistFolder(Domain.SmartPlaylist smartPlaylist, string playlistName);
        public abstract void Remove(Domain.SmartPlaylist smartPlaylist);
        public abstract BaseItem[] GetAllPlaylists();
        public abstract BaseItem[] GetAllCollections();
        public abstract long[] GetItemsIdsForFolderId(Guid folderId, User user);
        public abstract BaseItem[] GetItemsForFolderId(Guid folderId, User user);
        public abstract BaseItem[] GetItemsForFolderId(Domain.SmartPlaylist smartPlaylist, User user);
        public abstract Folder GetFolder(Domain.SmartPlaylist smartPlaylist);

        public abstract Task<(UserFolder, BaseItem[])> GetBaseItemsForSmartPlayList(Domain.SmartPlaylist smartPlaylist, IUserItemsProvider userItemsProvider);
    }

    public class FolderRepository : IFolderRepository
    {

        public FolderRepository(IUserManager userManager, ILibraryManager libraryManager,
            IFolderItemsUpdater collectionItemUpdater, IFolderItemsUpdater playListItemsUpdater, ISmartPlaylistStore smartPlaylistStore)
        : base(userManager, libraryManager, collectionItemUpdater, playListItemsUpdater, smartPlaylistStore) { }

        public override UserFolder GetUserPlaylistOrCollectionFolder(Domain.SmartPlaylist smartPlaylist)
        {
            return smartPlaylist.SmartType == SmartType.Playlist
                ? FindPlaylist(smartPlaylist)
                : FindCollection(smartPlaylist);
        }

        public override UserFolder FindCollection(Domain.SmartPlaylist smartPlaylist)
        {
            return FindCollection(smartPlaylist, smartPlaylist.Name);
        }
        public override UserFolder FindCollection(Domain.SmartPlaylist smartPlaylist, string collectionName)
        {
            var user = _userManager.GetUserById(smartPlaylist.UserId);

            if (smartPlaylist.ForceCreate)
                return new UserFolder(user, smartPlaylist);

            Folder folder = null;
            if (smartPlaylist.InternalId > 0)
                folder = (Folder)_libraryManager.GetItemById(smartPlaylist.InternalId);

            if (folder == null)
                folder = FindCollectionFolder(smartPlaylist, collectionName);

            return folder != null
                ? new LibraryUserFolder<Folder>(user, folder, smartPlaylist)
                : new UserFolder(user, smartPlaylist);
        }

        public override Folder FindCollectionFolder(Domain.SmartPlaylist smartPlaylist, string collectionName)
        {
            var user = _userManager.GetUserById(smartPlaylist.UserId);

            return _libraryManager.GetItemsResult(new InternalItemsQuery
            {
                Name = collectionName,
                IncludeItemTypes = new[] { "collections", "Boxset" },
                User = user ////try using the user with policy.IsAdministrator for testing               
            }).Items.OfType<Folder>().FirstOrDefault();
        }

        public override UserFolder FindPlaylist(Domain.SmartPlaylist smartPlaylist)
        {
            return FindPlaylist(smartPlaylist, smartPlaylist.Name);
        }

        public override UserFolder FindPlaylist(Domain.SmartPlaylist smartPlaylist, string playlistName)
        {
            var user = _userManager.GetUserById(smartPlaylist.UserId);

            if (smartPlaylist.ForceCreate)
                return new UserFolder(user, smartPlaylist);

            Playlist folder = null;
            if (smartPlaylist.InternalId > 0)
                folder = (Playlist)_libraryManager.GetItemById(smartPlaylist.InternalId);

            if (folder == null)
                folder = FindPlaylistFolder(smartPlaylist, playlistName);

            return folder != null
                ? new LibraryUserFolder<Playlist>(user, folder, smartPlaylist)
                : new UserFolder(user, smartPlaylist);
        }

        public override Playlist FindPlaylistFolder(Domain.SmartPlaylist smartPlaylist, string playlistName)
        {
            var user = _userManager.GetUserById(smartPlaylist.UserId);

            return _libraryManager.GetItemsResult(new InternalItemsQuery(user)
            {
                IncludeItemTypes = new[] { typeof(Playlist).Name },
                Name = playlistName,
                Recursive = true
            }).Items.OfType<Playlist>().FirstOrDefault();
        }

        public override void Remove(Domain.SmartPlaylist smartPlaylist)
        {
            if (smartPlaylist.OriginalSmartType == SmartType.Collection)
                Remove<Folder>(smartPlaylist, _collectionItemUpdater, GetUser(smartPlaylist.UserId), () => { return FindCollectionFolder(smartPlaylist, smartPlaylist.Name); });
            else if (smartPlaylist.OriginalSmartType == SmartType.Playlist)
                Remove<Playlist>(smartPlaylist, _playListItemsUpdater, GetUser(smartPlaylist.UserId), () => { return FindPlaylistFolder(smartPlaylist, smartPlaylist.Name); });
        }

        private void Remove<T>(Domain.SmartPlaylist smartPlaylist, IFolderItemsUpdater folderItemsUpdater, User user, Func<Folder> action) where T : Folder
        {
            BaseItem baseFolder = smartPlaylist.InternalId > 0 ? _libraryManager.GetItemById(smartPlaylist.InternalId) : action();
            if (baseFolder != null)
            {
                if (baseFolder is T)
                {
                    LibraryUserFolder<T> folder = new LibraryUserFolder<T>(user, (T)baseFolder, smartPlaylist);
                    folderItemsUpdater.RemoveItems(folder, folder.GetItems(), new BaseItem[] { });
                }

                try
                {
                    _libraryManager.DeleteItem(baseFolder, new DeleteOptions()
                    {
                        DeleteFileLocation = true
                    });
                }
                catch (Exception)
                {
                    _libraryManager.DeleteItems(new long[] { baseFolder.InternalId });
                }//TODO: This needs to be fixed, as it work intermittently, but we don't want to crash out.
            }
        }

        public override BaseItem[] GetAllPlaylists()
        {
            return _libraryManager.GetItemsResult(new InternalItemsQuery
            {
                IncludeItemTypes = new[] { typeof(Playlist).Name },
                Recursive = true
            }).Items.ToArray();
        }

        public override BaseItem[] GetAllCollections()
        {
            return _libraryManager.GetItemsResult(new InternalItemsQuery
            {
                IncludeItemTypes = new[] { "collections", "Boxset" },
                Recursive = true
            }).Items.ToArray();
        }

        public override BaseItem[] GetItemsForFolderId(Guid folderId, User user)
        {
            HashSet<BaseItem> results = new HashSet<BaseItem>();

            var folder = _libraryManager.GetItemById(folderId);

            if (folder is Folder f)
            {
                f.GetChildren(new InternalItemsQuery(user)
                {
                    Recursive = false
                }).ForEach(b => RecurseToChildren(b, user, results));
            }

            return results.ToArray();
        }

        public override long[] GetItemsIdsForFolderId(Guid folderId, User user)
        {
            return (_libraryManager.GetItemById(folderId) as Folder).GetChildrenIds(new InternalItemsQuery(user)
            {
                Recursive = false
            });
        }

        private HashSet<BaseItem> RecurseToChildren(BaseItem item, User user, HashSet<BaseItem> current)
        {
            if (item.IsFolder)
                GetItemsForFolderId(item.Id, user).ForEach(b => current.Add(b));
            else
                current.Add(item);
            return current;
        }

        public override BaseItem[] GetItemsForFolderId(Domain.SmartPlaylist smartPlaylist, User user)
        {
            Folder folder = GetFolder(smartPlaylist);
            return folder == null ? new BaseItem[] { } : GetItemsForFolderId(folder.Id, user);
        }

        public override async Task<(UserFolder, BaseItem[])> GetBaseItemsForSmartPlayList(Domain.SmartPlaylist smartPlaylist, IUserItemsProvider userItemsProvider)
        {
            var baseFolder = GetUserPlaylistOrCollectionFolder(smartPlaylist);

            if (smartPlaylist.SourceType.Equals("Playlist", StringComparison.OrdinalIgnoreCase) || smartPlaylist.SourceType.Equals("Collection", StringComparison.OrdinalIgnoreCase))
            {
                smartPlaylist.Log($"Source is {smartPlaylist.Source.Name} [{smartPlaylist.SourceType}] ");
                return (baseFolder, GetItemsForFolderId(Guid.Parse(smartPlaylist.Source.Id), baseFolder.User));
            }
            else if (smartPlaylist.SourceType.Equals("Smart Playlist (Execute)", StringComparison.OrdinalIgnoreCase))
            {
                var linkedPlaylistDto = await _playlistStore.GetSmartPlaylistAsync(Guid.Parse(smartPlaylist.Source.Id));
                if (linkedPlaylistDto.InternalId == -1)
                    throw new Exception("Base linked playlist does not contain any source items.");

                Domain.SmartPlaylist linkedSmartPlaylist = new Domain.SmartPlaylist(linkedPlaylistDto);
                smartPlaylist.Log($"Source is {smartPlaylist.Source.Name} [{smartPlaylist.SourceType}] ");
                smartPlaylist.Log($"Executing base linked playlist: {smartPlaylist.Source.Name}");
                await Plugin.Instance.SmartPlaylistCommandHandler.HandleAsync(new UpdateSmartPlaylistCommand(Guid.Parse(smartPlaylist.Source.Id), ExecutionModes.LinkedAsSource));
                smartPlaylist.Log("Base linked playlist execution completed.");

                return (baseFolder, GetItemsForFolderId(linkedSmartPlaylist, baseFolder.User));
            }
            else
            {
                if ((smartPlaylist.UpdateType == UpdateType.Live || (smartPlaylist.IsShuffleUpdateType && smartPlaylist.MonitorMode)) && smartPlaylist.InternalId > 0 && baseFolder is LibraryUserFolder<Playlist>)
                {
                    smartPlaylist.Log($"Source is {smartPlaylist.Name} {(smartPlaylist.IsShuffleUpdateType ? "[Shuffle:Monitor]" : "[Live]")}");
                    return (baseFolder, GetItemsForFolderId(smartPlaylist, baseFolder.User));
                }
                else
                {
                    smartPlaylist.Log($"Source is [{string.Join(", ", Const.SupportedItemTypeNames)}]");
                    return (baseFolder, userItemsProvider?.GetItems(baseFolder.User, Const.SupportedItemTypeNames).ToArray());
                }
            }
        }

        public override Folder GetFolder(Domain.SmartPlaylist smartPlaylist)
        {
            return smartPlaylist.SmartType == SmartType.Playlist ?
                FindPlaylistFolder(smartPlaylist, smartPlaylist.Name) :
                FindCollectionFolder(smartPlaylist, smartPlaylist.Name);
        }
    }
}