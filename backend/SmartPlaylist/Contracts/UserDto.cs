using System;
using System.Linq;

namespace SmartPlaylist.Contracts
{
    [Serializable]
    public class UserDto
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public static UserDto[] All
        {
            get
            {
                return Plugin.Instance.UserManager.GetUsers(new MediaBrowser.Model.Querying.UserQuery()).Items
                    .Select(x => new UserDto()
                    {
                        Id = x.Id.ToString(),
                        Name = x.Name
                    }).ToArray();
            }
        }
    }
}