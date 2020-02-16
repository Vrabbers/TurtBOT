using System.Text.Json;
using System.Text.Json.Serialization;

namespace TurtBOT
{
    public struct BotConfig
    {

        [JsonPropertyName("prefix")]
        public string Prefix { get; set; }
        
        [JsonPropertyName("errorMessage")]
        public string ErrorMessage { get; set; }

    }
}