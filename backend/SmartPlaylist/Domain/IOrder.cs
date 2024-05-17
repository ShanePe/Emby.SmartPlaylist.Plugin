using System;
using System.Collections.Generic;
using System.Linq;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Querying;
using SmartPlaylist.Comparers;
using SmartPlaylist.Contracts;
using SmartPlaylist.Extensions;

namespace SmartPlaylist.Domain
{
    public static class Sorter
    {

        public static BaseItem[] Sort(BaseItem[] items, SmartPlaylist smartPlaylist, params IOrder[] orders)
        {

            foreach (IOrder order in orders)
            {
                order.SmartPlaylist = smartPlaylist;
            }

            if (orders.Any(x => x.IsShuffle))
                return items.Shuffle();

            if (orders.Length == 0)
                return items;

            var orderedEnumerator = orders[0].Direction == SortOrder.Ascending
                ? orders[0].Comparer == null ? items.OrderBy(orders[0].Function) : items.OrderBy(x => x, orders[0].Comparer)
                : orders[0].Comparer == null ? items.OrderByDescending(orders[0].Function) : items.OrderByDescending(x => x, orders[0].Comparer);


            if (orders.Length > 1)
            {
                orderedEnumerator = orders.Skip(1).Aggregate(orderedEnumerator, (current, thenBy) => thenBy.Direction == SortOrder.Ascending
                 ? thenBy.Comparer == null ? current.ThenBy(thenBy.Function) : current.ThenBy(x => x, thenBy.Comparer)
                 : thenBy.Comparer == null ? current.ThenByDescending(thenBy.Function) : current.ThenByDescending(x => x, thenBy.Comparer));
            }

            return orderedEnumerator.ToArray();

        }
    }
    public class SmartPlaylistLimit
    {
        public SmartPlaylistLimit() { }
        public SmartPlaylistLimit(SmartPlaylistLimitDto dto)
        {
            MaxItems = dto.HasLimit ? dto.MaxItems : 100000;
            OrderBy = dto.HasLimit ? IOrder.GetOrderFromString(dto.OrderBy) : new OrderNone();
        }

        public static readonly SmartPlaylistLimit None = new SmartPlaylistLimit
        {
            MaxItems = 100000,
            OrderBy = new OrderNone()
        };

        public int MaxItems { get; set; }

        public IOrder OrderBy { get; set; }

        public bool HasLimit => !(OrderBy is OrderNone);

        public SmartPlaylistLimitDto ToDto()
        {
            return new SmartPlaylistLimitDto()
            {
                HasLimit = HasLimit,
                MaxItems = HasLimit ? MaxItems : 0,
                OrderBy = OrderBy.Name
            };
        }
    }

    public class OrderNone : IOrder
    {
        public override string Name => "None";
    }

    public static class DefinedOrders
    {
        public static readonly IOrder[] All = typeof(IOrder).Assembly.FindAndCreateDerivedTypes<IOrder>()
            .Where(x => x.GetType() != SmartPlaylistLimit.None.OrderBy.GetType()).ToArray();

        public static readonly string[] AllNames = All.Select(x => x.Name).ToArray();
    }

    public abstract class IOrder
    {
        public abstract string Name { get; }
        public static IOrder GetOrderFromString(string orderBy)
        {
            return DefinedOrders.All.FirstOrDefault(x =>
                                string.Equals(x.Name, orderBy, StringComparison.CurrentCultureIgnoreCase)) ??
                          SmartPlaylistLimit.None.OrderBy;
        }

        public virtual bool IsShuffle => false;
        public virtual Func<BaseItem, IComparable> Function => x => (IComparable)x;
        public virtual IComparer<BaseItem> Comparer { get; }
        public virtual SortOrder Direction => SortOrder.Ascending;
        public virtual SmartPlaylist SmartPlaylist { get; set; }
    }

    public class OrderRandom : IOrder
    {
        public override string Name => "Random";
        public override bool IsShuffle => true;
    }

    public class OrderAlbum : IOrder
    {
        public override string Name => "Album";

        public override Func<BaseItem, IComparable> Function => x => x.Album;
    }


    public class OrderArtist : IOrder
    {
        public override string Name => "Artist";
        public override IComparer<BaseItem> Comparer => new ArtistsComparer(a => a.Artists);
    }

    public class OrderAlbumArtist : IOrder
    {
        public override string Name => "Album artist";
        public override IComparer<BaseItem> Comparer => new ArtistsComparer(a => a.AlbumArtists);
    }

    public class OrderMostFavorite : IOrder
    {
        public override string Name => "Most favorite";
        public override SortOrder Direction => SortOrder.Descending;
        public override Func<BaseItem, IComparable> Function => x => x.IsFavorite;
    }

    public class OrderLessFavorite : IOrder
    {
        public override string Name => "Less favorite";
        public override Func<BaseItem, IComparable> Function => x => x.IsFavorite;
    }


    public class OrderAddedDateDesc : IOrder
    {
        public override string Name => "Added date desc";
        public override SortOrder Direction => SortOrder.Descending;
        public override Func<BaseItem, IComparable> Function => x => x.DateCreated;
    }

    public class OrderAddedDateAsc : IOrder
    {
        public override string Name => "Added date asc";
        public override Func<BaseItem, IComparable> Function => x => x.DateCreated;
    }

    public class OrderMostPlayed : IOrder
    {
        public override string Name => "Most played";
        public override SortOrder Direction => SortOrder.Descending;
        public override Func<BaseItem, IComparable> Function => x => x.PlayCount;
    }

    public class OrderLeastPlayed : IOrder
    {
        public override string Name => "Least played";
        public override Func<BaseItem, IComparable> Function => x =>
        {
            //this.SmartPlaylist.Log($"OrderLeastPlayed:{x.Name}:PlayCount:{x.PlayCount}");
            return x.PlayCount;
        };
    }

    public class OrderPlayedDateDesc : IOrder
    {
        public override string Name => "Played date desc";
        public override SortOrder Direction => SortOrder.Descending;
        public override Func<BaseItem, IComparable> Function => x => x.LastPlayedDate;
    }

    public class OrderPlayedDateAsc : IOrder
    {
        public override string Name => "Played date asc";
        public override Func<BaseItem, IComparable> Function => x =>
        {
            this.SmartPlaylist.Log($"Sort: Last Play date:{x.Name}:{x.LastPlayedDate}");
            return x.LastPlayedDate;
        };
    }

    public class OrderName : IOrder
    {
        public override string Name => "Name";
        public override Func<BaseItem, IComparable> Function => x => x.Name;
    }

    public class OrderEpisode : IOrder
    {
        public override string Name => "Episode";
        public override IComparer<BaseItem> Comparer => new EpisodeComparer();

    }

    public class OrderSortName : IOrder
    {
        public override string Name => "SortName asc";
        public override Func<BaseItem, IComparable> Function => x => x.SortName;
    }

    public class OrderSortNameDesc : IOrder
    {
        public override string Name => "SortName desc";
        public override SortOrder Direction => SortOrder.Descending;
        public override Func<BaseItem, IComparable> Function => x => x.SortName;
    }

    public class OrderReleaseDate : IOrder
    {
        public override string Name => "Release date asc";
        public override IComparer<BaseItem> Comparer => new ReleaseDateComparer();
    }

    public class OrderReleaseDateDesc : IOrder
    {
        public override string Name => "Release date desc";
        public override SortOrder Direction => SortOrder.Descending;
        public override IComparer<BaseItem> Comparer => new ReleaseDateComparer();
    }

    public class OrderRuntime : IOrder
    {
        public override string Name => "Runtime asc";
        public override Func<BaseItem, IComparable> Function => x => x.RunTimeTicks;
    }

    public class OrderRuntimeDesc : IOrder
    {
        public override string Name => "Runtime desc";
        public override SortOrder Direction => SortOrder.Descending;
        public override Func<BaseItem, IComparable> Function => x => x.RunTimeTicks;

    }

    public class OrderCommunityRating : IOrder
    {
        public override string Name => "Community rating asc";
        public override Func<BaseItem, IComparable> Function => x => x.CommunityRating;
    }

    public class OrderCommunityRatingDesc : IOrder
    {
        public override string Name => "Community rating desc";
        public override SortOrder Direction => SortOrder.Descending;
        public override Func<BaseItem, IComparable> Function => x => x.CommunityRating;
    }

    public class OrderParentalRating : IOrder
    {
        public override string Name => "Parental rating asc";
        public override Func<BaseItem, IComparable> Function => x => x.GetInheritedParentalRatingValue();
    }

    public class OrderParentalRatingDesc : IOrder
    {
        public override string Name => "Parental rating desc";
        public override SortOrder Direction => SortOrder.Descending;
        public override Func<BaseItem, IComparable> Function => x => x.GetInheritedParentalRatingValue();
    }
}