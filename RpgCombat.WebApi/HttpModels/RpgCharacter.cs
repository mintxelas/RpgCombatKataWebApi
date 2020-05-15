using System.Text.Json.Serialization;

namespace RpgCombat.WebApi.HttpModels
{
    public class RpgCharacter
    {
        [JsonPropertyName("health")]
        public int Health { get; set; }

        [JsonPropertyName("level")]
        public int Level { get; set; }
    }
}