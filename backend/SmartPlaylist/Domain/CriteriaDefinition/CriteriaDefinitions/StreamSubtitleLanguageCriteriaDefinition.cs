using SmartPlaylist.Domain.Rule;
using SmartPlaylist.Domain.Values;
using System.Linq;

namespace SmartPlaylist.Domain.CriteriaDefinition.CriteriaDefinitions
{
    public class StreamSubtitleLanguageCriteriaDefinition : CriteriaDefinition
    {
        public override string Name => "Stream: Subtitle Language";
        public override CriteriaDefinitionType Type => new ListValueDefinitionType(Getter.SubtitleLanguages.First());
        public override Value[] Values => Getter.SubtitleLanguages;
        public override Value GetValue(UserItem item)
        {
            var subtitle = item.Item.GetMediaStreams().Where(x => x.Type == MediaBrowser.Model.Entities.MediaStreamType.Subtitle);
            if (subtitle.Any())
                return ArrayValue<ListValue>.Create(subtitle.Select(x => x.Language).Distinct().Select(x => ListValue.Create(x)).ToArray());

            return Value.None;
        }
    }
}