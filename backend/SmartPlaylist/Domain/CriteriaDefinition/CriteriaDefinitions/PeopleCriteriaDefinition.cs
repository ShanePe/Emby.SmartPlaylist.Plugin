using System.Linq;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Model.Entities;
using SmartPlaylist.Domain.Rule;
using SmartPlaylist.Domain.Values;

namespace SmartPlaylist.Domain.CriteriaDefinition.CriteriaDefinitions
{
    public abstract class PeopleCriteriaDefinition : CriteriaDefinition
    {
        public abstract PersonType[] PersonTypes { get; }
        public override CriteriaDefinitionType Type => StringDefinitionType.Instance;
        public override Value GetValue(UserItem item)
        {
            if (item.Item is Video video && video.SupportsPeople)
            {
                var peoples = Plugin.Instance.LibraryManager.GetItemPeople(item.Item);

                return ArrayValue<StringValue>.Create(peoples.Where(x => PersonTypes.Any(x.IsType))
                    .Select(x => x.Name)?.OrderBy(x => x)?.Select(StringValue.Create).ToArray());
            }

            return Value.None;
        }
    }
}