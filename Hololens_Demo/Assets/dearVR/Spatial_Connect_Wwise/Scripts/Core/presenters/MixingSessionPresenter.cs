using System;

namespace SpatialConnect.Wwise.Core
{
    public class MixingSessionPresenter : IDisposable
    {
        private readonly IMixingSessionBehaviour mixingSessionBehaviour_;
        private readonly ISliderBehaviour sliderBehaviour_;
        private readonly IMixingSession mixingSession_;
        
        public MixingSessionPresenter(IMixingSessionBehaviour mixingSessionBehaviour, 
            ISliderBehaviour sliderBehaviour, IMixingSession mixingSession)
        {
            mixingSessionBehaviour_ = mixingSessionBehaviour;
            sliderBehaviour_ = sliderBehaviour;
            mixingSession_ = mixingSession;

            mixingSessionBehaviour_.ChannelStripSelectionChanged += OnChannelStripSelectionChanged;
            sliderBehaviour_.SliderValueChanged += OnSliderValueChanged;
            mixingSession_.AudioObjectSelectionChanged += OnAudioObjectSelectionChanged;

            sliderBehaviour_.MaxValue = mixingSession.AudioObjects.Length - 1;
            if(mixingSession_.AudioObjects.Length <= 1)
                sliderBehaviour_.HideSlider();

            OnSliderValueChanged(0);
        }

        private void OnChannelStripSelectionChanged(int index)
        {
            mixingSession_.AudioObjectSelection = index;
        }
        
        private void OnSliderValueChanged(float value)
        {
            var offset = (int) value;
            sliderBehaviour_.Value = offset;
            mixingSessionBehaviour_.ScrollTo(offset);
        }

        private void OnAudioObjectSelectionChanged(int? index, IAudioObject audioObject)
        {
            mixingSessionBehaviour_.ChannelStripSelection = index;
        }
        
        public void Dispose()
        {
            mixingSessionBehaviour_.ChannelStripSelectionChanged -= OnChannelStripSelectionChanged;
            sliderBehaviour_.SliderValueChanged -= OnSliderValueChanged;
            mixingSession_.AudioObjectSelectionChanged -= OnAudioObjectSelectionChanged;
        }
    }
}
