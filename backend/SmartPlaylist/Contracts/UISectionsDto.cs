
using System;

namespace SmartPlaylist.Contracts
{
    [Serializable]
    public class UISectionsDto
    {
        public static UISectionsDto Default => new UISectionsDto()
        {
            Setup = true,
            Sort = true,
            Rules = true,
            Notes = false
        };

        public bool Setup { get; set; }
        public bool Sort { get; set; }
        public bool Rules { get; set; }
        public bool Notes { get; set; }
    }
}