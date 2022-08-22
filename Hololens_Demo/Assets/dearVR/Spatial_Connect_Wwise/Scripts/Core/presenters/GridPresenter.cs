using System;

namespace SpatialConnect.Wwise.Core
{
    public class GridPresenter : IDisposable
    {
        private readonly IGridBehaviour gridBehaviour_;
        private readonly IAttenuationOption attenuationOption_;
        
        public GridPresenter(IGridBehaviour gridBehaviour, IPositioningPropertySet positioningPropertySet)
        {
            gridBehaviour_ = gridBehaviour;
            attenuationOption_ = positioningPropertySet.AttenuationOption;
            
            attenuationOption_.MaxDistanceChanged += OnMaxDistanceChanged;

            if (positioningPropertySet.IsEditable)
                OnMaxDistanceChanged(attenuationOption_.MaxDistance);
            else
                gridBehaviour_.MaxDistance = null;
        }

        private void OnMaxDistanceChanged(uint maxDistance)
        {
            gridBehaviour_.MaxDistance = maxDistance;
        }
        
        public void Dispose()
        {
            attenuationOption_.MaxDistanceChanged -= OnMaxDistanceChanged;
        }
    }
}
