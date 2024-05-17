using System;
using System.IO;
using System.Threading.Tasks;
using SmartPlaylist.Contracts;
using dom = SmartPlaylist.Domain;
namespace SmartPlaylist.Services.SmartPlaylist
{
    public interface ISmartPlaylistStore
    {
        Task<SmartPlaylistDto> GetSmartPlaylistAsync(Guid smartPlaylistId);
        Task<SmartPlaylistDto[]> LoadPlaylistsAsync(Guid userId);
        Task<SmartPlaylistDto[]> GetAllSmartPlaylistsAsync();
        void Save(SmartPlaylistDto smartPlaylist);
        Task WriteToLogAsync(dom.SmartPlaylist smartPlaylist);

        Stream GetLogFileStream(Guid userId, string smartPlaylistId);
        string GetLogFilePath(Guid userId, string smartPlaylistId);

        void Delete(Guid userId, string smartPlaylistId);
        void Delete(string path);

        bool Exists(Guid userId, string smartPlaylistId);

        Task<string> ExportAsync(string[] smartPlaylistIds);
        Task<string> ImportAsync(byte[] fileData, Guid userId);
    }
}