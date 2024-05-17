using System.Diagnostics;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Entities.TV;
using SmartPlaylist.Domain.Rule;
using SmartPlaylist.Domain.Values;
using System.Linq;

namespace SmartPlaylist.Domain.CriteriaDefinition.CriteriaDefinitions
{
    public class CountryCriteriaDefinition : CriteriaDefinition
    {
        public override string Name => "Country";

        public override CriteriaDefinitionType Type => StringDefinitionType.Instance;

        public override Value GetValue(UserItem item)
        {
            if (item.Item is Video video)
                return ArrayValue<StringValue>.Create(video.ProductionLocations.Select(StringValue.Create).ToArray());

            return Value.None;
        }
    }
}