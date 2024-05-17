using System;

namespace SmartPlaylist.Services.SmartPlaylist
{
    public interface ISmartPlaylistFileSystem
    {
        string BasePath { get; }
        string GetOrCreateSmartPlaylistDir(Guid userId);
        string GetSmartPlaylistFilePath(Guid smartPlaylistId);
        string[] GetSmartPlaylistFilePaths(Guid userId);
        string[] GetAllSmartPlaylistFilePaths();
        string GetSmartPlaylistPath(Guid userId, string playlistId);
        string GetSmartPlaylistLog(Guid userId, string playlistId);

        bool PlaylistFileExists(Guid userId, string playlistId);
    }
}