using System;
using System.Collections.Generic;
using System.Linq;
using SmartPlaylist.Contracts;
using SmartPlaylist.Domain;
using SmartPlaylist.Domain.Rule;

namespace SmartPlaylist.Adapters
{
    public static class SmartPlaylistAdapter
    {
        public static Domain.SmartPlaylist Adapt(SmartPlaylistDto dto)
        {
            return new Domain.SmartPlaylist(dto);
        }

        public static Domain.SmartPlaylist[] Adapt(IEnumerable<SmartPlaylistDto> smartPlaylistDtos)
        {
            return smartPlaylistDtos.Select(ns =>
                Adapt(ns
                )).ToArray();
        }
    }
}