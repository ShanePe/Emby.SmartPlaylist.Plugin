using System;
using SmartPlaylist.Extensions;

namespace SmartPlaylist.Domain.Values
{
    public class RegexValue : StringValue
    {
        public bool CaseSensitive { get; set; }
        public static readonly new RegexValue Default = new RegexValue(string.Empty, false);

        public RegexValue(string value, bool caseSensitive)
        : base(value)
        {
            CaseSensitive = caseSensitive;
        }

        public override string Kind => "regex";

        internal override string Friendly => $"Regex: {Value}, case sensitive:{CaseSensitive}";
        public static RegexValue Create(string value, bool caseSensitive)
        {
            return new RegexValue(value, caseSensitive);
        }
    }
}