using System.Linq;
using SmartPlaylist.Domain.Rule;
using SmartPlaylist.Domain.Values;

namespace SmartPlaylist.Domain.CriteriaDefinition.CriteriaDefinitions
{
    public class GenreListedCriteriaDefinition : CriteriaDefinition
    {
        public override string Name => "Genre (Listed)";
        public override CriteriaDefinitionType Type => new ListValueDefinitionType(Getter.Genres.First());
        public override Value[] Values => Getter.Genres;
        public override Value GetValue(UserItem item)
        {
            return ArrayValue<ListValue>.Create(item.Item.Genres?.Select(s => ListValue.Create(s)).ToArray());
        }
    }
}