using MediaBrowser.Controller.Entities;

namespace SmartPlaylist.Domain.Rule
{
    public class UserItem
    {
        public UserItem(User user, BaseItem item, SmartPlaylist smartPlaylist)
        {
            User = user;
            Item = item;
            SmartPlaylist = smartPlaylist;
        }

        public User User { get; }
        public BaseItem Item { get; }
        public SmartPlaylist SmartPlaylist { get; }
        public bool TryGetUserItemData(out UserItemData userItemData)
        {
            return TryGetUserItemData(out userItemData, User, Item);
        }

        public static bool TryGetUserItemData(out UserItemData userItemData, User user, BaseItem item)
        {
            userItemData = BaseItem.UserDataManager.GetUserData(user, item);
            return userItemData != null;
        }
    }
}