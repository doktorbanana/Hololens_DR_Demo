namespace SpatialConnect.Wwise.Core
{
    public interface IInspectorBehaviour
    {
        void UpdateInspector(IAudioObject audioObject, IProjectPropertySet projectPropertySet, IEvent @event);
    }
}
