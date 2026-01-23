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
            // Get the assembly containing the base abstract class T
            // You can also use AppDomain.CurrentDomain.GetAssemblies() to check all loaded assemblies
            Assembly assembly = Assembly.GetAssembly(typeof(T));

            if (assembly == null)
            {
                return [];
            }

            // Filter the types in the assembly
            var derivedTypes = assembly.GetTypes()
                .Where(type => type.IsClass && // Ensure it is a class
                               !type.IsAbstract && // Ensure it is not abstract itself
                               type.IsSubclassOf(typeof(T))); // Ensure it inherits from T

            return derivedTypes;
        }
    }
}
