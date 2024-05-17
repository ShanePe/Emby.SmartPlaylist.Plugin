using System.Collections.Generic;
using System.Linq;
using SmartPlaylist.Contracts;
using SmartPlaylist.Extensions;
namespace SmartPlaylist.Domain.Rule
{
    public class RuleGroup : RuleBase
    {
        public static string Kind = "ruleGroup";

        public RuleGroup(string id, RuleBase[] children, RuleGroupMatchMode matchMode) : base(id)
        {
            Children = children;
            MatchMode = matchMode;
        }

        public RuleBase[] Children { get; }
        public RuleGroupMatchMode MatchMode { get; }

        public override void Explain(HierarchyStringDto hs, int level, UserDto[] users)
        {
            HierarchyStringDto child = null;
            switch (MatchMode)
            {
                case RuleGroupMatchMode.All:
                    child = new HierarchyStringDto("AND", level, true);
                    break;
                case RuleGroupMatchMode.Any:
                    child = new HierarchyStringDto("OR", level, true);
                    break;
            }

            if (child != null)
            {
                level++;
                Children.ForEach(x => x.Explain(child, level, users));
                hs.AddChild(child);
            }
        }

        public override bool IsMatch(UserItem item)
        {
            switch (MatchMode)
            {
                case RuleGroupMatchMode.All:
                    return Children.All(x => x.IsMatch(item));
                case RuleGroupMatchMode.Any:
                    return Children.Any(x => x.IsMatch(item));
                default:
                    return false;
            }
        }
    }
}