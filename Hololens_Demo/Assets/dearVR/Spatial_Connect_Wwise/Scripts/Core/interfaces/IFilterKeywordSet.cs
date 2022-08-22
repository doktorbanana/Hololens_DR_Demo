using System;
using System.Collections.Generic;

namespace SpatialConnect.Wwise.Core
{
    public interface IFilterKeywordSet
    {
        IList<IKeyword> Keywords { get; }
        
        event Action KeywordStatesChanged;
    }
}
