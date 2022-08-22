using System;
using OVRSimpleJSON;
using SpatialConnect.Wwise.Core;

namespace SpatialConnect.Wwise
{
    public class AttenuationCurveChangedMessageChannel : IAttenuationCurveChangedMessageChannel
    {
        public void Stream(string content)
        {
            var attenuationId = (string)JSON.Parse(content)["attenuation"]["id"];
            AttenuationCurveChanged?.Invoke(attenuationId);
        }

        public event Action<string> AttenuationCurveChanged;
    }
}
