using SmartPlaylist.Domain.Rule;
using SmartPlaylist.Domain.Values;

namespace SmartPlaylist.Domain.CriteriaDefinition.CriteriaDefinitions
{
    public class RatingCommunityCriteriaDefinition : CriteriaDefinition
    {
        public override string Name => "Rating (Community)";
        public override CriteriaDefinitionType Type => NumberDefinitionType.Instance;

        public override Value GetValue(UserItem item)
        {
            return item.Item.CommunityRating.HasValue ? (Value)NumberValue.Create(item.Item.CommunityRating.Value) : Value.None;
        }
    }
}