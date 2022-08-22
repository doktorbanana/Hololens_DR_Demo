using System;
using System.Collections.Generic;
using System.Linq;

namespace SpatialConnect.Wwise.Core
{
    public class SourceObjectFollowerPresenter : IDisposable
    {
        private readonly ISourceObjectFollowerBehaviour sourceObjectFollowerBehaviour_;
        private readonly IEventManager eventManager_;
        private readonly Queue<IEvent> eventQueue_ = new Queue<IEvent>();
        private bool paused_;
        
        public SourceObjectFollowerPresenter(ISourceObjectFollowerBehaviour sourceObjectFollowerBehaviour,
            IEventManager eventManager)
        {
            sourceObjectFollowerBehaviour_ = sourceObjectFollowerBehaviour;
            eventManager_ = eventManager;

            eventManager_.EventPosted += OnEventPosted;
            eventManager_.EventRemoved += OnEventRemoved;
            eventManager_.PauseToggle.StateChanged += OnPauseStateChanged;
        }

        private void OnEventPosted(IEvent @event)
        {
            sourceObjectFollowerBehaviour_.Subscribe(@event);
        }

        private void OnEventRemoved(IEvent @event)
        {
            if (paused_)
                eventQueue_.Enqueue(@event);
            else
                sourceObjectFollowerBehaviour_.Unsubscribe(@event);
        }

        private void OnPauseStateChanged(bool state)
        {
            paused_ = state;
            if (paused_)
                return;
            
            foreach (var @event in eventQueue_)
                sourceObjectFollowerBehaviour_.Unsubscribe(@event);
            eventQueue_.Clear();
        }
        
        public void Dispose()
        {
            eventManager_.EventPosted -= OnEventPosted;
            eventManager_.EventRemoved -= OnEventRemoved;
            eventManager_.PauseToggle.StateChanged -= OnPauseStateChanged;

        }
    }
}
