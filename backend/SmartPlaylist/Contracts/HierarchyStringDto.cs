using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartPlaylist.Contracts
{
    [Serializable]
    public class HierarchyStringDto
    {
        private List<HierarchyStringDto> _children = new List<HierarchyStringDto>();
        public string Value { get; set; }
        internal bool Container { get; set; }
        public HierarchyStringDto[] Children => _children.ToArray();

        public int Level { get; set; }

        internal HierarchyStringDto(string value, int level)
        {
            Value = value;
            Level = level;
        }

        internal HierarchyStringDto(string value, int level, bool container) : this(value, level)
        {
            Container = container;
        }


        internal void AddChild(HierarchyStringDto value)
        {
            _children.Add(value);
        }

        internal void AddChild(string value, int level)
        {
            _children.Add(new HierarchyStringDto(value, level));
        }

        internal string JoinChildren(string sep)
        {
            return string.Join(sep, _children.Select(x => x.Value));
        }

        internal void CompressContainers()
        {
            CompressContainers(0);
        }
        internal void CompressContainers(int level)
        {
            if (Container)
            {
                string containerHeader = Value;
                Value = $"({string.Join($" {Value} ", _children.Where(x => !x.Container).Select(x => x.Value))})";
                HierarchyStringDto header = new HierarchyStringDto(containerHeader, level);
                header._children.AddRange(_children.Where(x => x.Container).Select(x => x));
                header._children.ForEach(x => x.Level = level);
                _children.Clear();
                if (header._children.Count > 0)
                    _children.Add(header);
            }

            if (_children.Count > 0)
            {
                level++;
                _children.ForEach(x => x.CompressContainers(level));
            }
        }

        public override string ToString()
        {
            return $"{Value} {string.Join(" ", Children.Select(x => x.ToString()))} ";
        }
    }
}