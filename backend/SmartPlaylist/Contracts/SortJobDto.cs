using System;

namespace SmartPlaylist.Contracts
{
    public class SortJobDto
    {
        public static SortJobDto Default => new SortJobDto()
        {
            Enabled = false,
            OrderBy = "Name",
            UpdateType = "Daily"
        };
        public bool Enabled { get; set; }
        public string UpdateType { get; set; }
        public string OrderBy { get; set; }
        public int SyncCount { get; set; }
        public long LastSyncDuration { get; set; } = 0;
        public string Status { get; set; }
        public DateTimeOffset? NextUpdate { get; set; } = null;
        public DateTime? LastUpdated { get; set; } = null;
        public DateTime? LastRan { get; set; } = null;
        public string[] ThenBys { get; set; }

        public string LastDurationStr => String.Format("{0:%h} hours {0:%m} minutes and {0:%s}.{0:%f} seconds", TimeSpan.FromMilliseconds(LastSyncDuration));

    }
}