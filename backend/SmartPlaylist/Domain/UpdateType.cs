using MediaBrowser.Controller.Entities.TV;

namespace SmartPlaylist.Domain
{
    public enum UpdateType
    {
        Live = 1,
        Manual = 2,
        ShuffleDaily = 3,
        ShuffleWeekly = 4,
        ShuffleMonthly = 5,
        Daily = 6,
        Weekly = 7,
        Monthly = 8
    }

    public enum SmartType
    {
        Playlist = 1,
        Collection = 2
    }

    public enum CollectionMode
    {
        [Epimode(typeof(Episode))]
        Item = 1,
        [Epimode(typeof(Season))]
        Season = 2,
        [Epimode(typeof(Series))]
        Series = 3
    }
}