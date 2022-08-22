using System;
using UnityEngine;
using UnityEngine.UI;

namespace SpatialConnect.Wwise
{
    public interface ISliderInputTranslator
    {
        float Translate(Transform sliderTransform, Transform otherTransform);
    }
    
    public class SliderInputTranslator : ISliderInputTranslator
    {
        private readonly Slider slider_;
        private readonly Rect rect_;
        private readonly float range_;

        public SliderInputTranslator(Slider slider, Rect rect)
        {
            slider_ = slider;
            rect_ = rect;
            range_ = slider.maxValue - slider.minValue;
        }

        public float Translate(Transform sliderTransform, Transform otherTransform)
        {
            float Proportion()
            {
                switch (slider_.direction)
                {
                    case Slider.Direction.LeftToRight:
                        return sliderTransform.InverseTransformPoint(otherTransform.position).x / rect_.width;
                    case Slider.Direction.RightToLeft:
                        return -sliderTransform.InverseTransformPoint(otherTransform.position).x / rect_.width;
                    case Slider.Direction.BottomToTop:
                        return sliderTransform.InverseTransformPoint(otherTransform.position).y / rect_.height;
                    case Slider.Direction.TopToBottom:
                        return -sliderTransform.InverseTransformPoint(otherTransform.position).y / rect_.height;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            var value = range_ * Proportion() + slider_.minValue;
            return Mathf.Clamp(value, slider_.minValue, slider_.maxValue);
        }
    }
}
