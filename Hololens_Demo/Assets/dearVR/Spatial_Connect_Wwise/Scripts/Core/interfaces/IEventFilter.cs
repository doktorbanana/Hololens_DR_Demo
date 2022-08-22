using System;
using System.Collections.Generic;

namespace SpatialConnect.Wwise.Core
{
    public interface IEventFilter
    {
        IEnumerable<IEvent> ToggleableFilterList(IEnumerable<IEvent> eventList);
        IEnumerable<IEvent> PermanentFilterList(IEnumerable<IEvent> eventList);

        event Action FilterUpdated;
    }
}
