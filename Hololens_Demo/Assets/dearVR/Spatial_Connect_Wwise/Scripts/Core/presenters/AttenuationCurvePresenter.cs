using System;
using System.Collections.Generic;
using System.Linq;

namespace SpatialConnect.Wwise.Core
{
    public class AttenuationCurvePresenter : IDisposable
    {
        private readonly INodePointConverter nodePointConverter_;
        private readonly IAttenuationCurveEditorBehaviour attenuationCurveEditorBehaviour_;
        private readonly IAttenuationOption attenuationOption_;

        public AttenuationCurvePresenter(IAttenuationCurveEditorBehaviour attenuationCurveEditorBehaviour,
            IPositioningPropertySet positioningPropertySet, IFactory factory)
        {
            nodePointConverter_ = factory.CreateNodePointConverter();
            attenuationCurveEditorBehaviour_ = attenuationCurveEditorBehaviour;
            attenuationOption_ = positioningPropertySet.AttenuationOption;

            attenuationCurveEditorBehaviour_.EditorChanged += OnEditorChanged;
            attenuationOption_.CurveChanged += OnCurveChanged;
            
            if (positioningPropertySet.IsEditable)
                OnCurveChanged(attenuationOption_.Points);
            else
                attenuationCurveEditorBehaviour_.Nodes = new List<Node>();
        }

        private void OnEditorChanged(List<Node> nodes)
        {
            attenuationOption_.Points =
                nodePointConverter_.NodesToPoints(nodes, attenuationOption_.MaxDistance);
        }

        private void OnCurveChanged(List<Point> points)
        {
            attenuationCurveEditorBehaviour_.Nodes = nodePointConverter_.PointsToNodes(points, (uint) points.Last().x);
        }

        public void Dispose()
        {
            attenuationCurveEditorBehaviour_.EditorChanged -= OnEditorChanged; 
            attenuationOption_.CurveChanged -= OnCurveChanged; 
        }
    }
}
