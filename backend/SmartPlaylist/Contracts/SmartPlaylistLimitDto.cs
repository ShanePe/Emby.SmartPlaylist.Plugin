using System;
using SmartPlaylist.Domain;

namespace SmartPlaylist.Contracts
{
    [Serializable]
    public class SmartPlaylistLimitDto
    {
        public bool HasLimit { get; set; }
        public int MaxItems { get; set; }
        public string OrderBy { get; set; }
    }
}