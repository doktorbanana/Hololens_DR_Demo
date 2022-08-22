using System;

namespace SpatialConnect.Wwise.Core
{
    public interface IAudioObjectListItemBehaviour : IOutlinable, ISelectable, IHighlightable
    {
        int[] Index { get; }
        
        event Action<int[]> Selected;
    }
}
