using System;

namespace SpatialConnect.Wwise.Core
{
    public class MaxDistanceSliderPresenter : IDisposable
    {
        private readonly ISliderBehaviour sliderBehaviour_;
        private readonly IAttenuationOption attenuationOption_;

        public MaxDistanceSliderPresenter(ISliderBehaviour sliderBehaviour, IPositioningPropertySet positioningPropertySet)
        {
            sliderBehaviour_ = sliderBehaviour;
            attenuationOption_ = positioningPropertySet.AttenuationOption;

            sliderBehaviour_.SliderValueChanged += OnSliderValueChanged;
            attenuationOption_.MaxDistanceChanged += OnMaxDistanceChanged;

            if (!positioningPropertySet.IsEditable)
            {
                sliderBehaviour_.DisableSlider();
                return;
            }

            OnMaxDistanceChanged(attenuationOption_.MaxDistance);
            sliderBehaviour_.EnableSlider();
        }

        private void OnMaxDistanceChanged(uint maxDistance)
        {
            sliderBehaviour_.Value = maxDistance;
        }

        private void OnSliderValueChanged(float value)
        {
            attenuationOption_.MaxDistance = (uint) value;
        }

        public void Dispose()
        {
            sliderBehaviour_.SliderValueChanged -= OnSliderValueChanged;
            attenuationOption_.MaxDistanceChanged -= OnMaxDistanceChanged;
        }
    }
}
