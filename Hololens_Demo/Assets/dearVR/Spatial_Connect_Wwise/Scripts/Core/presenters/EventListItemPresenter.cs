using System;

namespace SpatialConnect.Wwise.Core
{
    public class EventListItemPresenter : IDisposable
    {
        private readonly IEventListItemBehaviour eventListItemBehaviour_;
        private readonly IStringShortenerToggle stringShortenerToggle_;
        private readonly IEvent event_;
        
        public EventListItemPresenter(IEventListItemBehaviour eventListItemBehaviour, IStringShortenerToggle stringShortenerToggle, IEvent @event)
        {
            eventListItemBehaviour_ = eventListItemBehaviour;
            stringShortenerToggle_ = stringShortenerToggle;
            event_ = @event;

            eventListItemBehaviour_.Selected += OnSelected;
            stringShortenerToggle_.StateChanged += OnStateChanged;
            event_.EventEnded += OnEventEnded;
            event_.EventSelectionStateChanged += OnEventSelectionStateChanged;
            
            if(event_.Ghosting)
                OnEventEnded();
            eventListItemBehaviour_.OutlineActive = event_.Selected;
            UpdateText();
        }   
        
        private void OnSelected()
        {
            event_.Select();
        }

        private void OnStateChanged(bool state)
        {
            UpdateText();
        }

        private void OnEventEnded()
        {
            eventListItemBehaviour_.TurnIntoGhost();
        }

        private void OnEventSelectionStateChanged(bool state, IEvent unused)
        {
            eventListItemBehaviour_.OutlineActive = state;
        }

        private void UpdateText()
        {
            eventListItemBehaviour_.EventName = stringShortenerToggle_.Process(event_.Name);
            eventListItemBehaviour_.GameObjectName = stringShortenerToggle_.Process(event_.GameObjectName);
        }
        
        public void Dispose()
        {
            eventListItemBehaviour_.Selected -= OnSelected;
            stringShortenerToggle_.StateChanged -= OnStateChanged;
            event_.EventEnded -= OnEventEnded;
            event_.EventSelectionStateChanged -= OnEventSelectionStateChanged;
        }
    }
}
