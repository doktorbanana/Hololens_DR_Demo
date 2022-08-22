using System;
using System.Collections.Generic;
using System.Linq;

namespace SpatialConnect.Wwise.Core
{
    public interface IListManager<T>
    {
        void Highlight(int? index);

        void Select(int index);

        void ForEach(Action<T> action);
    }
    
    public class ListManager<T> : IListManager<T> where T : class, ISelectable, IHighlightable
    {
        private readonly IEnumerable<T> items_;

        public ListManager(IEnumerable<T> items)
        {
            items_ = items;
        }

        public void Highlight(int? index)
        {
            if (!index.HasValue)
                foreach (var item in items_)
                    item.HighlightActive = false;
            else
            {
                var i = 0;
                foreach (var item in items_)
                {
                    item.HighlightActive = i == index.Value;
                    i++;
                }
            }
        }

        public void Select(int index)
        {
            if (index < items_.Count())
                items_.ToList()[index].Select();
        }

        public void ForEach(Action<T> action)
        {
            foreach (var item in items_)
                action.Invoke(item);
        }
    }
}
