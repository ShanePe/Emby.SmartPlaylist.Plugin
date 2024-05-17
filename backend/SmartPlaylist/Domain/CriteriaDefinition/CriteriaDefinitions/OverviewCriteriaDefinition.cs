using SmartPlaylist.Domain.Rule;
using SmartPlaylist.Domain.Values;

namespace SmartPlaylist.Domain.CriteriaDefinition.CriteriaDefinitions
{
    public class OverviewCriteriaDefinition : CriteriaDefinition
    {
        public override string Name => "Overview";

        public override CriteriaDefinitionType Type => StringDefinitionType.Instance;

        public override Value GetValue(UserItem item)
        {
            if (!string.IsNullOrEmpty(item.Item.Overview))
                return StringValue.Create(item.Item.Overview);

            return Value.None;
        }
    }
}