using System;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Playlists;
using System.Linq;

namespace SmartPlaylist.Contracts
{
    [Serializable]
    public class SourceDto
    {
        public string Type { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }

        public static SourceDto[] All
        {
            get
            {
                return Plugin.Instance.FolderRepository.GetAllPlaylists().Select(x => new SourceDto()
                {
                    Type = "Playlist",
                    Id = x.Id.ToString(),
                    Name = x.Name
                }).Concat(Plugin.Instance.FolderRepository.GetAllCollections().Select(x => new SourceDto()
                {
                    Type = "Collection",
                    Id = x.Id.ToString(),
                    Name = x.Name
                })).OrderBy(x => x.Name).ToArray();
            }
        }
    }
}
