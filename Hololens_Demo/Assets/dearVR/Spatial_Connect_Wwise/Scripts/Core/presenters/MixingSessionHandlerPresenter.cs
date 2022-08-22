using System;

namespace SpatialConnect.Wwise.Core
{
    public class MixingSessionHandlerPresenter : IDisposable
    {
        private readonly IMixingSessionHandlerBehaviour mixingSessionHandlerBehaviour_;
        private readonly IMixingSessionManager mixingSessionManager_;

        public MixingSessionHandlerPresenter(IMixingSessionHandlerBehaviour mixingSessionHandlerBehaviour, IMixingSessionManager mixingSessionManager)
        {
            mixingSessionHandlerBehaviour_ = mixingSessionHandlerBehaviour;
            mixingSessionManager_ = mixingSessionManager;
            mixingSessionManager_.MixingSessionSelectionChanged += OnMixingSessionSelectionChanged;
        }

        private void OnMixingSessionSelectionChanged(int index, IMixingSession mixingSession)
        {
            mixingSessionHandlerBehaviour_.UpdateMixingSession(mixingSession);
        }

        public void Dispose()
        {
            mixingSessionManager_.MixingSessionSelectionChanged -= OnMixingSessionSelectionChanged;
        }
    }
}
