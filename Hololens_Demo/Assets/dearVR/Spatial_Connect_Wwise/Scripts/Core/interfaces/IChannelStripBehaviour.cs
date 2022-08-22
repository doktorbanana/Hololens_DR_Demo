using System;

namespace SpatialConnect.Wwise.Core
{
    public interface IChannelStripBehaviour : IOutlinable
    {
        event Action<int> ChannelStripSelected;
    }
}
