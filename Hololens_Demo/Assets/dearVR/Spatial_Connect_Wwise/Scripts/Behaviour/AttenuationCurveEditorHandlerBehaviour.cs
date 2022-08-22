using System;
using UnityEngine;
using SpatialConnect.Wwise.Core;

namespace SpatialConnect.Wwise
{
    public class AttenuationCurveEditorHandlerBehaviour : MonoBehaviour, IAttenuationCurveEditorHandlerBehaviour
    {
        [SerializeField] private AttenuationCurveEditorBehaviour attenuationCurveEditorBehaviour = default;

        private IDisposable attenuationCurveEditorHandlerPresenter_;
        private IDistanceLinePropertySet distanceLinePropertySet_;
        private IStringShortener stringShortener_;
        
        public void Init(IStringShortener stringShortener, IPositioningPropertySet positioningPropertySet, IDistanceLinePropertySet distanceLinePropertySet, IFactory factory = null)
        {
            factory ??= new Factory();
            stringShortener_ = stringShortener;
            distanceLinePropertySet_ = distanceLinePropertySet;

            attenuationCurveEditorHandlerPresenter_ = factory.CreateAttenuationCurveEditorHandlerPresenter(this, positioningPropertySet);
        }
        
        public void UpdateAttenuationCurveEditor(IPositioningPropertySet positioningPropertySet)
        {
            attenuationCurveEditorBehaviour.Init(stringShortener_, distanceLinePropertySet_, positioningPropertySet);
        }

        private void OnDestroy()
        {
            attenuationCurveEditorHandlerPresenter_.Dispose();
        }
    }
}
