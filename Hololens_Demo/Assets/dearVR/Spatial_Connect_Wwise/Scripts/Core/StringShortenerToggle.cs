using System;

namespace SpatialConnect.Wwise.Core
{
    public interface IStringShortener
    {
        string Process(string original);
    }
    
    public interface IStringShortenerToggle : IStringShortener, IToggleable
    {
    }
    
    public class StringShortenerToggle : IStringShortenerToggle
    {
        private readonly int maxCharacters_;

        public bool State { get; private set; }

        public StringShortenerToggle(int maxCharacters)
        {
            maxCharacters_ = maxCharacters;
        }
        
        public string Process(string original)
        {
            return State ? original.Substring(Math.Max(0, original.Length - maxCharacters_)) : original;
        }

        public void SwitchState()
        {
            State = !State;
            StateChanged?.Invoke(State);
        }

        public event Action<bool> StateChanged;
    }
}
