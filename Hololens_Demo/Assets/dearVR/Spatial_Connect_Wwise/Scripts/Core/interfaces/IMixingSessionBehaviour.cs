using System;

namespace SpatialConnect.Wwise.Core
{
    public interface IMixingSessionBehaviour
    {
        int? ChannelStripSelection { set; }

        void ScrollTo(int offset);

        event Action<int> ChannelStripSelectionChanged;
    }
}
