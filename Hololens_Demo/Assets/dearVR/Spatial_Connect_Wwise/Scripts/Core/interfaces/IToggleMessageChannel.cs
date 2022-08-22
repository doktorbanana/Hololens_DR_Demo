using System;

namespace SpatialConnect.Wwise.Core
{
    public interface IToggleMessageChannel : IMessageChannel
    {
        event Action<string> ToggleReceived;
    
        event Action ResetReceived;
    }
}
