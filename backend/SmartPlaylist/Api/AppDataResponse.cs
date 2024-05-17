using SmartPlaylist.Contracts;
using SmartPlaylist.Domain;
using SmartPlaylist.Domain.CriteriaDefinition;

namespace SmartPlaylist.Api
{
    public class AppDataResponse
    {
        public SmartPlaylistDto[] Playlists { get; set; }

        public CriteriaDefinition[] RulesCriteriaDefinitions => DefinedCriteriaDefinitions.All;

        public string[] LimitOrdersBy => DefinedOrders.AllNames;

        public SourceDto[] Sources => SourceDto.All;

        public UserDto[] Users => UserDto.All;
    }
}