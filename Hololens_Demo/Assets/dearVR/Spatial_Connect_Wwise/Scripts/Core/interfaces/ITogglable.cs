using System;

namespace SpatialConnect.Wwise.Core
{
    public interface IToggleable
    {
        bool State { get; }

        void SwitchState();

        event Action<bool> StateChanged;
    }
}
