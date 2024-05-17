using System;

namespace SmartPlaylist.Contracts
{
    [Serializable]
    public class SmartPlaylistDto
    {
        public SmartPlaylistDto()
        {
            Enabled = true;
            NewItemOrder = SmartPlaylistNewItemOrderDto.Default;
            SourceType = "Media Items";
            SortJob = SortJobDto.Default;
            MonitorMode = false;
            UISections = UISectionsDto.Default;
            Notes = string.Empty;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public RuleTreeNodeDto[] RulesTree { get; set; }
        public Guid UserId { get; set; }
        public SmartPlaylistLimitDto Limit { get; set; }
        public SmartPlaylistNewItemOrderDto NewItemOrder { get; set; }
        public DateTimeOffset? LastShuffleUpdate { get; set; } = null;
        public DateTime? LastUpdated { get; set; } = null;
        public DateTime? LastSync { get; set; } = null;
        public string UpdateType { get; set; }
        public string SmartType { get; set; }
        public long InternalId { get; set; }
        public string OriginalSmartType { get; set; }
        public bool ForceCreate { get; set; }
        public string CollectionMode { get; set; }
        public int SyncCount { get; set; }
        public long LastSyncDuration { get; set; } = 0;
        public string Status { get; set; }
        public bool Enabled { get; set; }
        public string SourceType { get; set; }
        public SourceDto Source { get; set; }
        public SortJobDto SortJob { get; set; }
        public bool MonitorMode { get; set; }
        public UISectionsDto UISections { get; set; }
        public string Notes { get; set; }

    }
}