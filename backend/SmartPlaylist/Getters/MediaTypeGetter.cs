using System.Linq;
using SmartPlaylist.Domain.CriteriaDefinition.CriteriaDefinitions;
using SmartPlaylist.Domain.Rule;
using SmartPlaylist.Extensions;

namespace SmartPlaylist.Domain
{
    public class MediaTypeGetter
    {
        public static string Get(RuleBase[] rules)
        {
            var mediaTypes = rules.OfType<RuleGroup>()
                .Flatten(x => x.Children.OfType<RuleGroup>())
                .SelectMany(x => x.Children)
                .OfType<Rule.Rule>()
                .Select(x => x.Criteria)
                .Where(x => x.Definition is MediaTypeCriteriaDefinition)
                .Select(x => x.Value.ToString())
                .Distinct()
                .ToArray();

            MediaTypeDescriptor descriptor = Const.SupportedItemTypes.FirstOrDefault(x => mediaTypes.Contains(x.Description));
            return descriptor != null ? descriptor.BaseType : MediaBrowser.Model.Entities.MediaType.Audio;
        }
    }
}