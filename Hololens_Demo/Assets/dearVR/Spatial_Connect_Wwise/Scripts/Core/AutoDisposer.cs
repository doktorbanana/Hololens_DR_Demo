using System;

namespace SpatialConnect.Wwise.Core
{
    public interface IAutoDisposer : IDisposable
    {
        void Update(IDisposable disposable);
    }

    public class AutoDisposer : IAutoDisposer
    {
        private IDisposable disposable_;

        public void Update(IDisposable disposable)
        {
            disposable_?.Dispose();
            disposable_ = disposable;
        }

        public void Dispose()
        {
            disposable_?.Dispose();
        }
    }
}
