using System;
using System.Collections.Generic;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Entities.Audio;

namespace SmartPlaylist.Comparers
{
    public class ArtistsComparer : IComparer<BaseItem>, IComparable<BaseItem>
    {
        private readonly Func<Audio, string[]> _artistsPropSelector;

        public ArtistsComparer(Func<Audio, string[]> artistsPropSelector)
        {
            _artistsPropSelector = artistsPropSelector;
        }

        public int Compare(BaseItem x, BaseItem y)
        {
            var xaudio = x as Audio;
            var yaudio = y as Audio;

            if (xaudio != null && yaudio != null)
                return CompareStringArrays(_artistsPropSelector(xaudio), _artistsPropSelector(yaudio));

            if (xaudio != null) return -1;

            if (yaudio != null) return 1;

            return 0;
        }

        private static int CompareStringArrays(string[] x, string[] y)
        {
            for (var i = 0; i < x.Length && i < y.Length; i++)
            {
                var res = string.Compare(x[i], y[i], StringComparison.InvariantCultureIgnoreCase);
                if (res != 0)
                    return res;
            }

            return x.Length - y.Length;
        }

        public int CompareTo(BaseItem other)
        {

            throw new NotImplementedException();
        }
    }
}