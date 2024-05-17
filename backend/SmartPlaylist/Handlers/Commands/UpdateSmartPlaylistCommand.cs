using System;
using SmartPlaylist.Infrastructure.MessageBus;

namespace SmartPlaylist.Handlers.Commands
{
    public enum ExecutionModes { Scheduled, Manual, OnSave, LinkedAsSource }
    public class UpdateSmartPlaylistCommand : IMessage
    {

        public UpdateSmartPlaylistCommand(Guid smartPlaylistId, ExecutionModes executionMode)
        {
            SmartPlaylistId = smartPlaylistId;
            ExecutionMode = executionMode;
        }

        public Guid SmartPlaylistId { get; }
        public ExecutionModes ExecutionMode { get; }
    }
}