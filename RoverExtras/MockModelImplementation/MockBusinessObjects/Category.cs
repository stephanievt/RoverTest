using System.Text.Json.Serialization;
using RoverTest.ModelApplicationData;

namespace RoverExtras.MockModelImplementation.MockBusinessObjects
{
    public class Category : RoverDataObject
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
