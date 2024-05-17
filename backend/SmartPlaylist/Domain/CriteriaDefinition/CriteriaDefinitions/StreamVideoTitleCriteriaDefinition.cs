using SmartPlaylist.Domain.Rule;
using SmartPlaylist.Domain.Values;
using System.Linq;

namespace SmartPlaylist.Domain.CriteriaDefinition.CriteriaDefinitions
{
    public class StreamVideoTitleCriteriaDefinition : CriteriaDefinition
    {
        public override string Name => "Stream: Video Display Title";

        public override CriteriaDefinitionType Type => StringDefinitionType.Instance;

        public override Value GetValue(UserItem item)
        {
            var video = item.Item.GetMediaStreams().Where(x => x.Type == MediaBrowser.Model.Entities.MediaStreamType.Video);
            if (video.Any())
                return ArrayValue<StringValue>.Create(video.Select(x => x.DisplayTitle).Distinct().Select(x => StringValue.Create(x)).ToArray());

            return Value.None;
        }
    }
}