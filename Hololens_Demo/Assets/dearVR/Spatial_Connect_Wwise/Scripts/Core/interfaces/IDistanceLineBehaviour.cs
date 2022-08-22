using System;

namespace SpatialConnect.Wwise.Core
{
    public interface IDistanceLineBehaviour
    {
        string Label { set; }

        void Enable();

        void Disable();

        void Show();

        void Hide();

        void Move(float position);

        event Action<float> DistanceChanged;
    }
}
