using System.Collections.Generic;
using System.Linq;
using MediaBrowser.Controller.Entities;
using SmartPlaylist.Domain.Rule;
using SmartPlaylist.Domain.Values;
using SmartPlaylist.Extensions;
using System;

namespace SmartPlaylist.Domain.CriteriaDefinition.CriteriaDefinitions
{
    public class TagParentCriteriaDefinition : CriteriaDefinition
    {
        public override string Name => "Tag (Tree Crawl)";
        public override CriteriaDefinitionType Type => StringDefinitionType.Instance;

        public override Value GetValue(UserItem item)
        {
            return ArrayValue<StringValue>.Create(RecurseTags(item.Item, new HashSet<string>()).Select(StringValue.Create).ToArray());
        }

        private HashSet<string> RecurseTags(BaseItem item, HashSet<string> current)
        {
            item.Tags.Where(x => !current.Contains(x)).ForEach(x => current.Add(x));
            return (item.Parent != null && item.Parent.SupportsTags) ? RecurseTags(item.Parent, current) : current;
        }
    }
}