namespace SpatialConnect.Wwise.Core.Wwu
{
    public class MixingSessionPropertySet
    {
        public MixingSessionPropertySet(string id, string name, string[] audioObjectIds)
        {
            Id = id;
            Name = name;
            AudioObjectIds = audioObjectIds;
        }
        
        public readonly string Id;
        public readonly string Name;
        public readonly string[] AudioObjectIds;
    }
}
