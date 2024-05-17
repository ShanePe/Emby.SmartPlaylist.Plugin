using System.Diagnostics;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Entities.TV;
using SmartPlaylist.Domain.Rule;
using SmartPlaylist.Domain.Values;
using System.Linq;

namespace SmartPlaylist.Domain.CriteriaDefinition.CriteriaDefinitions
{
    public class ResolutionWidthCriteriaDefinition : CriteriaDefinition
    {
        public override string Name => "Resolution Width";

        public override CriteriaDefinitionType Type => NumberDefinitionType.Instance;

        public override Value GetValue(UserItem item)
        {
            var video = item.Item.GetMediaStreams().Where(x => x.Type == MediaBrowser.Model.Entities.MediaStreamType.Video);
            if (video.Any())
                return ArrayValue<NumberValue>.Create(video.Select(x => x.Width.Value).Distinct().Select(x => NumberValue.Create(x)).ToArray());
            else if (item.Item is Video videoItem)
                return NumberValue.Create(videoItem.Width);

            return Value.None;
        }
    }
}