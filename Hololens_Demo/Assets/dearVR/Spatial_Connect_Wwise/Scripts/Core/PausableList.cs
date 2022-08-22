using System;
using System.Collections;
using System.Collections.Generic;

namespace SpatialConnect.Wwise.Core
{
    public interface IPausableList<T> : IEnumerable<T>, IToggleable
    {
        void AddToTop(T item);

        void Remove(T item);

        T Find(Predicate<T> predicate);
    }

    public class PausableList<T> : IPausableList<T>
    {
        private readonly List<T> list_ = new List<T>();
        private List<T> activeList_;

        public PausableList()
        {
            activeList_ = list_;
        }
        
        public bool State { get; private set; }
        
        public void SwitchState()
        {
            State = !State;
            activeList_ = State ? new List<T>(list_) : list_;
            StateChanged?.Invoke(State);
        }

        public T Find(Predicate<T> predicate)
        {
            return list_.Find(predicate);
        }

        public void AddToTop(T item)
        {
            list_.Insert(0, item);
        }

        public void Remove(T item)
        {
            list_.Remove(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return activeList_.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return activeList_.GetEnumerator();
        }
        
        public event Action<bool> StateChanged;
    }
}
