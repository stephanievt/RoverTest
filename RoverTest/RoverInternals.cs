using System.Reflection;
using System.Text.Json;

namespace RoverTest
{
    public class RoverInternals
    {
        public static List<T> DeserializeJsonArray<T>(string json)
        {
            return JsonSerializer.Deserialize<List<T>>(json);

        }

        public static T GetTypedObject<T>(object untypedObject)
        {
            if (untypedObject is T typed)
                return typed;

            throw new InvalidCastException(
                $"Object is of type '{untypedObject?.GetType().FullName ?? "null"}' " +
                $"but was expected to be '{typeof(T).FullName}'.");
        }

        public static IEnumerable<Type> GetDerivedClasses<T>() where T : class
        {
            var baseType = typeof(T);
            // Force load all assemblies from the application directory
            LoadAllAssemblies();

            return AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(assembly =>
                {
                    try
                    {
                        return assembly.GetTypes();
                    }
                    catch (ReflectionTypeLoadException ex)
                    {
                        // Handle partially loadable assemblies safely
                        return ex.Types.Where(t => t != null)!;
                    }
                })
                .Where(type =>
                    type is not null &&
                    type.IsClass &&
                    !type.IsAbstract &&
                    baseType.IsAssignableFrom(type) &&
                    type != baseType);

        }

        private static void LoadAllAssemblies()
        {
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            var loadedPaths = loadedAssemblies.Where(a => !a.IsDynamic).Select(a => a.Location).ToHashSet();

            var referencedPaths = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll");

            foreach (var path in referencedPaths)
            {
                if (!loadedPaths.Contains(path))
                {
                    try
                    {
                        Assembly.LoadFrom(path);
                    }
                    catch
                    {
                        // Ignore assemblies that can't be loaded (native, incompatible, etc.)
                    }
                }
            }

        }
    }
}
