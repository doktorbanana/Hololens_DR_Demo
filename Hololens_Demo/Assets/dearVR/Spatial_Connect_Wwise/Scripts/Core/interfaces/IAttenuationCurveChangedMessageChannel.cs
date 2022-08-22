using System;

namespace SpatialConnect.Wwise.Core
{
    public interface IAttenuationCurveChangedMessageChannel : IMessageChannel
    {
        event Action<string> AttenuationCurveChanged;
    }
}
