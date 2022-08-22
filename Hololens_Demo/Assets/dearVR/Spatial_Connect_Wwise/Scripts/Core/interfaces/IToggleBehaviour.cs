using System;

namespace SpatialConnect.Wwise.Core
{
    public interface IToggleBehaviour
    {
        bool State { set; }

        event Action Toggled;
    }
}
