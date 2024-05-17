using System.Collections.Generic;
using System.Linq;
using MediaBrowser.Controller.Entities;
using SmartPlaylist.Domain.Rule;
using SmartPlaylist.Domain.Values;
using SmartPlaylist.Extensions;

namespace SmartPlaylist.Domain.CriteriaDefinition.CriteriaDefinitions
{
    public class GenreParentCriteriaDefinition : CriteriaDefinition
    {
        public override string Name => "Genre (Tree Crawl)";
        public override CriteriaDefinitionType Type => StringDefinitionType.Instance;

        public override Value GetValue(UserItem item)
        {
            return ArrayValue<StringValue>.Create(RecurseGenre(item.Item, new HashSet<string>()).Select(StringValue.Create).ToArray());
        }

        private HashSet<string> RecurseGenre(BaseItem item, HashSet<string> current)
        {
            item.Genres.ForEach(g =>
            {
                if (!current.Contains(g))
                    current.Add(g);
            });

            return (item.Parent != null && item.Parent.SupportsGenres) ? RecurseGenre(item.Parent, current) : current;
        }
    }
}