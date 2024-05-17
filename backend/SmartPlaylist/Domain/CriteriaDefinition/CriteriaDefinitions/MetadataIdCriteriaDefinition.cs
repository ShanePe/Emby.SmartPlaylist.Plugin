using MediaBrowser.Model.Entities;

using SmartPlaylist.Domain.Rule;
using SmartPlaylist.Domain.Values;

namespace SmartPlaylist.Domain.CriteriaDefinition.CriteriaDefinitions
{
    public abstract class MetadataIdCriteriaDefinition : CriteriaDefinition
    {
        public override string Name => $"Metadata: {ProviderType} Id";
        public override CriteriaDefinitionType Type => StringDefinitionType.Instance;

        protected abstract MetadataProviders ProviderType { get; }

        public override Value GetValue(UserItem item)
        {
            var providerId = item.Item.GetProviderId(ProviderType);

            if (string.IsNullOrWhiteSpace(providerId))
                return Value.None;

            return StringValue.Create(providerId);
        }
    }

    public class ImdbCriteriaDefinition : MetadataIdCriteriaDefinition
    {
        protected override MetadataProviders ProviderType => MetadataProviders.Imdb;
    }

    public class TvdbCriteriaDefinition : MetadataIdCriteriaDefinition
    {
        protected override MetadataProviders ProviderType => MetadataProviders.Tvdb;
    }

    public class TmdbCriteriaDefinition : MetadataIdCriteriaDefinition
    {
        protected override MetadataProviders ProviderType => MetadataProviders.Tmdb;
    }

    public class TmdbCollectionCriteriaDefinition : MetadataIdCriteriaDefinition
    {
        protected override MetadataProviders ProviderType => MetadataProviders.TmdbCollection;
    }
}