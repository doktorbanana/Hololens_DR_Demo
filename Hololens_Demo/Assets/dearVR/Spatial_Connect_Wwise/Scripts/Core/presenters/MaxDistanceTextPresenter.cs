using System;

namespace SpatialConnect.Wwise.Core
{
    public class MaxDistanceTextPresenter : IDisposable
    {
        private readonly ITextBehaviour textBehaviour_;
        private readonly IAttenuationOption attenuationOption_;
        
        public MaxDistanceTextPresenter(ITextBehaviour textBehaviour, IPositioningPropertySet positioningPropertySet)
        {
            textBehaviour_ = textBehaviour;
            attenuationOption_ = positioningPropertySet.AttenuationOption;

            attenuationOption_.MaxDistanceChanged += OnMaxDistanceChanged;

            if (positioningPropertySet.IsEditable)
                OnMaxDistanceChanged(attenuationOption_.MaxDistance);
            else
                textBehaviour_.Text = "N/A";
        }

        private void OnMaxDistanceChanged(uint maxDistance)
        {
            textBehaviour_.Text = maxDistance.ToString();
        }

        public void Dispose()
        {
            attenuationOption_.MaxDistanceChanged -= OnMaxDistanceChanged;
        }
    }
}
