using System;

namespace SpatialConnect.Wwise.Core
{
    public interface IInput : IDisposable
    {
        bool LeftIndexTrigger { get; }
        bool RightIndexTrigger { get; }
        bool LeftHandTrigger { get; }
        bool RightHandTrigger { get; }
        bool LeftThumbstickClick { get; }
        bool RightThumbstickClick { get; }
        bool AButton { get; }
        bool BButton { get; }
        bool XButton { get; }
        bool YButton { get; }
    }
}
