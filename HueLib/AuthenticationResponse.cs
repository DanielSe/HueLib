using System.Text.Json.Serialization;

namespace HueLib
{
    public class AuthenticationResponse
    {
        public SuccessResponse Success { get; set; }
        public ErrorResponse Error { get; set; }

        public class SuccessResponse
        {
            [JsonPropertyName("username")]
            public string Username { get; set; }
        }

        public class ErrorResponse
        {
            [JsonPropertyName("type")]
            public uint Type { get; set; }
            [JsonPropertyName("address")]
            public string Address { get; set; }
            [JsonPropertyName("description")]
            public string Description { get; set; }
        }
    }
}