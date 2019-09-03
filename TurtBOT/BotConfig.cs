using Newtonsoft.Json;

namespace TurtBOT
{
    public struct BotConfig
    {
        [JsonProperty(Required = Required.Always)]
        public string Prefix;
        [JsonProperty(Required = Required.Always)]
        public string ErrorMessage;

    }
}