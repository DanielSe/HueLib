using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace HueLib
{
    public static class HttpClientProvider
    {
        public static readonly HttpClient Client = new HttpClient(new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (m, c, a, b) => true
        });

        public static async Task<JsonDocument> GetJsonAsync(this HttpClient client, string url)
        {
            return JsonDocument.Parse(await client.GetByteArrayAsync(url));
        }
    }
}