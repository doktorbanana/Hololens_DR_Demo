using System;
using OVRSimpleJSON;
using SpatialConnect.Wwise.Core;

namespace SpatialConnect.Wwise
{
    public class AttenuationReferenceChangedMessageChannel : IReferenceChangedMessageChannel
    {
        public void Stream(string content)
        {
            var json = JSON.Parse(content);
            var reference = (string)json["reference"];
                
            if(reference != "Attenuation")
                return;

            var audioObjectId = json["object"]["id"];
            var attenuationId = json["new"]["id"];
            var attenuationName = json["new"]["name"];

            ReferenceChanged?.Invoke(new ReferenceChangedMessage(audioObjectId, attenuationId, attenuationName));
        }

        public event Action<ReferenceChangedMessage> ReferenceChanged;
    }
}
