using System;
using System.Collections.Generic;

namespace SpatialConnect.Wwise.Core
{
    public class NodeValuePresenter : IDisposable
    {
        private readonly IAttenuationCurveEditorBehaviour attenuationCurveEditorBehaviour_;
        private readonly IAttenuationOption attenuationOption_;
        private readonly INodePointConverter nodePointConverter_;


        public NodeValuePresenter(IAttenuationCurveEditorBehaviour attenuationCurveEditorBehaviour, IAttenuationOption attenuationOption, IFactory factory)
        {
            attenuationCurveEditorBehaviour_ = attenuationCurveEditorBehaviour;
            attenuationOption_ = attenuationOption;
            nodePointConverter_ = factory.CreateNodePointConverter();

            attenuationCurveEditorBehaviour_.HoverStateUpdated += OnHoverStateChanged;
        }

        private void OnHoverStateChanged(Node? node)
        {
            if(!node.HasValue)
            {
                attenuationCurveEditorBehaviour_.NodeHoverText = "";
                return;
            }

            var maxDistance = attenuationOption_.MaxDistance;

            var point = nodePointConverter_.NodeToPoint(node.Value, maxDistance);

            var xFormatted = point.x.ToString("0.0", System.Globalization.CultureInfo.InvariantCulture);
            var yFormatted = point.y.ToString("0.0", System.Globalization.CultureInfo.InvariantCulture);

            attenuationCurveEditorBehaviour_.NodeHoverText = "x: " + xFormatted + "m | y: " + yFormatted + "dB";
        }

        public void Dispose()
        {
            attenuationCurveEditorBehaviour_.HoverStateUpdated -= OnHoverStateChanged;
        }
    }
}
