using System;

namespace SpatialConnect.Wwise.Core
{
    public class EventSpherePresenter : IDisposable
    {
        private readonly IEventSphereBehaviour eventSphereBehaviour_;
        private readonly IEvent event_;
        
        public EventSpherePresenter(IEventSphereBehaviour eventSphereBehaviour, IEvent @event)
        {
            eventSphereBehaviour_ = eventSphereBehaviour;
            event_ = @event;

            event_.EventSelectionStateChanged += OnEventSelectionStateChanged;
            event_.EventEnded += OnEventEnded;
            event_.SourceMoved += OnSourceMoved;
            eventSphereBehaviour_.Selected += OnSelected;
            
            if(event_.Ghosting)
                OnEventEnded();
            eventSphereBehaviour_.OutlineActive = event_.Selected;
        }

        private void OnSelected(object unused)
        {
            event_.Select();
        }

        private void OnEventSelectionStateChanged(bool state, IEvent unused)
        {
            eventSphereBehaviour_.OutlineActive = state;
        }

        private void OnEventEnded()
        {
            eventSphereBehaviour_.TurnIntoGhost();
        }

        private void OnSourceMoved(Position position)
        {
            eventSphereBehaviour_.Position = position;
        }

        public void Dispose()
        {
            event_.EventSelectionStateChanged -= OnEventSelectionStateChanged;
            event_.EventEnded -= OnEventEnded;
            event_.SourceMoved -= OnSourceMoved;
            eventSphereBehaviour_.Selected -= OnSelected;
        }
    }
}
