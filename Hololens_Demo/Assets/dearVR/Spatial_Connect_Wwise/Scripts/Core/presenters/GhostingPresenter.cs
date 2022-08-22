using System;

namespace SpatialConnect.Wwise.Core
{
    public class GhostingPresenter : IDisposable
    {
        private readonly IDelayedExecutionBehaviour delayedExecutionBehaviour_;
        private readonly IEventManager eventManager_;
        private readonly float ghostingDuration_;

        public GhostingPresenter(IDelayedExecutionBehaviour delayedExecutionBehaviour, IEventManager eventManager, float ghostingDuration)
        {
            delayedExecutionBehaviour_ = delayedExecutionBehaviour;
            eventManager_ = eventManager;
            ghostingDuration_ = ghostingDuration;

            eventManager_.EventEnded += OnEventEnded;
        }

        public void Dispose()
        {
            eventManager_.EventEnded -= OnEventEnded;
        }
        
        private void OnEventEnded(IEvent @event)
        {
            delayedExecutionBehaviour_.ExecuteWithDelay(@event.Dispose, ghostingDuration_);
        }
    }
}
