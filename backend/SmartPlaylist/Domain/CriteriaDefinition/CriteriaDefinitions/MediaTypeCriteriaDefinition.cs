using System.Linq;
using SmartPlaylist.Domain.Rule;
using SmartPlaylist.Domain.Values;

namespace SmartPlaylist.Domain.CriteriaDefinition.CriteriaDefinitions
{
    public class MediaTypeCriteriaDefinition : CriteriaDefinition
    {
        public override string Name => "Media Type";
        public override CriteriaDefinitionType Type => new ListMapValueDefinitionType(Getter.SupportedTypes.First() as ListMapValue);
        public override Value[] Values => Getter.SupportedTypes;
        public override Value GetValue(UserItem item)
        {
            string name = item.Item.GetType().Name;
            return ListMapValue.Create(name, name);
        }
    }
}