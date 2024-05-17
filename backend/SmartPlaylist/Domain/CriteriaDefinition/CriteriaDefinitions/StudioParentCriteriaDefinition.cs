
using MediaBrowser.Controller.Entities;
using SmartPlaylist.Domain.Rule;
using SmartPlaylist.Domain.Values;
using System.Linq;
using System.Collections.Generic;
using SmartPlaylist.Extensions;

namespace SmartPlaylist.Domain.CriteriaDefinition.CriteriaDefinitions
{
    public class StudioParentCriteriaDefinition : CriteriaDefinition
    {
        public override string Name => "Studio (Tree Crawl)";

        public override CriteriaDefinitionType Type => StringDefinitionType.Instance;

        public override Value GetValue(UserItem item)
        {
            if (item.Item is Video video && video.SupportsStudios)
                return ArrayValue<StringValue>.Create(RecurseStudio(video, new HashSet<string>()).Select(StringValue.Create).ToArray());

            return Value.None;
        }

        private HashSet<string> RecurseStudio(BaseItem item, HashSet<string> current)
        {
            item.Studios.Where(x => !current.Contains(x)).ForEach(x => current.Add(x));
            return (item.Parent != null && item.Parent.SupportsStudios) ? RecurseStudio(item.Parent, current) : current;
        }
    }
}