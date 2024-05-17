using System;
using System.Threading.Tasks;
using SmartPlaylist.Infrastructure;
using SmartPlaylist.Services.SmartPlaylist;

namespace SmartPlaylist.PerfLoggerDecorators.Services
{
    public class SmartPlaylistProviderPerfLoggerDecorator : ISmartPlaylistProvider
    {
        private readonly ISmartPlaylistProvider _decorated;

        public SmartPlaylistProviderPerfLoggerDecorator(ISmartPlaylistProvider decorated)
        {
            _decorated = decorated;
        }

        public async Task<Domain.SmartPlaylist[]> GetAllSortableSmartPlaylistsAsync()
        {
            using (PerfLogger.Create("GetAllSortableSmartPlaylistsAsync"))
            {
                return await _decorated.GetAllSortableSmartPlaylistsAsync().ConfigureAwait(false);
            }
        }

        public async Task<Domain.SmartPlaylist[]> GetAllUpdateableSmartPlaylistsAsync()
        {
            using (PerfLogger.Create("GetAllUpdateableSmartPlaylists"))
            {
                return await _decorated.GetAllUpdateableSmartPlaylistsAsync().ConfigureAwait(false);
            }
        }

        public async Task<Domain.SmartPlaylist> GetSmartPlaylistAsync(Guid smartPlaylistId)
        {
            Domain.SmartPlaylist smartPlaylist = null;
            using (PerfLogger.Create("GetSmartPlaylistFromProvider", () => new { smartPlaylistName = smartPlaylist?.Name }))
            {
                smartPlaylist = await _decorated.GetSmartPlaylistAsync(smartPlaylistId).ConfigureAwait(false);
                return smartPlaylist;
            }
        }
    }
}