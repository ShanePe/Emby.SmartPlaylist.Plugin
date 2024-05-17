using System;
using System.Collections.Specialized;
using System.Linq;
using SmartPlaylist.Contracts;

namespace SmartPlaylist.Extensions
{
    public static class StringExtensions
    {

        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source?.IndexOf(toCheck, comp) >= 0;
        }

        public static string[] ToArray(this StringCollection sc)
        {
            string[] ret = new string[sc.Count];
            for (int i = 0; i < sc.Count; i++)
                ret[i] = sc[i];
            return ret;
        }

        public static string GetUserById(this UserDto[] users, string id)
        {
            UserDto user = users.FirstOrDefault(x => x.Id.Equals(id));
            return user == null ? "[Current User]" : user.Name;
        }
    }
}