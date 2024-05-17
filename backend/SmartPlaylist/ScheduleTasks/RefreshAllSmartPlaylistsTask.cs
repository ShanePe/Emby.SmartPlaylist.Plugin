using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Model.Tasks;
using SmartPlaylist.Handlers.Commands;

namespace SmartPlaylist.ScheduleTasks
{
    public class RefreshAllSmartPlaylistsTask : IScheduledTask, IConfigurableScheduledTask
    {
        public bool IsHidden => false;

        public bool IsEnabled => true;

        public bool IsLogged => true;

        public IEnumerable<TaskTriggerInfo> GetDefaultTriggers()
        {
            return Const.RefreshAllSmartPlaylistsTaskTriggers;
        }

        public Task Execute(CancellationToken cancellationToken, IProgress<double> progress)
        {
            Plugin.Instance.MessageBus.Publish(new UpdateAllSmartPlaylistsCommand());

            return Task.CompletedTask;
        }

        public string Key => typeof(RefreshAllSmartPlaylistsTask).Name;

        public string Name => "Refresh all Smart Playlists and Collections";

        public string Description => "Refresh all Enabled Smart Playlists and Collections";

        public string Category => "Library";
    }
}