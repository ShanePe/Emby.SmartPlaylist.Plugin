using System;
using System.IO;
using System.Linq;
using MediaBrowser.Controller;

namespace SmartPlaylist.Services.SmartPlaylist
{
    public class SmartPlaylistFileSystem : ISmartPlaylistFileSystem
    {
        public SmartPlaylistFileSystem(IServerApplicationPaths serverApplicationPaths)
        {
            BasePath = Path.Combine(serverApplicationPaths.DataPath, "smartplaylists");
        }

        public string BasePath { get; }

        public string GetOrCreateSmartPlaylistDir(Guid userId)
        {
            var path = Path.Combine(BasePath, userId.ToString());

            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            return path;
        }

        public string GetSmartPlaylistFilePath(Guid smartPlaylistId)
        {
            return Directory.GetFiles(BasePath, $"{smartPlaylistId}.json", SearchOption.AllDirectories).FirstOrDefault();
        }

        public string[] GetSmartPlaylistFilePaths(Guid userId)
        {
            var smartPlaylistDir = GetOrCreateSmartPlaylistDir(userId);
            return Directory.GetFiles(smartPlaylistDir);
        }

        public string[] GetAllSmartPlaylistFilePaths()
        {
            return Directory.GetFiles(BasePath, "*.json", SearchOption.AllDirectories);
        }

        public string GetSmartPlaylistPath(Guid userId, string playlistId)
        {
            var playlistsPath = GetOrCreateSmartPlaylistDir(userId);
            return Path.Combine(playlistsPath, $"{playlistId}.json");
        }

        public string GetSmartPlaylistLog(Guid userId, string playlistId)
        {
            var playlistsPath = GetOrCreateSmartPlaylistDir(userId);
            return Path.Combine(playlistsPath, $"{playlistId}.log");
        }

        public bool PlaylistFileExists(Guid userId, string playlistId)
        {
            return File.Exists(GetSmartPlaylistPath(userId, playlistId));
        }
    }
}