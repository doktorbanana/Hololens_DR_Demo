using System;

namespace SpatialConnect.Wwise.Core
{
    public class EventInspectorHandlerPresenter : IDisposable
    {
        private readonly IEventInspectorHandlerBehaviour eventInspectorHandlerBehaviour_;
        private readonly IEventManager eventManager_;

        public EventInspectorHandlerPresenter(IEventInspectorHandlerBehaviour eventInspectorHandlerBehaviour, IEventManager eventManager)
        {
            eventInspectorHandlerBehaviour_ = eventInspectorHandlerBehaviour;
            eventManager_ = eventManager;
            eventManager_.EventSelected += OnEventSelected;
        }
        
        public void Dispose()
        {
            eventManager_.EventSelected -= OnEventSelected;
        }
        
        private void OnEventSelected(IEvent @event)
        {
            eventInspectorHandlerBehaviour_.UpdateList(@event);
        }
    }
}
