using System.Linq;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Playlists;

namespace SmartPlaylist.Domain
{
    public class LibraryUserFolder<T> : UserFolder where T : Folder
    {
        private readonly T _item;
        public T Item => _item;
        public LibraryUserFolder(User user, T folder, SmartPlaylist smartPlaylist) : base(user, smartPlaylist)
        {
            _item = folder;
        }

        public long InternalId => _item.InternalId;

        public override BaseItem[] GetItems()
        {
            return _item.GetChildren(User).ToArray();
        }

        public void DynamicUpdate()
        {
            bool update = false;
            if (!Item.Name.Equals(SmartPlaylist.Name))
            {
                if (Item.LockedFields.Any(x => x == MediaBrowser.Model.Entities.MetadataFields.Name))
                    SmartPlaylist.Name = Item.Name;
                else
                {
                    Item.Name = SmartPlaylist.Name;
                    update = true;
                }

            }
            if (update)
                Item.UpdateToRepository(MediaBrowser.Controller.Library.ItemUpdateType.MetadataEdit);
        }
    }
}