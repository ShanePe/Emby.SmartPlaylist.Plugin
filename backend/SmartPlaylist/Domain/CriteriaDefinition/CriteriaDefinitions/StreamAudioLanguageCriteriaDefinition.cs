using SmartPlaylist.Domain.Rule;
using SmartPlaylist.Domain.Values;
using System.Linq;

namespace SmartPlaylist.Domain.CriteriaDefinition.CriteriaDefinitions
{
    public class StreamAudioLanguageCriteriaDefinition : CriteriaDefinition
    {
        public override string Name => "Stream: Audio Language";
        public override CriteriaDefinitionType Type => new ListValueDefinitionType(Getter.AudioLanguages.First());
        public override Value[] Values => Getter.AudioLanguages;
        public override Value GetValue(UserItem item)
        {
            var audio = item.Item.GetMediaStreams().Where(x => x.Type == MediaBrowser.Model.Entities.MediaStreamType.Audio);
            if (audio.Any())
                return ArrayValue<ListValue>.Create(audio.Select(x => x.Language).Distinct().Select(x => ListValue.Create(x)).ToArray());

            return Value.None;
        }
    }
}