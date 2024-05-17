using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Entities.TV;
using SmartPlaylist.Adapters;
using SmartPlaylist.Comparers;
using SmartPlaylist.Contracts;
using SmartPlaylist.Domain.Rule;
using SmartPlaylist.Extensions;
namespace SmartPlaylist.Domain
{
    public class SmartPlaylist
    {
        private readonly SmartPlaylistDto _dto;
        private long _internalid = 0;
        private string _name = string.Empty;
        internal StringCollection _logEntries = new StringCollection();
        public SmartPlaylist(SmartPlaylistDto dto)
        {
            _dto = dto;
            Id = Guid.Parse(dto.Id);
            Name = dto.Name;
            UserId = dto.UserId;
            Rules = new RuleBase[] { RuleAdapter.Adapt(dto.RulesTree) };
            Limit = new SmartPlaylistLimit(dto.Limit);
            NewItemOrder = new NewItemOrder(dto.NewItemOrder);
            LastShuffleUpdate = dto.LastShuffleUpdate;
            UpdateType = UpdateType.ConvertFromString<UpdateType>(dto.UpdateType, UpdateType.Live);
            SmartType = SmartType.ConvertFromString<SmartType>(dto.SmartType, SmartType.Playlist);
            InternalId = dto.InternalId;
            MediaType = MediaTypeGetter.Get(Rules);
            ForceCreate = dto.ForceCreate;
            OriginalSmartType = SmartType.ConvertFromString<SmartType>(dto.OriginalSmartType, SmartType.Playlist);
            CollectionMode = CollectionMode.ConvertFromString<CollectionMode>(dto.CollectionMode, CollectionMode.Item);
            LastUpdated = dto.LastUpdated;
            LastSync = dto.LastSync;
            SyncCount = dto.SyncCount;
            LastSyncDuration = dto.LastSyncDuration;
            Status = dto.Status;
            Enabled = dto.Enabled;
            SourceType = dto.SourceType;
            if (dto.Source != null)
                Source = new Source(dto.Source);
            SortJob = new SortJob(dto.SortJob);
            MonitorMode = dto.MonitorMode;
            UISections = new UISections(dto.UISections);
            Notes = dto.Notes;
        }

        public Guid Id { get; }
        public string Name { get { return _name; } set { _name = value; } }
        public Guid UserId { get; }
        public RuleBase[] Rules { get; }
        public SmartPlaylistLimit Limit { get; }
        public NewItemOrder NewItemOrder { get; }
        public UpdateType UpdateType { get; }
        public SmartType SmartType { get; }
        public SmartType OriginalSmartType { get; }
        public CollectionMode CollectionMode { get; }
        public bool Enabled { get; }
        public long InternalId
        {
            get { return _internalid; }
            set
            {
                _internalid = value;
                _dto.InternalId = _internalid;
            }
        }
        public bool ForceCreate { get; }
        public DateTimeOffset? LastShuffleUpdate { get; private set; }
        public DateTime? LastUpdated { get; set; }
        public DateTime? LastSync { get; set; }
        public int SyncCount { get; set; } = 0;
        public long LastSyncDuration { get; set; } = 0;
        public string Status { get; set; }
        public bool CanUpdatePlaylist => CheckIfCanUpdatePlaylist();

        public bool IsScheduledType => UpdateType == UpdateType.Daily ||
                                        UpdateType == UpdateType.Weekly ||
                                        UpdateType == UpdateType.Monthly;
        public bool IsShuffleUpdateType => UpdateType == UpdateType.ShuffleDaily ||
                                           UpdateType == UpdateType.ShuffleMonthly ||
                                           UpdateType == UpdateType.ShuffleWeekly;

        public bool IsRandomSort => Limit.OrderBy is OrderRandom;
        public string MediaType { get; }
        public String SourceType { get; }
        public Source Source { get; }
        public SortJob SortJob { get; }
        public bool MonitorMode { get; }
        public UISections UISections { get; }
        public string Notes { get; }
        internal StringCollection LogEntries { get => _logEntries; }

        private bool CheckIfCanUpdatePlaylist()
        {
            if (UpdateType == UpdateType.Manual) return false;

            if (IsShuffleUpdateType && MonitorMode) return true;

            if (LastShuffleUpdate.HasValue && (IsShuffleUpdateType || IsScheduledType)) return IsShuffleDue();

            return true;
        }

        public bool IsShuffleDue()
        {
            if (!(IsShuffleUpdateType || IsScheduledType)) return false;

            if (!LastShuffleUpdate.HasValue) return true;

            return DateTimeOffset.Now > LastShuffleUpdate.Value;
        }

        private BaseItem[] GetSourceItems(UserFolder userPlaylist)
        {
            if (SourceType.Equals("Media Items", StringComparison.OrdinalIgnoreCase))
            {
                return userPlaylist.GetItems();
            }
            return new BaseItem[] { };
        }

        public BaseItem[] FilterPlaylistItems(UserFolder userPlaylist, IEnumerable<BaseItem> items)
        {
            var playlistItems = this.GetSourceItems(userPlaylist);
            var newItems = FilterItems(playlistItems, items, userPlaylist.User).ToArray();
            Log($"Dealing with {newItems.Length} after filter.");
            if (newItems.Length == 0)
                return newItems;

            Log("Removing missing episodes if any.");
            newItems = RemoveMissingEpisodes(newItems);

            if (SmartType == SmartType.Collection && CollectionMode != CollectionMode.Item)
            {
                Log($"Rolling up collection to {CollectionMode}");
                newItems = RollUpTo(newItems);
            }

            if (IsShuffleUpdateType && !NewItemOrder.HasSort && !Limit.HasLimit)
            {
                Log("Type is shuffles so shuffling items.");
                newItems = newItems.Shuffle();
            }

            if (SmartType == SmartType.Playlist && NewItemOrder.HasSort)
            {
                Log($"Sorting items by: {NewItemOrder.OrderBy.Name}");
                newItems = OrderNewItems(newItems);
            }
            else if (Limit.HasLimit)
            {
                Log($"Limiting items to {Limit.MaxItems} based on {Limit.OrderBy.Name}:{Limit.OrderBy.Direction}:Shuffled - {Limit.OrderBy.IsShuffle}.");
                newItems = OrderLimitItems(newItems).Take(Limit.MaxItems).ToArray();
            }
            return newItems;
        }

        private static BaseItem[] RemoveMissingEpisodes(BaseItem[] items)
        {
            return items.Where(x => !(x is Episode episode && episode.IsMissingEpisode)).ToArray();
        }

        private BaseItem[] RollUpTo(BaseItem[] items)
        {
            EpimodeAttribute rollTo = CollectionMode.GetAttributeOfType<EpimodeAttribute>();
            return items.Select(x => (x is Episode) ? RollUpToItemType(x, rollTo.MediaType) : x).Distinct().ToArray();
        }

        private BaseItem RollUpToItemType(BaseItem item, Type rollType)
        {
            return item.GetType().IsAssignableFrom(rollType) ? item : RollUpToItemType(item.Parent, rollType);
        }

        private BaseItem[] OrderLimitItems(BaseItem[] playlistItems)
        {
            return Sorter.Sort(playlistItems, this, Limit.OrderBy);
        }

        private BaseItem[] OrderNewItems(BaseItem[] playlistItems)
        {
            return Sorter.Sort(playlistItems, this, NewItemOrder.OrderBy);
        }

        private IEnumerable<BaseItem> FilterItems(IEnumerable<BaseItem> playlistItems, IEnumerable<BaseItem> newItems,
            User user)
        {
            return playlistItems.Union(newItems, new BaseItemEqualByInternalId())
                .Where(x => IsMatchRules(x, user));
        }

        public HierarchyStringDto ExplainRules()
        {
            HierarchyStringDto where = new HierarchyStringDto("WHERE", 0);
            Rules.ForEach(x => x.Explain(where, 1, UserDto.All));
            where.CompressContainers();
            return where;
        }

        private bool IsMatchRules(BaseItem item, User user)
        {
            bool result = Rules.All(x => x.IsMatch(new UserItem(user, item, this)));

            Log(result ?
                $"'{item.Name}' added to playlist/collection!!!!" :
                $"'{item.Name}' failed to match on criteria.");

            return result;
        }

        public void UpdateLastShuffleTime()
        {
            var now = DateTime.Now.Date;

            switch (UpdateType)
            {
                case UpdateType.ShuffleDaily:
                case UpdateType.Daily:
                    LastShuffleUpdate = now.AddDays(1);
                    break;
                case UpdateType.ShuffleWeekly:
                case UpdateType.Weekly:
                    LastShuffleUpdate = now.AddDays(7);
                    break;
                case UpdateType.ShuffleMonthly:
                case UpdateType.Monthly:
                    LastShuffleUpdate = now.AddMonths(1);
                    break;
            }

            Log($"Updating Shuffle time to: {LastShuffleUpdate}");
        }

        public void Log(string message)
        {
            _logEntries.Add($"[{DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")}]: {message}");
        }

        public SmartPlaylistDto ToDto()
        {
            return new SmartPlaylistDto
            {
                Id = Id.ToString(),
                LastShuffleUpdate = LastShuffleUpdate,
                Limit = Limit.ToDto(),
                NewItemOrder = NewItemOrder.ToDto(),
                Name = Name,
                RulesTree = _dto.RulesTree,
                UpdateType = _dto.UpdateType,
                SmartType = _dto.SmartType,
                UserId = UserId,
                OriginalSmartType = _dto.OriginalSmartType,
                InternalId = InternalId,
                CollectionMode = _dto.CollectionMode,
                LastUpdated = LastUpdated,
                LastSync = LastSync,
                SyncCount = SyncCount,
                LastSyncDuration = LastSyncDuration,
                Status = Status,
                Enabled = Enabled,
                SourceType = SourceType,
                Source = _dto.Source,
                SortJob = SortJob.ToDto(),
                MonitorMode = _dto.MonitorMode,
                UISections = _dto.UISections,
                Notes = _dto.Notes
            };
        }
    }
}