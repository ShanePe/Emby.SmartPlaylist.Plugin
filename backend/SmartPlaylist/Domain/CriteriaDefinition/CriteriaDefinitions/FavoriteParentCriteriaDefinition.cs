using System.Collections.Generic;
using MediaBrowser.Controller.Entities;
using SmartPlaylist.Domain.Rule;
using SmartPlaylist.Domain.Values;
using System.Linq;

namespace SmartPlaylist.Domain.CriteriaDefinition.CriteriaDefinitions
{
    public class FavoriteParentCriteriaDefinition : UserCriteriaDefinition
    {
        public override string Name => "Favorite (Tree Crawl)";
        public override CriteriaDefinitionType Type => new ListValueDefinitionType(Getter.FavoriteLiked[Getter.FavoriteLikedEnum.Favorite]);
        public override Value[] Values => Getter.FavoriteLiked.Values.ToArray<Value>();
        public override Value GetValue(UserItem item)
        {
            BaseItem find = RecurseFavorite(item.Item, new HashSet<BaseItem>()).FirstOrDefault(x => x.IsFavoriteOrLiked(item.User));
            if (find != null && UserItem.TryGetUserItemData(out var userData, item.User, find))
            {
                return userData.IsFavorite ? Getter.FavoriteLiked[Getter.FavoriteLikedEnum.Favorite] : Getter.FavoriteLiked[Getter.FavoriteLikedEnum.Liked];
            }

            return Getter.FavoriteLiked[Getter.FavoriteLikedEnum.None];
        }

        private HashSet<BaseItem> RecurseFavorite(BaseItem item, HashSet<BaseItem> current)
        {
            if (!current.Contains(item))
                current.Add(item);

            return (item.Parent != null) ? RecurseFavorite(item.Parent, current) : current;
        }
    }
}