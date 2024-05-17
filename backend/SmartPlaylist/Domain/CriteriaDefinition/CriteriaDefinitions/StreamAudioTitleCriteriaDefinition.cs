using SmartPlaylist.Domain.Rule;
using SmartPlaylist.Domain.Values;
using System.Linq;

namespace SmartPlaylist.Domain.CriteriaDefinition.CriteriaDefinitions
{
    public class StreamAudioTitleCriteriaDefinition : CriteriaDefinition
    {
        public override string Name => "Stream: Audio Display Title";

        public override CriteriaDefinitionType Type => StringDefinitionType.Instance;

        public override Value GetValue(UserItem item)
        {
            var audio = item.Item.GetMediaStreams().Where(x => x.Type == MediaBrowser.Model.Entities.MediaStreamType.Audio);
            if (audio.Any())
                return ArrayValue<StringValue>.Create(audio.Select(x => x.DisplayTitle).Distinct().Select(x => StringValue.Create(x)).ToArray());

            return Value.None;
        }
    }
}