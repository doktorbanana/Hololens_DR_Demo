using System;

namespace SpatialConnect.Wwise.Core
{
    public class AttenuationCurveMessagePresenter : IDisposable
    {
        private const string ATTENUATION_DISABLED_MESSAGE = "Attenuation is disabled for this Audio Object";
        private const string NO_SHARE_SET_MESSAGE = "No Share Set selected for this Audio Object";
        private const string CUSTOM_DEFINED_MESSAGE = "Custom defined Attenuation not supported";
        private const string POSITIONING_IS_INHERITED = "Positioning is inherited from the parent";

        public AttenuationCurveMessagePresenter(ITextBehaviour textBehaviour, IPositioningPropertySet positioningPropertySet)
        {
            if (!positioningPropertySet.OverridePositioning.Enabled)
                textBehaviour.Text = POSITIONING_IS_INHERITED;
            else if (!positioningPropertySet.IsAttenuationEnabled)
                textBehaviour.Text = ATTENUATION_DISABLED_MESSAGE;
            else
                switch (positioningPropertySet.AttenuationOption.Type)
                {
                    case AttenuationType.None:
                        textBehaviour.Text = NO_SHARE_SET_MESSAGE;
                        break;
                    case AttenuationType.Custom:
                        textBehaviour.Text = CUSTOM_DEFINED_MESSAGE;
                        break;
                    default:
                        textBehaviour.Text = "";
                        break;
                }
        }

        public void Dispose()
        {
        }
    }
}
