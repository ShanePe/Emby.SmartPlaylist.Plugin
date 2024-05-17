using SmartPlaylist.Domain.Rule;
using SmartPlaylist.Domain.Values;
using System.Linq;

namespace SmartPlaylist.Domain.CriteriaDefinition.CriteriaDefinitions
{
    public class FavoriteCriteriaDefinition : UserCriteriaDefinition
    {
        public override string Name => "Favorite";
        public override CriteriaDefinitionType Type => new ListValueDefinitionType(Getter.FavoriteLiked[Getter.FavoriteLikedEnum.Favorite]);
        public override Value[] Values => Getter.FavoriteLiked.Values.ToArray<Value>();
        public override Value GetValue(UserItem item)
        {
            if (item.TryGetUserItemData(out var userData))
            {
                if (userData.IsFavorite) return Getter.FavoriteLiked[Getter.FavoriteLikedEnum.Favorite];
            }

            return Getter.FavoriteLiked[Getter.FavoriteLikedEnum.None];
        }
    }
}