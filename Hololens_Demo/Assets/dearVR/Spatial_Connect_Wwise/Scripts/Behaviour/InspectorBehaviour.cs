using System;
using UnityEngine;
using SpatialConnect.Wwise.Core;

namespace SpatialConnect.Wwise
{
    public class InspectorBehaviour : MonoBehaviour, IInspectorBehaviour
    {
        [SerializeField] private SliderBehaviour voiceVolumeSliderBehaviour = default;
        [SerializeField] private GridDropdownBehaviour attenuationDropdownBehaviour = default;
        [SerializeField] private AttenuationCurveEditorHandlerBehaviour attenuationCurveEditorHandlerBehaviour = default;
        [SerializeField] private TextBehaviour audioObjectNameTextBehaviour = default;

        private readonly IAutoDisposer dropDownPresenter_ = new AutoDisposer();
        private IDisposable inspectorHandlerPresenter_;
        private IStringShortener stringShortener_;
        private IFactory factory_;

        private Transform centerEyeTransform_;
        
        private const int MAX_NUMBER_OF_INSTANCES = 200;
        
        public void Init(Transform centerEyeTransform, IStringShortener stringShortener, IWaapiSessionPropertySet sessionPropertySet, IFactory factory = null)
        {
            centerEyeTransform_ = centerEyeTransform;
            factory_ = factory?? new Factory();
            stringShortener_ = stringShortener;
            inspectorHandlerPresenter_ = factory_.CreateInspectorHandlerPresenter(this, sessionPropertySet.MixingSessionManager,
                sessionPropertySet.EventManager, sessionPropertySet.ProjectPropertySet);
            
            attenuationDropdownBehaviour.Init(MAX_NUMBER_OF_INSTANCES);
        }
        
        public void UpdateInspector(IAudioObject audioObject, IProjectPropertySet projectPropertySet, IEvent @event)
        {
            var inspectorPropertySet = new InspectorPropertySet(projectPropertySet, @event, centerEyeTransform_);

            audioObjectNameTextBehaviour.TextWithEllipsis = audioObject.NameText.Text;
            voiceVolumeSliderBehaviour.EnableSlider();
            voiceVolumeSliderBehaviour.Init(factory_.CreateSliderPresenter(voiceVolumeSliderBehaviour, audioObject.VoiceVolumeValue));
            dropDownPresenter_.Update(factory_.CreateAttenuationOptionDropdownPresenter(attenuationDropdownBehaviour,
                audioObject.PositioningPropertySet, inspectorPropertySet.ProjectPropertySet));

            attenuationCurveEditorHandlerBehaviour.Init(stringShortener_, audioObject.PositioningPropertySet, inspectorPropertySet);
        }

        private void OnDestroy()
        {
            dropDownPresenter_.Dispose();
            inspectorHandlerPresenter_.Dispose();
        }
    }
}
