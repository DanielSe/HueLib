using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HueLib
{
    public class BridgeFactory
    {
        public Bridge Connect(string host, string token)
        {
            return new Bridge(host).Authenticate(token);
        }

        public Bridge Connect(string host)
        {
            return new Bridge(host);
        }

        public async Task<IEnumerable<Bridge>> DiscoverAsync()
        {
            var json = await HttpClientProvider.Client.GetJsonAsync("https://discovery.meethue.com/");
            return json.RootElement
                .EnumerateArray()
                .Select(x => new Bridge(x.GetProperty("internalipaddress").GetString()));
        }
    }
}