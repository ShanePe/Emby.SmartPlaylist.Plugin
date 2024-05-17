using System;
using System.Linq;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Entities.Audio;
using MediaBrowser.Controller.Entities.Movies;
using MediaBrowser.Controller.Entities.TV;
using MediaBrowser.Model.Tasks;
using SmartPlaylist.Domain;
using SmartPlaylist.Infrastructure.Queue;

namespace SmartPlaylist
{
    public static class Const
    {
        public static readonly MediaTypeDescriptor[] SupportedItemTypes =
        {
            MediaTypeDescriptor.CreateAudio<Audio>(),
            MediaTypeDescriptor.CreateVideo<Movie>(),
            MediaTypeDescriptor.CreateVideo<Episode>(),
            MediaTypeDescriptor.CreateVideo<MusicVideo>("Music Video"),
            MediaTypeDescriptor.CreateVideo<Video>("Home Video")
        };

        public static readonly string[] SupportedItemTypeNames = SupportedItemTypes.Select(x => x.MediaType.Name).ToArray();
        public static readonly string[] SupportedItemTypeDescriptions = SupportedItemTypes.Select(x => x.Description).ToArray();

        public static readonly Type[] ListenForChangeItemTypes =
            SupportedItemTypes.Select(x => x.MediaType).Concat(new[] { typeof(MusicAlbum), typeof(Season), typeof(Series) }).ToArray();

        public static readonly TimeSpan GetAllSmartPlaylistsCacheExpiration = TimeSpan.FromHours(2);
        public static readonly TimeSpan DefaultSemaphoreSlimTimeout = TimeSpan.FromSeconds(30);

        public static readonly AutoDequeueQueueConfig UpdatedItemsQueueConfig = new AutoDequeueQueueConfig
        {
#if DEBUG
            InactiveDequeueTime = TimeSpan.FromSeconds(2),
            AbsoluteDequeueTime = TimeSpan.FromSeconds(5),
            MaxItemsLimit = 5
#else
            InactiveDequeueTime = TimeSpan.FromMinutes(2),
            AbsoluteDequeueTime = TimeSpan.FromMinutes(5),
            MaxItemsLimit = 1000
#endif
        };


        public static readonly TaskTriggerInfo[] RefreshAllSmartPlaylistsTaskTriggers =
        {
            new TaskTriggerInfo
                {Type = TaskTriggerInfo.TriggerDaily, TimeOfDayTicks = TimeSpan.FromHours(1).Ticks}
        };


    }
}