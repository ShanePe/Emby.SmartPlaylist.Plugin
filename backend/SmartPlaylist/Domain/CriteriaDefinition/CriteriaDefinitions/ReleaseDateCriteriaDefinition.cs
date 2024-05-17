using SmartPlaylist.Domain.Rule;
using SmartPlaylist.Domain.Values;
using SmartPlaylist.Getters;

namespace SmartPlaylist.Domain.CriteriaDefinition.CriteriaDefinitions
{
    public class ReleaseDateCriteriaDefinition : CriteriaDefinition
    {
        public override string Name => "Release date";
        public override CriteriaDefinitionType Type => DateDefinitionType.Instance;

        public override Value GetValue(UserItem item)
        {
            var releaseDate = ReleaseDateGetter.Get(item.Item);
            if (releaseDate.HasValue) return DateValue.Create(releaseDate.Value);

            return Value.None;
        }
    }
}