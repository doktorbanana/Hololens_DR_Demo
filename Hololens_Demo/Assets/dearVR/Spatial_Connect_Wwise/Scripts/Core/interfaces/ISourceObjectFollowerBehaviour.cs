namespace SpatialConnect.Wwise.Core
{
    public interface ISourceObjectFollowerBehaviour
    {
        void Subscribe(IEvent @event);

        void Unsubscribe(IEvent @event);
    }
}
