using System;
using System.Collections.Generic;
using System.Linq;

namespace SpatialConnect.Wwise.Core
{
    public class EventSphereHandlerPresenter : IDisposable
    {
        private readonly IEventSphereHandlerBehaviour eventSphereHandlerBehaviour_;
        private readonly IEventManager eventManager_;
        private readonly IEventFilter eventFilter_;
        private readonly IToggleable filterEventSpheresToggleable_;

        public EventSphereHandlerPresenter(IEventSphereHandlerBehaviour eventSphereHandlerBehaviour, IEventManager eventManager,
            IEventFilter eventFilter, IToggleable filterEventSpheresToggleable)
        {
            eventSphereHandlerBehaviour_ = eventSphereHandlerBehaviour;
            eventManager_ = eventManager;
            eventFilter_ = eventFilter;
            filterEventSpheresToggleable_ = filterEventSpheresToggleable;

            eventManager_.EventListUpdated += OnEventListUpdated;
            eventFilter_.FilterUpdated += OnFilterUpdated;
        }

        private void OnEventListUpdated(IEnumerable<IEvent> events)
        {
            var permanentFilterList = eventFilter_.PermanentFilterList(events);
            var toggleableFilterList = eventFilter_.ToggleableFilterList(permanentFilterList);
            
            eventSphereHandlerBehaviour_.UpdateSpheres(filterEventSpheresToggleable_.State ? toggleableFilterList : permanentFilterList);
        }

        private void OnFilterUpdated()
        {
            OnEventListUpdated(eventManager_.EventList);
        }

        public void Dispose()
        {
            eventManager_.EventListUpdated -= OnEventListUpdated;
            eventFilter_.FilterUpdated -= OnFilterUpdated;
        }
    }
}
