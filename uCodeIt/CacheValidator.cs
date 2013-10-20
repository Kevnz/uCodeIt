using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Web.Compilation;
using System.Xml.Linq;
using uCodeIt.DocumentTypes;
using Umbraco.Core;
using Umbraco.Core.IO;

namespace uCodeIt
{
    internal static class CacheValidator
    {
        private const string BuildManagerTracker = "uCodeIt-Tracker.txt";
        private const string TypeCache = "~/App_Data/uCodeIt-TypeCache.xml";

        private static Dictionary<Type, string> cache;

        private static object locker = new object();

        internal static bool RebuildRequired()
        {
            try
            {
                var stream = BuildManager.ReadCachedFile(BuildManagerTracker);

                using (var reader = new StreamReader(stream))
                {
                    var contents = reader.ReadToEnd();

                    //todo - properly validate the contents
                    if (!string.IsNullOrEmpty(contents))
                        return false;
                }
            }
            catch
            {
            }

            return true;
        }

        internal static void RebuildComplete(IEnumerable<Type> types)
        {
            try
            {
                var stream = BuildManager.CreateCachedFile(BuildManagerTracker);

                using (var writer = new StreamWriter(stream))
                {
                    writer.WriteLine("Build completed");
                }

                var location = IOHelper.MapPath(TypeCache);

                if (File.Exists(location))
                {
                    File.Delete(location);
                }

                StoreCache(location, cache.Concat(types.ToDictionary(t => t, CreateHash)));
            }
            catch
            {
            }
        }

        internal static void EnsureInitialized()
        {
            if (cache == null)
            {
                lock (locker)
                {
                    if (cache == null)
                    {
                        var location = IOHelper.MapPath(TypeCache);

                        if (!File.Exists(location))
                        {
                            var types = TypeFinder.FindClassesOfType<DocumentTypeBase>();
                            cache = new Dictionary<Type, string>();
                            StoreCache(location, types.ToDictionary(t => t, CreateHash));
                        }
                        else
                        {
                            var readTypes = ReadTypesFromCache(location);
                            cache = readTypes;
                        }
                    }
                }
            }
        }

        private static void StoreCache(string location, IEnumerable<KeyValuePair<Type, string>> types)
        {
            var xml = types
                .GroupBy(x => x.Key.Module)
                .GroupBy(x => x.Key.Assembly)
                .Select(x =>
                    new XElement("assembly",
                        new XAttribute("name", x.Key.FullName),
                        x.Select(module =>
                            new XElement("module",
                                new XAttribute("versionId", module.Key.ModuleVersionId),
                                module.Select(type =>
                                    new XElement("type",
                                        new XElement("name", type.Key.FullName),
                                        new XElement("hash", type.Value)
                                    )
                                )
                            )
                        )
                    )
                );

            new XElement("typeCache", xml).Save(location);
        }

        private static Dictionary<Type, string> ReadTypesFromCache(string location)
        {
            var xml = XDocument.Load(location);
            var assemblies = xml.Descendants("assembly");
            var cache = new Dictionary<Type, string>();

            foreach (var assembly in assemblies)
            {
                var asm = Assembly.Load(assembly.Attribute("name").Value);
                foreach (var module in assembly.Descendants("module"))
                {
                    var moduleVersionId = new Guid(module.Attribute("versionId").Value);
                    foreach (var type in module.Descendants("type"))
                    {
                        var typeName = type.Descendants("name").First().Value;
                        var typeHash = type.Descendants("hash").First().Value;

                        var t = asm.GetType(typeName);

                        if (t != null && t.Module.ModuleVersionId == moduleVersionId && CreateHash(t) == typeHash)
                            cache.Add(t, typeHash);
                    }
                }
            }

            return cache;
        }

        private static string CreateHash(Type t)
        {
            var properties = t.GetProperties(BindingFlags.Public | BindingFlags.Instance).Select(p => p.Name);
            var hasher = MD5.Create();

            var id = t.FullName + "-" + properties.Aggregate((x, y) => x += "-" + y);

            var bytes = System.Text.Encoding.ASCII.GetBytes(id);
            var hash = hasher.ComputeHash(bytes);

            return string.Join(string.Empty, hash.Select(x => x.ToString("X2")));
        }

        internal static IEnumerable<Type> GetTypesToUpdate()
        {
            var allTypes = TypeFinder.FindClassesOfType<DocumentTypeBase>();

            return allTypes.Where(x => !cache.Any(c => c.Key == x));
        }
    }
}
