using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HueLib.Lights;

namespace HueLib.Groups
{
    public class Group : BridgeEntity
    {
        public static string ResourceUri => $"groups";
        public override string EntityUri => $"{ResourceUri}/{Id}";

        private readonly string[] _lights;

        public Group(Bridge bridge, string id, string name, string type, Action action, string[] lights) : base(bridge, id)
        {
            Name = name;
            Type = type;
            Action = action;
            _lights = lights;
        }

        public string Name { get; }
        public string Type { get; }
        public Action Action { get; }

        public async Task<IEnumerable<Light>> Lights()
        {
            return (await _bridge.Lights()).Where(l => _lights.Contains(l.Id));
        }

        public Task TurnOn()
        {
            return _bridge.Update(this, "action", new UpdateAction {On = true});
        }

        public Task TurnOff()
        {
            return _bridge.Update(this, "action", new UpdateAction {On = false});
        }
    }
}