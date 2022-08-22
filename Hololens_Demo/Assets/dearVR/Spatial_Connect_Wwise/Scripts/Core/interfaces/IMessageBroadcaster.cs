using System;

namespace SpatialConnect.Wwise.Core
{
    public interface IMessageBroadcaster : ISubscriber
    {
        IToggleMessageChannel SoloToggleMessageChannel { get; }
        IToggleMessageChannel MuteToggleMessageChannel { get; }
        IReferenceChangedMessageChannel AttenuationReferenceChangedMessageChannel { get; }
        IAttenuationCurveChangedMessageChannel AttenuationCurveChangedMessageChannel { get; }
    }
}
