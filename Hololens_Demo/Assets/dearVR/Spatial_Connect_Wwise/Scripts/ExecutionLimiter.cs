using System;
using UnityEngine;
using SpatialConnect.Wwise.Core;

namespace SpatialConnect.Wwise
{
    public class ExecutionLimiter : IExecutionLimiter
    {
        private float elapsedTime_;
        private readonly float interval_ = 1f;

        public ExecutionLimiter(uint ratio)
        {
            interval_ = 1f / ratio;
        }

        public void TryExecute(Action action)
        {
            elapsedTime_ += Time.fixedDeltaTime;
            if (elapsedTime_ < interval_)
                return;

            action.Invoke();
            elapsedTime_ = 0f;
        }
    }
}
