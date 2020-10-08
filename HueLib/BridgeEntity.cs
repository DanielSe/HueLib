namespace HueLib
{
    public abstract class BridgeEntity : HueEntity
    {
        protected readonly Bridge _bridge;

        public abstract string EntityUri { get; }
        public string Id { get; }

        protected BridgeEntity(Bridge bridge, string id)
        {
            _bridge = bridge;
            Id = id;
        }
    }
}