using System;
using System.Collections.Generic;
using System.Linq;

namespace SpatialConnect.Wwise.Core
{
    public class EventListHandlerPresenter : IDisposable
    {
        private readonly IEventListHandlerBehaviour eventListHandlerBehaviour_;
        private readonly IEventManager eventManager_;
        private readonly IEventFilter eventFilter_;

        private const int MAX_EVENT_COUNT = 25;

        public EventListHandlerPresenter(IEventListHandlerBehaviour eventListHandlerBehaviour, IEventManager eventManager, IEventFilter eventFilter)
        {
            eventListHandlerBehaviour_ = eventListHandlerBehaviour;
            eventManager_ = eventManager;
            eventFilter_ = eventFilter;

            eventManager_.EventListUpdated += OnEventListUpdated;
            eventFilter_.FilterUpdated += OnFilterUpdated;
        }

        private void OnEventListUpdated(IEnumerable<IEvent> events)
        {
            var eventSequence = eventFilter_.ToggleableFilterList(events).Take(MAX_EVENT_COUNT);
            eventListHandlerBehaviour_.UpdateList(eventSequence);
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
