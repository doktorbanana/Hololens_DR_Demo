using System;
using System.Collections.Generic;

namespace SpatialConnect.Wwise.Core
{
    public interface IStateChangeNotifier<T> 
    {
        T Value { set; }
        
        event Action<T> StateChanged;
    }
    
    public class StateChangeNotifier<T> : IStateChangeNotifier<T> 
    {
        private T state_;
        
        public T Value
        {
            set
            {
                if (EqualityComparer<T>.Default.Equals(state_, value))
                    return;
                
                state_ = value;
                StateChanged?.Invoke(state_);
            }
        }
        
        public event Action<T> StateChanged;
    }
}
