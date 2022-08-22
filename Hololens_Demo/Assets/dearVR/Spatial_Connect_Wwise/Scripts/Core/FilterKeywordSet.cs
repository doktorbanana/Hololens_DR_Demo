using System;
using System.Linq;
using System.Collections.Generic;

namespace SpatialConnect.Wwise.Core
{
    public class FilterKeywordSet : IFilterKeywordSet
    {
        public IList<IKeyword> Keywords { get; }
        
        public FilterKeywordSet(string[] keywords, IFactory factory = null)
        {
            Keywords = keywords
                .Where(keywordString => keywordString != "")
                .Select(keywordString =>
                {
                    var keyword = factory.CreateKeyword(keywordString);
                    keyword.StateChanged += OnStateChanged;
                    return keyword;
                }).ToList();
        }

        private void OnStateChanged(bool state)
        {
            KeywordStatesChanged?.Invoke();
        }

        public event Action KeywordStatesChanged;
    }
}
