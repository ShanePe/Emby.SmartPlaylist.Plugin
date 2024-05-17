using System;
using System.IO;

namespace SmartPlaylist.Services.SmartPlaylist
{
    public class EnsureBaseDirSmartPlaylistFileSystemDecorator : ISmartPlaylistFileSystem
    {
        private readonly ISmartPlaylistFileSystem _decorated;

        public EnsureBaseDirSmartPlaylistFileSystemDecorator(ISmartPlaylistFileSystem decorated)
        {
            _decorated = decorated;
        }

        public string BasePath => _decorated.BasePath;

        public string GetOrCreateSmartPlaylistDir(Guid userId)
        {
            CreateBasePathIfNotExits();

            return _decorated.GetOrCreateSmartPlaylistDir(userId);
        }

        public string GetSmartPlaylistFilePath(Guid smartPlaylistId)
        {
            CreateBasePathIfNotExits();

            return _decorated.GetSmartPlaylistFilePath(smartPlaylistId);
        }

        public string[] GetSmartPlaylistFilePaths(Guid userId)
        {
            CreateBasePathIfNotExits();


            return _decorated.GetSmartPlaylistFilePaths(userId);
        }

        public string[] GetAllSmartPlaylistFilePaths()
        {
            CreateBasePathIfNotExits();
            return _decorated.GetAllSmartPlaylistFilePaths();
        }

        public string GetSmartPlaylistPath(Guid userId, string playlistId)
        {
            CreateBasePathIfNotExits();

            return _decorated.GetSmartPlaylistPath(userId, playlistId);
        }

        private void CreateBasePathIfNotExits()
        {
            if (!Directory.Exists(BasePath)) Directory.CreateDirectory(BasePath);
        }

        public bool PlaylistFileExists(Guid userId, string playlistId)
        {
            return _decorated.PlaylistFileExists(userId, playlistId);
        }

        public string GetSmartPlaylistLog(Guid userId, string playlistId)
        {
            return _decorated.GetSmartPlaylistLog(userId, playlistId);
        }
    }
}