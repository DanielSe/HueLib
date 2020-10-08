namespace HueLib.Lights
{
    public class State : HueEntity
    {
        public bool On { get; }
        public ushort Brightness { get; }
        public uint Hue { get; }
        public ushort Saturation { get; }
        public string Effect { get; }
        public float[] XY { get; }
        public uint ColorTemperature { get; }
        public string Alert { get; }
        public string ColorMode { get; }
        public string Mode { get; }
        public bool Reachable { get; }

        public State(bool @on, ushort bri, uint hue, ushort sat, string effect, float[] xy, uint ct, string alert, string colorMode, string mode, bool reachable)
        {
            On = @on;
            Brightness = bri;
            Hue = hue;
            Saturation = sat;
            Effect = effect;
            XY = xy;
            ColorTemperature = ct;
            Alert = alert;
            ColorMode = colorMode;
            Mode = mode;
            Reachable = reachable;
        }
    }
}