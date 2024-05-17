using System;
using SmartPlaylist.Domain.Values;
using SmartPlaylist.Parsers.JsonValueParsers;

namespace SmartPlaylist.Contracts
{
    [Serializable]
    public class RuleCriteriaValueDto
    {
        private Value _value;
        public string Name { get; set; }
        public string UserId { get; set; }

        public object Value
        {
            get => _value;
            set => _value = ValueParser.Parse(value);
        }

        public OperatorDto Operator { get; set; }
    }
}