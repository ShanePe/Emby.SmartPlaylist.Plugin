using SmartPlaylist.Domain.Rule;
using SmartPlaylist.Domain.Values;

namespace SmartPlaylist.Domain.CriteriaDefinition.CriteriaDefinitions
{
    public class PlayedCriteriaDefinition : UserCriteriaDefinition
    {
        public override string Name => "Played";
        public override CriteriaDefinitionType Type => BoolValueDefinitionType.Instance;
        public override Value GetValue(UserItem item)
        {
            if (item.TryGetUserItemData(out var userItemData)) return BoolValue.Create(userItemData.Played);

            return Value.None;
        }
    }
}