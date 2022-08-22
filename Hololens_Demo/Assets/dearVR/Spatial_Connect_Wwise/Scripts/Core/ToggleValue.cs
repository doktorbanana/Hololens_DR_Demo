using System;

namespace SpatialConnect.Wwise.Core
{
    public class ToggleValue : IToggleable
    {
        public bool State { get; private set; }

        public void SwitchState()
        {
            State = !State;
            StateChanged?.Invoke(State);
        }

        public event Action<bool> StateChanged;
    }
}
