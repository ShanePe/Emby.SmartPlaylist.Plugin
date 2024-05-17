using SmartPlaylist.Contracts;

namespace SmartPlaylist.Domain.Rule
{
    public abstract class RuleBase
    {
        protected RuleBase(string id)
        {
            Id = id;
        }

        public string Id { get; }

        public abstract bool IsMatch(UserItem item);
        public abstract void Explain(HierarchyStringDto hs, int level, UserDto[] users);
    }
}