using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HueLib.Converters;
using HueLib.Groups;
using HueLib.Lights;

namespace HueLib
{
    public class Bridge
    {
        private readonly string _host;
        private string _token;
        private bool _authenticated = true;
        
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _options;
        
        public Bridge(string host)
        {
            _host = host;
            
            _httpClient = HttpClientProvider.Client;

            _options = new JsonSerializerOptions();
            _options.IgnoreNullValues = true;
            _options.PropertyNamingPolicy = new HueNamingPolicy();
            _options.Converters.Add(new DictionaryToArrayConverter());
            _options.Converters.Add(new ObjectConstructorConverter(this));
        }

        private string BaseUrl => $"https://{_host}/api{(_authenticated ? $"/{_token}" : "")}";

        public async Task<string> Authenticate(string applicationName, string deviceName)
        {
            var json = JsonSerializer.Serialize(new {devicetype = $"{applicationName}#{deviceName}"}, _options);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await _httpClient.PostAsync($"{BaseUrl}", content);
            result.EnsureSuccessStatusCode();
            var response = JsonSerializer.Deserialize<AuthenticationResponse[]>(await result.Content.ReadAsStringAsync()).FirstOrDefault();
            
            if (response == null)
                throw new Exception("Invalid response");
            
            if (response.Error != null)
                throw new Exception($"{response.Error.Type}: {response.Error.Description}");

            return _token = response.Success.Username;
        }

        public Bridge Authenticate(string token)
        {
            _token = token;
            _authenticated = true;
            return this;
        }
        
        private IEnumerable<Light> _lights;
        public async Task<IEnumerable<Light>> Lights()
        {
            if (!_authenticated)
                throw new InvalidOperationException("Unauthenticated");
            
            if (_lights != null)
                return _lights;
            
            var result = await _httpClient.GetStringAsync($"{BaseUrl}/{Light.ResourceUri}");
            var lights = JsonSerializer.Deserialize<Light[]>(result, _options);
            
            return _lights = lights.ToArray();
        }

        private IEnumerable<Group> _groups;
        
        public async Task<IEnumerable<Group>> Groups()
        {
            if (!_authenticated)
                throw new InvalidOperationException("Unauthenticated");

            if (_groups != null)
                return _groups;
            
            var result = await _httpClient.GetStringAsync($"{BaseUrl}/{Group.ResourceUri}");
            var groups = JsonSerializer.Deserialize<Group[]>(result, _options);
            
            return _groups = groups.ToArray();
        }
        
        public Group AllLights => new Group(this, "0", "All lights", "all", null, new string[0]);

        public async Task Update(BridgeEntity entity, string property, object payload)
        {
            if (!_authenticated)
                throw new InvalidOperationException("Unauthenticated");

            var json = JsonSerializer.Serialize(payload, _options);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{BaseUrl}/{entity.EntityUri}/{property}", content);
        }

        private class HueNamingPolicy : JsonNamingPolicy
        {
            public override string ConvertName(string name)
            {
                return name.ToLowerInvariant();
            }
        }
    }
}