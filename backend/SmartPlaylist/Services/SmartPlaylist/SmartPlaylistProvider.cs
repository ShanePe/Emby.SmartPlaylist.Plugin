using System;
using System.Linq;
using System.Threading.Tasks;
using SmartPlaylist.Adapters;

namespace SmartPlaylist.Services.SmartPlaylist
{
    public interface ISmartPlaylistProvider
    {
        Task<Domain.SmartPlaylist[]> GetAllUpdateableSmartPlaylistsAsync();
        Task<Domain.SmartPlaylist> GetSmartPlaylistAsync(Guid smartPlaylistId);

        Task<Domain.SmartPlaylist[]> GetAllSortableSmartPlaylistsAsync();
    }

    public class SmartPlaylistProvider : ISmartPlaylistProvider
    {
        private readonly ISmartPlaylistStore _smartPlaylistStore;

        public SmartPlaylistProvider(ISmartPlaylistStore smartPlaylistStore)
        {
            _smartPlaylistStore = smartPlaylistStore;
        }

        public async Task<Domain.SmartPlaylist[]> GetAllSortableSmartPlaylistsAsync()
        {
            return SmartPlaylistAdapter.Adapt(await _smartPlaylistStore.GetAllSmartPlaylistsAsync().ConfigureAwait(false))
                                            .Where(x => x.SmartType == Domain.SmartType.Playlist && x.Enabled && x.SortJob.AvailableToSort()).ToArray();
        }

        public async Task<Domain.SmartPlaylist[]> GetAllUpdateableSmartPlaylistsAsync()
        {
            var smartPlaylistDtos = await _smartPlaylistStore.GetAllSmartPlaylistsAsync().ConfigureAwait(false);
            return SmartPlaylistAdapter.Adapt(smartPlaylistDtos).Where(x => x.Enabled && x.CanUpdatePlaylist).ToArray();
        }


        public async Task<Domain.SmartPlaylist> GetSmartPlaylistAsync(Guid smartPlaylistId)
        {
            var smartPlaylistDto =
                await _smartPlaylistStore.GetSmartPlaylistAsync(smartPlaylistId).ConfigureAwait(false);
            return SmartPlaylistAdapter.Adapt(smartPlaylistDto);
        }
    }
}