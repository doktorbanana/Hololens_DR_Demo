namespace SpatialConnect.Wwise.Core
{
    public interface IEventRepresentation : IGhostable, IOutlinable, ISelectable, IDeactivatable
    {
        uint PlayingId { get; }
    }
}
