namespace SpatialConnect.Wwise.Core
{
    public interface IProjectPropertySet
    { 
        string Name { get; }
        
        string ProjectPath { get; }
        
        IAttenuationOptionPropertySet[] AttenuationOptionPropertySets { get; }
    }
}
