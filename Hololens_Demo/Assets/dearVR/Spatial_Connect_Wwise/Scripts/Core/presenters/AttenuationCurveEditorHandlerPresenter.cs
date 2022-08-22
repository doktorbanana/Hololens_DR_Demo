using System;

namespace SpatialConnect.Wwise.Core
{
    public class AttenuationCurveEditorHandlerPresenter : IDisposable
    {
        private readonly IAttenuationCurveEditorHandlerBehaviour attenuationCurveEditorHandlerBehaviour_;
        private readonly IPositioningPropertySet positioningPropertySet_;
        
        public AttenuationCurveEditorHandlerPresenter(
            IAttenuationCurveEditorHandlerBehaviour attenuationCurveEditorHandlerBehaviour,
            IPositioningPropertySet positioningPropertySet)
        {
            attenuationCurveEditorHandlerBehaviour_ = attenuationCurveEditorHandlerBehaviour;
            positioningPropertySet_ = positioningPropertySet;
            positioningPropertySet_.AttenuationOptionChanged += OnAttenuationOptionChanged;
            
            OnAttenuationOptionChanged();
        }

        private void OnAttenuationOptionChanged()
        {
            attenuationCurveEditorHandlerBehaviour_.UpdateAttenuationCurveEditor(positioningPropertySet_);
        }
        
        public void Dispose()
        {
            positioningPropertySet_.AttenuationOptionChanged -= OnAttenuationOptionChanged;
        }
    }   
}
