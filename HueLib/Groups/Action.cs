using System.Text.Json.Serialization;

namespace HueLib.Groups
{
    public class Action : HueEntity
    {
        [JsonPropertyName("on")]
        public bool On { get; set; }
        [JsonPropertyName("bri")]
        public ushort Brightness { get; set; }
        [JsonPropertyName("hue")]
        public uint Hue { get; set; }
        [JsonPropertyName("sat")]
        public ushort Saturation { get; set; }
        [JsonPropertyName("effect")]
        public string Effect { get; set; }
        [JsonPropertyName("xy")]
        public float[] XY { get; set; }
        [JsonPropertyName("ct")]
        public uint ColorTemperature { get; set; }
        [JsonPropertyName("alert")]
        public string Alert { get; set; }
        [JsonPropertyName("colormode")]
        public string ColorMode { get; set; }
    }
}