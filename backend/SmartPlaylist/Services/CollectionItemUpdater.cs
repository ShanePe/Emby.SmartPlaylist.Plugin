using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Controller.Collections;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Library;
using MediaBrowser.Model.Configuration;
using MediaBrowser.Model.Dto;
using MediaBrowser.Model.Entities;
using SmartPlaylist.Domain;
using SmartPlaylist.Extensions;

namespace SmartPlaylist.Services
{
    public class CollectionItemUpdater : IFolderItemsUpdater
    {
        private readonly ILibraryManager _libraryManager;
        private readonly ICollectionManager _collectionManager;
        public CollectionItemUpdater(ILibraryManager libraryManager, ICollectionManager collectionManager)
        {
            _libraryManager = libraryManager;
            _collectionManager = collectionManager;
        }

        public (long internalId, string message) UpdateAsync(UserFolder folder, BaseItem[] newItems)
        {
            (long internalId, string message) ret = (0, string.Empty);
            if (folder is LibraryUserFolder<Folder> libraryUserCollection)
            {
                var currentItems = libraryUserCollection.GetItems();
                int removed = RemoveItems(libraryUserCollection, currentItems,
                    (folder.SmartPlaylist.IsShuffleUpdateType && folder.SmartPlaylist.IsShuffleDue()) || folder.SmartPlaylist.Limit.HasLimit ? new BaseItem[] { } : newItems);
                int added = AddItemsToCollection(libraryUserCollection,
                    (folder.SmartPlaylist.IsShuffleUpdateType && folder.SmartPlaylist.IsShuffleDue()) || folder.SmartPlaylist.Limit.HasLimit ? new BaseItem[] { } : currentItems, newItems);
                libraryUserCollection.DynamicUpdate();
                ret = (libraryUserCollection.InternalId, $"Completed - (Removed: {removed} Added: {added} items to the existing collection)");
            }
            else if (newItems.Any())
            {
                BoxSet result = CreateCollection(new CollectionCreationOptions()
                {
                    ItemIdList = newItems.Select(x => x.InternalId).ToArray<long>(),
                    Name = folder.SmartPlaylist.Name
                });

                ret = (result.InternalId, $"Completed - (Added {newItems.Count()} to new collection)");

            }
            else
                ret = (-1, "Completed - (Collection not created, no items found to add)");

            return ret;
        }

        public int RemoveItems(UserFolder folder, BaseItem[] currentItems, BaseItem[] newItems)
        {
            List<BaseItem> toRemove = new List<BaseItem>(currentItems.Except(newItems, (c, n) => c.InternalId == n.InternalId));

            if (toRemove.Any() && folder is LibraryUserFolder<Folder> collectionFolder)
            {
                _collectionManager.RemoveFromCollection((BoxSet)collectionFolder.Item, toRemove.Select(i => i.InternalId).ToArray());
            }

            return toRemove.Count;
        }

        private int AddItemsToCollection(LibraryUserFolder<Folder> collection, BaseItem[] currentItems, BaseItem[] newItems)
        {
            List<BaseItem> toAdd = new List<BaseItem>(newItems.Except(currentItems, (c, n) => c.InternalId == n.InternalId));
            if (toAdd.Any() && collection is LibraryUserFolder<Folder> collectionFolder)
            {
                _collectionManager.AddToCollection(collectionFolder.InternalId, toAdd.Select(i => i.InternalId).ToArray());
            }
            return toAdd.Count;
        }

        private List<Folder> FindFolders() => this._libraryManager.GetUserRootFolder().GetChildren(new InternalItemsQuery()
        {
            IsFolder = new bool?(true)
        }).OfType<CollectionFolder>().ToList<CollectionFolder>()
            .Where<CollectionFolder>(
                (Func<CollectionFolder, bool>)
                (
                    i => MemoryExtensions.Equals(i.CollectionType.AsSpan(), CollectionType.BoxSets.Span, StringComparison.OrdinalIgnoreCase))
                ).OfType<Folder>().ToList<Folder>();


        public BoxSet CreateCollection(CollectionCreationOptions options)
        {

            Folder folder = EnsureLibraryFolder(true);
            BoxSet newEntry = null;

            foreach (long itemId in options.ItemIdList)
            {
                BaseItem itemById = _libraryManager.GetItemById(itemId);
                if (itemById != null)
                {
                    if (newEntry == null)
                        newEntry = AddCollection(itemById, options.Name);
                    else if (itemById.AddCollection(newEntry))
                        itemById.UpdateToRepository(ItemUpdateType.None);
                }
            }
            return newEntry;
        }

        internal Folder EnsureLibraryFolder(bool createIfNeeded)
        {
            List<Folder> folders = this.FindFolders();
            if (folders.Count > 0)
                return folders[0];
            if (!createIfNeeded)
                return (Folder)null;
            LibraryOptions options = new LibraryOptions()
            {
                EnableRealtimeMonitor = false,
                SaveLocalMetadata = true,
                ContentType = CollectionType.BoxSets.ToString()
            };

            this._libraryManager.AddVirtualFolder("Collections", options, true);
            return this.FindFolders().FirstOrDefault<Folder>();
        }

        private BoxSet AddCollection(BaseItem item, string collectionName)
        {
            LinkedItemInfo linkedItemInfo = item.AddCollection(collectionName);
            if (!linkedItemInfo.Id.Equals(0L))
                return (BoxSet)this._libraryManager.GetItemById(linkedItemInfo.Id);
            item.UpdateToRepository(ItemUpdateType.MetadataEdit);
            return (BoxSet)this._libraryManager.GetItemById(linkedItemInfo.Id);
        }

        public int ClearPlaylist(UserFolder folder)
        {
            if (folder is LibraryUserFolder<Folder> collectionFolder)
            {
                BaseItem[] items = collectionFolder.GetItems();
                _collectionManager.RemoveFromCollection((BoxSet)collectionFolder.Item, items.Select(i => i.InternalId).ToArray());

                return items.Length;
            }
            return 0;
        }
    }
}