using System;

namespace SpatialConnect.Wwise.Core
{
    public interface ISliderBehaviour
    {
        float Value { set; }
        
        float MaxValue { set; }
        
        void EnableSlider();
        
        void DisableSlider();

        void HideSlider();
        
        event Action<float> SliderValueChanged;
    }
}
