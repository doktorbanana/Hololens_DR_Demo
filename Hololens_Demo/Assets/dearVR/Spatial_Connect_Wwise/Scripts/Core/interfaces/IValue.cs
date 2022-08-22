using System;

namespace SpatialConnect.Wwise.Core
{
    public interface IValue : IDisposable
    {
        float Value { get; set; }

        event Action<float> ValueChanged;
    }
}
