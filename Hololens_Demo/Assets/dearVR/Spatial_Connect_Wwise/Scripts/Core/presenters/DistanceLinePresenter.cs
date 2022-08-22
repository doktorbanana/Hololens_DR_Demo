using System;

namespace SpatialConnect.Wwise.Core
{
    public class DistanceLinePresenter : IDisposable
    {
        private const float EDITOR_WIDTH = 900f;
        private const int MAX_TEXT_LENGTH = 26;
        
        private readonly IDistanceLineBehaviour distanceLineBehaviour_;
        private readonly IAttenuationOption attenuationOption_;
        private readonly IStringShortener stringShortener_;
        private readonly IEvent event_;
        private float maxDistance_;
        private float distance_;

        public DistanceLinePresenter(IDistanceLineBehaviour distanceLineBehaviour, IStringShortener stringShortener, IPositioningPropertySet positioningPropertySet, IEvent @event)
        {
            distanceLineBehaviour_ = distanceLineBehaviour;
            stringShortener_ = stringShortener;
            attenuationOption_ = positioningPropertySet.AttenuationOption;
            event_ = @event;

            if (positioningPropertySet.IsEditable && event_ != null)
            {
                distanceLineBehaviour_.DistanceChanged += OnDistanceChanged;
                attenuationOption_.MaxDistanceChanged += OnMaxDistanceChanged;
                event_.EventEnded += OnEventEnded;

                distanceLineBehaviour_.Enable();

                OnMaxDistanceChanged(attenuationOption_.MaxDistance);
            }
            else
                distanceLineBehaviour_.Disable();
        }

        private void OnMaxDistanceChanged(uint maxDistance)
        {
            maxDistance_ = maxDistance;
            UpdateLine();
        }

        private void OnDistanceChanged(float distance)
        {
            distance_ = distance;
            UpdateLine();
        }

        private void OnEventEnded()
        {
            event_.EventEnded -= OnEventEnded;
            distanceLineBehaviour_.Disable();
        }

        private void UpdateLine()
        {
            if (distance_ < maxDistance_)
                distanceLineBehaviour_.Show();
            else
                distanceLineBehaviour_.Hide();

            var gameObjectName = stringShortener_.Process(event_.GameObjectName);
            if (gameObjectName.Length > MAX_TEXT_LENGTH)
                gameObjectName = gameObjectName.Substring(0, MAX_TEXT_LENGTH - 1) + "…";
            
            var formattedString = distance_.ToString("F3", System.Globalization.CultureInfo.InvariantCulture);
            distanceLineBehaviour_.Label = gameObjectName+"("+formattedString+")";
            distanceLineBehaviour_.Move(distance_ / maxDistance_ * EDITOR_WIDTH);
        }

        public void Dispose()
        {
            distanceLineBehaviour_.DistanceChanged -= OnDistanceChanged;
            attenuationOption_.MaxDistanceChanged -= OnMaxDistanceChanged;
            
            if (event_ == null)
                return;
            event_.EventEnded -= OnEventEnded;
        }
    }
}
