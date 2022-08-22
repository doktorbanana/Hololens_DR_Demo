using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;

namespace SpatialConnect.Wwise.Core
{
    public interface IEventManager
    {
        IToggleable PauseToggle { get; }
        
        IEnumerable<IEvent> EventList { get; }

        void ClearSelection();
        
        event Action<IEvent> EventPosted;
        event Action<IEvent> EventEnded;
        event Action<IEvent> EventRemoved;

        event Action<IEvent> EventSelected;

        event Action<IAudioObject, IEvent> AudioObjectSelected;

        event Action<IEnumerable<IEvent>> EventListUpdated;
    }
}
