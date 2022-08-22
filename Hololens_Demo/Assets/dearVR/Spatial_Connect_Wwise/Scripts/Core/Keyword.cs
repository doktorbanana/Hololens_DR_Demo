using System;

namespace SpatialConnect.Wwise.Core
{
    public class Keyword : IKeyword
    {
        public string Name { get; }

        public bool State { get; private set; }

        public Keyword(string name)
        {
            Name = name;
        }

        public void SwitchState()
        {
            State = !State;
            StateChanged?.Invoke(State);
        }

        public event Action<bool> StateChanged;
    }
}
