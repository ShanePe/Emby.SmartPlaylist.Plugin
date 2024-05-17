using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SmartPlaylist.Extensions
{
    public static class AssemblyExtensions
    {
        public static IEnumerable<Type> FindDerivedTypes<TBase>(this Assembly assembly)
        {
            var baseType = typeof(TBase);
            return assembly.GetExportedTypes() //Only get Public types. ()
                .Where(t => t != baseType && !t.IsAbstract && baseType.IsAssignableFrom(t))
                .OrderBy(x => x.Name);
        }

        public static IEnumerable<TBase> FindAndCreateDerivedTypes<TBase>(this Assembly assembly)
        {
            return assembly
                .FindDerivedTypes<TBase>()
                .Where(x => x.GetConstructor(Type.EmptyTypes) != null)
                .Select(x =>
                {
                    try
                    {
                        return Activator.CreateInstance(x);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(x.Name);
                        Console.WriteLine(ex.Message);
                        return null;
                    }
                })
                .Where(x => x != null)
                .OfType<TBase>()
                .ToArray();
        }
    }
}