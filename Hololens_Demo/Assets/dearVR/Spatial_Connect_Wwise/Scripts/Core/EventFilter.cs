using System;
using System.Collections.Generic;
using System.Linq;

namespace SpatialConnect.Wwise.Core
{
    public class EventFilter : IEventFilter
    {
        private readonly IFilterKeywordSet toggleableFilterKeywordSet_;
        private readonly IFilterKeywordSet permanentFilterKeywordSet_;

        public EventFilter(IFilterKeywordSet toggleableFilterKeywordSet, IFilterKeywordSet permanentFilterKeywordSet = null)
        {
            toggleableFilterKeywordSet_ = toggleableFilterKeywordSet;
            permanentFilterKeywordSet_ = permanentFilterKeywordSet;
            toggleableFilterKeywordSet_.KeywordStatesChanged += () => FilterUpdated?.Invoke();
        }

        public IEnumerable<IEvent> ToggleableFilterList(IEnumerable<IEvent> eventList)
        {
            if (toggleableFilterKeywordSet_.Keywords.All(keyword => !keyword.State))
                return eventList;
            
            return eventList
                .Where(@event => toggleableFilterKeywordSet_.Keywords
                    .Where(keyword => keyword.State)
                    .Any(keyword => @event.Name.Contains(keyword.Name)))
                .ToList();
        }
        
        public IEnumerable<IEvent> PermanentFilterList(IEnumerable<IEvent> eventList)
        {
            if (permanentFilterKeywordSet_ == null)
                return eventList;

            var reducedList = eventList.ToList();
            reducedList.RemoveAll(@event =>
                permanentFilterKeywordSet_.Keywords.Any(keyword => @event.Name.CaseInsensitiveContains(keyword.Name)));
            return reducedList;
        }
        
        public event Action FilterUpdated;
    }
}
