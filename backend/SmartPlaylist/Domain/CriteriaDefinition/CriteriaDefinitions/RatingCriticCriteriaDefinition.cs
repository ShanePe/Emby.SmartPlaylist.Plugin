using SmartPlaylist.Domain.Rule;
using SmartPlaylist.Domain.Values;

namespace SmartPlaylist.Domain.CriteriaDefinition.CriteriaDefinitions
{
    public class RatingCriticCriteriaDefinition : CriteriaDefinition
    {
        public override string Name => "Rating (Critic)";
        public override CriteriaDefinitionType Type => NumberDefinitionType.Instance;

        public override Value GetValue(UserItem item)
        {
            return item.Item.CriticRating.HasValue ? (Value)NumberValue.Create(item.Item.CriticRating.Value) : Value.None;
        }
    }
}