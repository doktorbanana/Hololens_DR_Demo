using System.Collections.Generic;

namespace SpatialConnect.Wwise.Core
{
    public interface IEventSphereHandlerBehaviour : IShowable
    {
        void UpdateSpheres(IEnumerable<IEvent> events);
    }
}
