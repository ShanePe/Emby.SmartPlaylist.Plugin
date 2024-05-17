using System;
using System.Collections.Generic;
using MediaBrowser.Controller.Entities;
using SmartPlaylist.Getters;
using SmartPlaylist.Services;

namespace SmartPlaylist.Comparers
{
    public class ReleaseDateComparer : IComparer<BaseItem>
    {
        public int Compare(BaseItem x, BaseItem y)
        {
            return GetReleaseDate(x).CompareTo(GetReleaseDate(y));
        }

        private DateTimeOffset GetReleaseDate(BaseItem baseItem)
        {
            return baseItem.PremiereDate.GetValueOrDefault(ReleaseDateGetter.Get(baseItem) ?? DateTimeOffset.MinValue);
        }
    }
}