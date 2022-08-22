using System;

namespace SpatialConnect.Wwise.Core
{
    public readonly struct ReferenceChangedMessage
    {
        public readonly string targetAudioObjectId;
        public readonly string attenuationId;
        public readonly string attenuationName;

        public ReferenceChangedMessage(string targetAudioObjectId, string attenuationId, string attenuationName)
        {
            this.targetAudioObjectId = targetAudioObjectId;
            this.attenuationId = attenuationId;
            this.attenuationName = attenuationName;
        }
    }
    
    public interface IReferenceChangedMessageChannel : IMessageChannel
    {
        event Action<ReferenceChangedMessage> ReferenceChanged;
    }
}
