using System;
using System.IO;
using System.Text.RegularExpressions;

namespace SmartPlaylist.Infrastructure
{
    public class DataUrl
    {
        public string ContentType { get; }

        public byte[] FileData { get; }

        public DataUrl(string dataUrl)
        {
            var matches = Regex.Match(dataUrl, @"data:(?<type>.+?);base64,(?<data>.+)");

            if (matches.Groups.Count < 3)
            {
                throw new Exception("Invalid DataUrl format");
            }

            ContentType = matches.Groups["type"].Value;

            FileData = Convert.FromBase64String(matches.Groups["data"].Value);
        }

    }
}