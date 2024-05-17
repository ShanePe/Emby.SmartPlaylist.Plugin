using SmartPlaylist.Domain.Rule;
using SmartPlaylist.Domain.Values;
using System.Linq;

namespace SmartPlaylist.Domain.CriteriaDefinition.CriteriaDefinitions
{
    public class StreamVideoCodecCriteriaDefinition : CriteriaDefinition
    {
        public override string Name => "Stream: Video Codec";
        public override CriteriaDefinitionType Type => new ListValueDefinitionType(Getter.VideoCodecs.First());
        public override Value[] Values => Getter.VideoCodecs;
        public override Value GetValue(UserItem item)
        {
            var video = item.Item.GetMediaStreams().Where(x => x.Type == MediaBrowser.Model.Entities.MediaStreamType.Video);
            if (video.Any())
                return ArrayValue<ListValue>.Create(video.Select(x => x.Codec).Distinct().Select(x => ListValue.Create(x)).ToArray());

            return Value.None;
        }
    }
}