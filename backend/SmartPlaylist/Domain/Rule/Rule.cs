using SmartPlaylist.Contracts;
using SmartPlaylist.Extensions;
namespace SmartPlaylist.Domain.Rule
{
    public class Rule : RuleBase
    {
        public static string Kind = "rule";

        public Rule(string id, RuleCriteriaValue criteria) : base(id)
        {
            Criteria = criteria;
        }

        public RuleCriteriaValue Criteria { get; }

        public override void Explain(HierarchyStringDto hs, int level, UserDto[] users)
        {
            hs.AddChild(
               (Criteria.Operator.Valueless ?
               $"{Criteria.Definition.Name} {Criteria.Operator.Name}" :
               $"{Criteria.Definition.Name} {Criteria.Operator.Name} {Criteria.Value.Friendly}") +
                    (Criteria.Definition.IsUserSpecific ? $" for {users.GetUserById(Criteria.UserId)}" : "")
               , level);
        }

        public override bool IsMatch(UserItem item)
        {
            return Criteria.IsMatch(item);
        }
    }
}