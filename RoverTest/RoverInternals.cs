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


    }
}
