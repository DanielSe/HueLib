using System.Text.Json.Serialization;

namespace HueLib.Groups
{
    public class UpdateAction
    {
        [JsonPropertyName("on")]
        public bool? On { get; set; }
        [JsonPropertyName("bri")]
        public ushort? Brightness { get; set; }
        [JsonPropertyName("hue")]
        public uint? Hue { get; set; }
        [JsonPropertyName("sat")]
        public ushort? Saturation { get; set; }
    }
}