using System;

namespace SpatialConnect.Wwise.Core
{
    public interface IAudioObjectListBehaviour
    {
        void Select(int[] index);

        void UpdateList(IEvent @event);
        
        event Action<int[]> AudioObjectListSelectionChanged;
    }
}
