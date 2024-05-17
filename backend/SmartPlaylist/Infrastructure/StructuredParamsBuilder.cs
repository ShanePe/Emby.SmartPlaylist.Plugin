using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SmartPlaylist.Infrastructure
{
    public static class StructuredParamsBuilder
    {
        public static IDictionary<string, string> BuildDict(params object[] @params)
        {
            IDictionary<string, string> paramsDict = new Dictionary<string, string>();
            foreach (var param in @params)
            {
                var newParamsDict = GetPropertiesInOrder(param)
                    .ToDictionary(x => x.Name, y => y.GetValue(param).ToString());

                paramsDict = paramsDict.Concat(newParamsDict)
                    .ToLookup(pair => pair.Key, pair => pair.Value)
                    .ToDictionary(group => group.Key, group => group.FirstOrDefault());
            }

            return paramsDict;
        }

        public static string BuildStr(params object[] @params)
        {
            var dict = BuildDict(@params);
            return $"{{{string.Join(", ", dict.Select(x => $"{x.Key}:'{x.Value}'"))}}}";
        }

        public static IEnumerable<PropertyInfo> GetPropertiesInOrder<T>(T obj)
        {
            var type = obj.GetType();
            var ctor = type.GetConstructors().Single();
            var parameters = ctor.GetParameters().OrderBy(p => p.Position);
            foreach (var p in parameters) yield return type.GetProperty(p.Name);
        }
    }
}