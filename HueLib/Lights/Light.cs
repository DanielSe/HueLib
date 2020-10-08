using System.Threading.Tasks;

namespace HueLib.Lights
{
    public class Light : BridgeEntity
    {
        public static string ResourceUri => $"lights";
        public override string EntityUri => $"{ResourceUri}/{Id}";

        public Light(Bridge bridge, string id, string type, string name, string modelId, string manufacturerName, string productName, string uniqueId, string swVersion, State state) : base(bridge, id)
        {
            Type = type;
            Name = name;
            ModelId = modelId;
            ManufacturerName = manufacturerName;
            ProductName = productName;
            UniqueId = uniqueId;
            SwVersion = swVersion;

            State = state;
        }

        public string Type { get; }
        public string Name { get; }
        public string ModelId { get; }
        public string ManufacturerName { get; }
        public string ProductName { get; }
        public string UniqueId { get; }
        public string SwVersion { get; }
        public State State { get; }
        
        
        public Task TurnOn()
        {
            return _bridge.Update(this, "state", new UpdateState {On = true});
        }

        public Task TurnOff()
        {
            return _bridge.Update(this, "state", new UpdateState {On = false});
        }
    }
}