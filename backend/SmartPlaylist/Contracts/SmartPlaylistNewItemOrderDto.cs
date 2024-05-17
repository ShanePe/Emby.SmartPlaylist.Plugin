using System;

namespace SmartPlaylist.Contracts
{
    [Serializable]
    public class SmartPlaylistNewItemOrderDto
    {
        public static SmartPlaylistNewItemOrderDto Default => new SmartPlaylistNewItemOrderDto()
        {
            HasSort = false,
            OrderBy = "None"
        };
        public bool HasSort { get; set; }
        public string OrderBy { get; set; }
    }
}