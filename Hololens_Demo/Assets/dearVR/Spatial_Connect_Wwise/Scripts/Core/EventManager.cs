using System;
using System.Collections.Generic;
using System.Linq;

namespace SpatialConnect.Wwise.Core
{
    public class EventManager : IEventManager
    {
        private readonly IFactory factory_;
        private readonly IMessageBroadcaster messageBroadcaster_;
        private IEvent selectedEvent_;
        private IAudioObject selectedAudioObject_;
        private IPausableList<IEvent> eventList_;

        public IToggleable PauseToggle => eventList_;
        public IEnumerable<IEvent> EventList => eventList_;
        

        public EventManager(ICaptureLog captureLog, IMessageBroadcaster messageBroadcaster, IFactory factory)
        {
            factory_ = factory;
            messageBroadcaster_ = messageBroadcaster;
            eventList_ = factory.CreatePausableEventList();

            eventList_.StateChanged += OnEventListStateChanged;
            captureLog.EventTriggered += OnEventTriggered;
            captureLog.EventFinished += OnEventFinished;
        }

        private void OnEventTriggered(EventPropertySet eventPropertySet)
        {
            var @event = factory_.CreateEvent(eventPropertySet);
            @event.EventSelectionStateChanged += OnEventSelectionStateChanged;
            eventList_.AddToTop(@event);

            EventPosted?.Invoke(@event);
            EventListUpdated?.Invoke(EventList);
        }

        private void OnEventFinished(uint playingId)
        {
            var finishedEvent = eventList_.Find(@event => @event.PlayingId == playingId);
            if (finishedEvent == null)
                return;

            EventEnded?.Invoke(finishedEvent);
            finishedEvent.GhostingEnded += OnEventGhostingStateEnded;
            finishedEvent.SetToGhostState();
        }

        private void OnEventSelectionStateChanged(bool state, IEvent @event)
        {
            if (!state) return;
            if (@event == selectedEvent_) return;

            if (selectedEvent_ != null)
                selectedEvent_.AudioObjectSelectionChanged -= OnAudioObjectSelectionChanged;

            selectedEvent_ = @event;
            selectedEvent_.AudioObjectSelectionChanged += OnAudioObjectSelectionChanged;
            EventSelected?.Invoke(@event);

            foreach (var listedEvent in EventList.Where(listedEvent => listedEvent != @event))
                listedEvent.Deselect();
        }

        private void OnEventGhostingStateEnded(IEvent @event)
        {
            eventList_.Remove(@event);
            EventRemoved?.Invoke(@event);
            EventListUpdated?.Invoke(EventList);
        }

        private void OnAudioObjectSelectionChanged(IAudioObjectPropertySet audioObjectPropertySet, int[] index)
        {
            if (audioObjectPropertySet == null)
                return;

            selectedAudioObject_?.Dispose();
            selectedAudioObject_ = factory_.CreateEventAudioObject(audioObjectPropertySet.Id, messageBroadcaster_);
            AudioObjectSelected?.Invoke(selectedAudioObject_, selectedEvent_);
        }

        private void OnEventListStateChanged(bool state)
        {
            EventListUpdated?.Invoke(EventList);
        }

        public void ClearSelection()
        {
            if (selectedEvent_ == null)
                return;

            selectedEvent_.Deselect();
            selectedEvent_.AudioObjectSelection = new int[0];
        }

        public event Action<IEvent> EventPosted;
        public event Action<IEvent> EventEnded;
        public event Action<IEvent> EventRemoved;
        
        public event Action<IEvent> EventSelected;
        public event Action<IEnumerable<IEvent>> EventListUpdated;
        public event Action<IAudioObject, IEvent> AudioObjectSelected;
    }
}
