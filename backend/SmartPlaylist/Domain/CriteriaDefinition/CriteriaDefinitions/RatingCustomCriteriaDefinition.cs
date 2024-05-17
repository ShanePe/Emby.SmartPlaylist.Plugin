using SmartPlaylist.Domain.Rule;
using SmartPlaylist.Domain.Values;

namespace SmartPlaylist.Domain.CriteriaDefinition.CriteriaDefinitions
{
    public class RatingCustomCriteriaDefinition : CriteriaDefinition
    {
        public override string Name => "Rating (Custom)";
        public override CriteriaDefinitionType Type => StringDefinitionType.Instance;
        public override Value GetValue(UserItem item)
        {
            return string.IsNullOrEmpty(item.Item.CustomRating) ? Value.None : (Value)StringValue.Create(item.Item.CustomRating);
        }
    }
}