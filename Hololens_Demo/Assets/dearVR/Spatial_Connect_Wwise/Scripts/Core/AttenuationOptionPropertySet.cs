namespace SpatialConnect.Wwise.Core
{
    public class AttenuationOptionPropertySet : IAttenuationOptionPropertySet
    {
        public string Id { get; }
        public string Name { get; }

        public AttenuationOptionPropertySet(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
