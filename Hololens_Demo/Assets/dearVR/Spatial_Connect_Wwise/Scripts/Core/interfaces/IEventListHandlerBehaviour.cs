using System.Collections.Generic;

namespace SpatialConnect.Wwise.Core
{
    public interface IEventListHandlerBehaviour
    {
        void UpdateList(IEnumerable<IEvent> eventList);
    }
}
