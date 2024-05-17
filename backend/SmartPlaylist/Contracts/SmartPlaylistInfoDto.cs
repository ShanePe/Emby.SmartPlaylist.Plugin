
using System;
using System.Reflection;

namespace SmartPlaylist.Contracts
{
    [Serializable]
    public class SmartPlaylistInfoDto : SmartPlaylistDto
    {
        public string LastDurationStr { get; set; }
        public string[] Items { get; set; }
        public int RuleCount { get; set; }
        public static SmartPlaylistInfoDto FromSmartPlaylist(SmartPlaylistDto smartPlaylist)
        {
            SmartPlaylistInfoDto infoDto = new SmartPlaylistInfoDto();
            foreach (PropertyInfo srcProp in smartPlaylist.GetType().GetProperties())
            {
                PropertyInfo desProp = infoDto.GetType().GetProperty(srcProp.Name);
                if (desProp != null && desProp.CanWrite)
                    desProp.SetValue(infoDto, srcProp.GetValue(smartPlaylist));
            }

            infoDto.LastDurationStr = String.Format("{0:%h} hours {0:%m} minutes and {0:%s}.{0:%f} seconds", TimeSpan.FromMilliseconds(smartPlaylist.LastSyncDuration));
            infoDto.RuleCount = smartPlaylist.RulesTree.Length;


            return infoDto;
        }
    }
}