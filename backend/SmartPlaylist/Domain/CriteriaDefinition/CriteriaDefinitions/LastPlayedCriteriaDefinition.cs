using SmartPlaylist.Domain.Rule;
using SmartPlaylist.Domain.Values;

namespace SmartPlaylist.Domain.CriteriaDefinition.CriteriaDefinitions
{
    public class LastPlayedCriteriaDefinition : UserCriteriaDefinition
    {
        public override string Name => "Last Played";
        public override CriteriaDefinitionType Type => DateDefinitionType.Instance;

        public override Value GetValue(UserItem item)
        {
            if (item.TryGetUserItemData(out var userItemData) && userItemData.Played &&
                userItemData.LastPlayedDate.HasValue) return DateValue.Create(userItemData.LastPlayedDate.Value);

            return Value.None;
        }
    }
}