using System;
using System.IO;
using System.Threading.Tasks;
using SmartPlaylist.Contracts;
using SmartPlaylist.Infrastructure;
using SmartPlaylist.Services.SmartPlaylist;

namespace SmartPlaylist.PerfLoggerDecorators.Services
{
    public class SmartPlaylistStorePerfLoggerDecorator : ISmartPlaylistStore
    {
        private readonly ISmartPlaylistStore _decorated;

        public SmartPlaylistStorePerfLoggerDecorator(ISmartPlaylistStore decorated)
        {
            _decorated = decorated;
        }

        public async Task<SmartPlaylistDto> GetSmartPlaylistAsync(Guid smartPlaylistId)
        {
            SmartPlaylistDto smartPlaylistDto = null;
            using (PerfLogger.Create("GetSmartPlaylistFromStore", () => new { smartPlaylistName = smartPlaylistDto?.Name }))
            {
                smartPlaylistDto = await _decorated.GetSmartPlaylistAsync(smartPlaylistId).ConfigureAwait(false);
                return smartPlaylistDto;
            }
        }

        public async Task<SmartPlaylistDto[]> LoadPlaylistsAsync(Guid userId)
        {
            using (PerfLogger.Create("LoadPlaylistsFromStore"))
            {
                return await _decorated.LoadPlaylistsAsync(userId).ConfigureAwait(false);
            }
        }

        public async Task<SmartPlaylistDto[]> GetAllSmartPlaylistsAsync()
        {
            using (PerfLogger.Create("GetAllSmartPlaylistsFromStore"))
            {
                return await _decorated.GetAllSmartPlaylistsAsync().ConfigureAwait(false);
            }
        }

        public void Save(SmartPlaylistDto smartPlaylist)
        {
            using (PerfLogger.Create("SaveSmartPlaylist", () => new { smartPlaylistName = smartPlaylist.Name }))
            {
                _decorated.Save(smartPlaylist);
            }
        }

        public void Delete(Guid userId, string smartPlaylistId)
        {
            using (PerfLogger.Create("DeleteSmartPlaylist"))
            {
                _decorated.Delete(userId, smartPlaylistId);
            }
        }

        public bool Exists(Guid userId, string smartPlaylistId)
        {
            return _decorated.Exists(userId, smartPlaylistId);
        }

        public async Task WriteToLogAsync(Domain.SmartPlaylist smartPlaylist)
        {
            using (PerfLogger.Create($"WriteLogToFS:{smartPlaylist.Id} [{smartPlaylist.Name}]"))
            {
                await _decorated.WriteToLogAsync(smartPlaylist);
            }
        }

        public Stream GetLogFileStream(Guid userId, string smartPlaylistId)
        {
            using (PerfLogger.Create("ReadLogFromFS"))
            {
                return _decorated.GetLogFileStream(userId, smartPlaylistId);
            }
        }

        public string GetLogFilePath(Guid userId, string smartPlaylistId)
        {
            using (PerfLogger.Create("GetLogFilePath"))
            {
                return _decorated.GetLogFilePath(userId, smartPlaylistId);
            }
        }

        public async Task<string> ExportAsync(string[] ids)
        {
            string result;
            using (PerfLogger.Create("Exporting"))
            {
                result = await _decorated.ExportAsync(ids).ConfigureAwait(false);
                return result;
            }
        }

        public void Delete(string path)
        {
            using (PerfLogger.Create($"Deleting file: {path}"))
            {
                _decorated.Delete(path);
            }
        }

        public async Task<string> ImportAsync(byte[] fileData, Guid userId)
        {
            string result;
            using (PerfLogger.Create($"Importing"))
            {
                result = await _decorated.ImportAsync(fileData, userId).ConfigureAwait(false);
                return result;
            }
        }
    }
}