using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SmartPlaylist.Contracts;
using SmartPlaylist.Infrastructure;

namespace SmartPlaylist.Services.SmartPlaylist
{
    public class CacheableSmartPlaylistStore : ISmartPlaylistStore
    {
        private readonly ISmartPlaylistStore _decorated;
        private readonly MemCache _memCache;


        public CacheableSmartPlaylistStore(ISmartPlaylistStore decorated)
        {
            _decorated = decorated;
            _memCache = new MemCache();
        }

        public async Task<SmartPlaylistDto> GetSmartPlaylistAsync(Guid smartPlaylistId)
        {
            return (await GetAllCachedSmartPlaylistAsync().ConfigureAwait(false)).Single(x =>
                x.Id == smartPlaylistId.ToString());
        }

        public async Task<SmartPlaylistDto[]> LoadPlaylistsAsync(Guid userId)
        {
            return (await GetAllCachedSmartPlaylistAsync().ConfigureAwait(false)).Where(x => x.UserId == userId)
            .OrderBy(x => x.Name).ToArray();
        }

        public async Task<SmartPlaylistDto[]> GetAllSmartPlaylistsAsync()
        {
            return await GetAllCachedSmartPlaylistAsync();
        }

        public void Save(SmartPlaylistDto smartPlaylist)
        {
            _decorated.Save(smartPlaylist);
            _memCache.Set(smartPlaylist.Id, smartPlaylist);
        }

        public void Delete(Guid userId, string smartPlaylistId)
        {
            _decorated.Delete(userId, smartPlaylistId);
            _memCache.Remove(smartPlaylistId);
        }

        private async Task<SmartPlaylistDto[]> GetAllCachedSmartPlaylistAsync()
        {
            return (await _memCache.GetOrCreateManyAsync(async () =>
                {
                    return (await _decorated.GetAllSmartPlaylistsAsync()
                            .ConfigureAwait(false))
                        .ToDictionary(x => (object)x.Id, y => (object)y);
                }, Const.GetAllSmartPlaylistsCacheExpiration).ConfigureAwait(false))
                .OfType<SmartPlaylistDto>()
                .ToArray();
        }

        public bool Exists(Guid userId, string smartPlaylistId)
        {
            return _decorated.Exists(userId, smartPlaylistId);
        }

        public async Task WriteToLogAsync(Domain.SmartPlaylist smartPlaylist)
        {
            await _decorated.WriteToLogAsync(smartPlaylist);
        }

        public Stream GetLogFileStream(Guid userId, string smartPlaylistId)
        {
            return _decorated.GetLogFileStream(userId, smartPlaylistId);
        }

        public string GetLogFilePath(Guid userId, string smartPlaylistId)
        {
            return _decorated.GetLogFilePath(userId, smartPlaylistId);
        }

        public async Task<string> ExportAsync(string[] smartPlaylistIds)
        {
            return await _decorated.ExportAsync(smartPlaylistIds).ConfigureAwait(false);
        }

        public void Delete(string path)
        {
            _decorated.Delete(path);
        }

        public async Task<string> ImportAsync(byte[] fileData, Guid userId)
        {
            _memCache.Invalidate();
            return await _decorated.ImportAsync(fileData, userId).ConfigureAwait(false);
        }
    }
}