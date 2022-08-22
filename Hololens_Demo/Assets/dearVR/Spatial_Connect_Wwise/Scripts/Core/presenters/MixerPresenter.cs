using System;

namespace SpatialConnect.Wwise.Core
{
    public class MixerPresenter : IDisposable
    {
        private readonly IMixingSessionManager mixingSessionManager_;
        private readonly IMixerBehaviour mixerBehaviour_;
        
        private const int MIN_CHANNEL_COUNT = 4;
        private const int MAX_CHANNEL_COUNT = 8;
        
        public MixerPresenter(IMixerBehaviour mixerBehaviour, IMixingSessionManager mixingSessionManager)
        {
            mixerBehaviour_ = mixerBehaviour;
            mixingSessionManager_ = mixingSessionManager;

            mixingSessionManager_.MixingSessionSelectionChanged += OnMixingSessionSelectionChanged;
        }

        private void OnMixingSessionSelectionChanged(int index, IMixingSession mixingSession)
        {
            var channelCount = mixingSession.AudioObjects.Length;
            channelCount = channelCount < MIN_CHANNEL_COUNT ? MIN_CHANNEL_COUNT : channelCount;
            channelCount = channelCount > MAX_CHANNEL_COUNT ? MAX_CHANNEL_COUNT : channelCount;
            mixerBehaviour_.Resize(channelCount);
        }
        
        public void Dispose()
        {
            mixingSessionManager_.MixingSessionSelectionChanged -= OnMixingSessionSelectionChanged;
        }
    }
}
