using System;
using MediaBrowser.Controller.Entities;
using SmartPlaylist.Domain.Values;

namespace SmartPlaylist.Domain.Rule
{
    public class RuleCriteriaValue
    {
        public RuleCriteriaValue(Value value, Operator.Operator @operator, string userId,
            CriteriaDefinition.CriteriaDefinition criteriaDefinition)
        {
            Value = value;
            Operator = @operator;
            UserId = userId;
            Definition = criteriaDefinition;
        }

        public Value Value { get; }
        public string UserId { get; set; }

        public Operator.Operator Operator { get; }

        public CriteriaDefinition.CriteriaDefinition Definition { get; }

        private (Value value, string user) GetItemValueForUser(UserItem item)
        {
            Guid userId;
            if (!Guid.TryParse(UserId, out userId) || item.User.Id == userId) return GetItemValue(item);

            User user = Plugin.Instance.GetOrCreateGlobalCache<User>(userId, () =>
            {
                return Plugin.Instance.UserManager.GetUserById(userId);
            });
            if (user == null) return GetItemValue(item);

            return GetItemValue(new UserItem(user, item.Item, item.SmartPlaylist));
        }

        private (Value value, string user) GetItemValue(UserItem item)
        {
            return (Definition.GetValue(item), item.User.Name);
        }

        public bool IsMatch(UserItem item)
        {
            var itemValue = item.Item.SupportsUserData && Definition.IsUserSpecific && !string.IsNullOrEmpty(UserId)
                    ? GetItemValueForUser(item)
                    : GetItemValue(item);
            try
            {


                if (Operator.CanCompare(itemValue.value, Value))
                {
                    bool result = Operator.Compare(itemValue.value, Value);
                    item.SmartPlaylist.Log($"Comparing: '{item.Item.Name}', Field: '{Definition.Name}', Expected: '{Value.Friendly}', Actual: '{itemValue.value.Friendly}', Operator: '{Operator.Name}', Type: '{itemValue.value.Kind.ToString()}', Context '{itemValue.user}', Matched: {result}");
                    return result;
                }
                else
                {
                    item.SmartPlaylist.Log(
                        itemValue.value.IsNone ?
                        $"Incomparable: '{item.Item.Name}', Field: '{Definition.Name}': 'No metadata value found'" :
                        $"Incomparable: '{item.Item.Name}', Field: '{Definition.Name}', Expected: '{Value.Friendly}', Actual: '{itemValue.value.Friendly}', Operator: '{Operator.Name}', Type: '{itemValue.value.Kind.ToString()}', Context '{itemValue.user}'");
                    return false;
                }
            }
            catch (Exception ex)
            {
                item.SmartPlaylist.Log($"Error: {item.Item.Name}, Field: '{Definition.Name}', Metadata '{Value.Friendly}': {ex.Message}");
                return false;
            }
        }
    }
}