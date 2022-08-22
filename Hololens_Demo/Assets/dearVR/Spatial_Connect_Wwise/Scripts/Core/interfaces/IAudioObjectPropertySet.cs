namespace SpatialConnect.Wwise.Core
{
    public interface IAudioObjectPropertySet
    {
        string Id { get; }
        
        string Name { get; }

        IAudioObjectPropertySet[] Children { get; }
        
        int ChildCount { get; }
    }
}
