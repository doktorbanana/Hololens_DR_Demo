using System;

namespace SpatialConnect.Wwise.Core
{
    public interface IDelayedExecutionBehaviour
    {
        void ExecuteWithDelay(Action action, float delay);
    }
}
