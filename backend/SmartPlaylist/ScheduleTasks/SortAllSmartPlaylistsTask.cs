using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Model.Tasks;

namespace SmartPlaylist.ScheduleTasks
{
    public class SortAllSmartPlaylistsTask : IScheduledTask, IConfigurableScheduledTask
    {
        public SortAllSmartPlaylistsTask()
        {
        }

        public string Name => "Sort all Smart Playlists and Collections";

        public string Key => typeof(SortAllSmartPlaylistsTask).Name;

        public string Description => "Sort all Enabled Smart Playlists and Collections";

        public string Category => "Library";

        public bool IsHidden => false;

        public bool IsEnabled => true;

        public bool IsLogged => true;

        public Task Execute(CancellationToken cancellationToken, IProgress<double> progress)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TaskTriggerInfo> GetDefaultTriggers()
        {
            return Const.RefreshAllSmartPlaylistsTaskTriggers;
        }
    }
}