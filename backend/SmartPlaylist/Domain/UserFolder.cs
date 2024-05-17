using MediaBrowser.Controller.Entities;

namespace SmartPlaylist.Domain
{
    public class UserFolder
    {
        public UserFolder(User user, SmartPlaylist smartPlaylist)
        {
            User = user;
            SmartPlaylist = smartPlaylist;
        }

        public User User { get; }
        public SmartPlaylist SmartPlaylist { get; }


        public virtual BaseItem[] GetItems()
        {
            return new BaseItem[0];
        }
    }
}