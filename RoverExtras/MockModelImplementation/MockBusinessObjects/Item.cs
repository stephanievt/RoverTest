using System.Text.Json.Serialization;
using RoverTest.ModelApplicationData;

namespace RoverExtras.MockModelImplementation.MockBusinessObjects
{
    public  class Item : RoverDataObject
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("categoryId")]
        public int CategoryId { get; set; }

    }
}
