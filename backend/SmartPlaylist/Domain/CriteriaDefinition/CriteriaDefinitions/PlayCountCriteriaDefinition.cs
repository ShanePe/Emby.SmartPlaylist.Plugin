using SmartPlaylist.Domain.Rule;
using SmartPlaylist.Domain.Values;

namespace SmartPlaylist.Domain.CriteriaDefinition.CriteriaDefinitions
{
    public class PlayCountCriteriaDefinition : UserCriteriaDefinition
    {
        public override string Name => "Play Count";
        public override CriteriaDefinitionType Type => NumberDefinitionType.Instance;

        public override Value GetValue(UserItem item)
        {
            if (item.TryGetUserItemData(out var userItemData)) return NumberValue.Create(userItemData.PlayCount);

            return Value.None;
        }
    }
}