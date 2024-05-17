using SmartPlaylist.Domain.Rule;
using SmartPlaylist.Domain.Values;
using System.Linq;

namespace SmartPlaylist.Domain.CriteriaDefinition.CriteriaDefinitions
{
    public class StreamAudioCodecCriteriaDefinition : CriteriaDefinition
    {
        public override string Name => "Stream: Audio Codec";
        public override CriteriaDefinitionType Type => new ListValueDefinitionType(Getter.AudioCodecs.First());
        public override Value[] Values => Getter.AudioCodecs;
        public override Value GetValue(UserItem item)
        {
            var audio = item.Item.GetMediaStreams().Where(x => x.Type == MediaBrowser.Model.Entities.MediaStreamType.Audio);
            if (audio.Any())
                return ArrayValue<ListValue>.Create(audio.Select(x => x.Codec).Distinct().Select(x => ListValue.Create(x)).ToArray());

            return Value.None;
        }
    }
}