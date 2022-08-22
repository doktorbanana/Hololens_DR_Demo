using System;

namespace SpatialConnect.Wwise.Core
{
    public class SliderPresenter : IDisposable
    {
        private readonly IValue value_;
        private readonly ISliderBehaviour sliderBehaviour_;
        
        public SliderPresenter(ISliderBehaviour sliderBehaviour, IValue value)
        {
            sliderBehaviour_ = sliderBehaviour;
            value_ = value;
            
            sliderBehaviour_.SliderValueChanged += OnSliderValueChanged;
            value_.ValueChanged += OnValueChanged;
            
            OnValueChanged(value_.Value);
        }

        private void OnValueChanged(float value)
        {
            sliderBehaviour_.Value = value;
        }

        private void OnSliderValueChanged(float value)
        {
            value_.Value = value;
        }
        
        public void Dispose()
        {
            sliderBehaviour_.SliderValueChanged -= OnSliderValueChanged;
            value_.ValueChanged -= OnValueChanged;
        }
    }
}
