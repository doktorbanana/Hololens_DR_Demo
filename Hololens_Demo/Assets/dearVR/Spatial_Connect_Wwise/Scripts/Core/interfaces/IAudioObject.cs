using System;

namespace SpatialConnect.Wwise.Core
{
    public interface IAudioObject : IDisposable
    {
        string Id { get; }
        IText NameText { get; }
        IToggle SoloToggle { get; }
        IToggle MuteToggle { get; }
        IValue VoiceVolumeValue { get; }
        IPositioningPropertySet PositioningPropertySet { get; }
    }
}
