using System;

namespace SpatialConnect.Wwise.Core
{
    public interface IExecutionLimiter
    {
        void TryExecute(Action action);
    }
}
