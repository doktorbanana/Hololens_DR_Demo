using System;

namespace SpatialConnect.Wwise.Core
{
    public class SpatialConnectWwise : IDisposable
    {
        private readonly IWaapiSession waapiSession_;
        private readonly IRestorer mixingSessionSelectionRestorer_;

        public SpatialConnectWwise(Action<IWaapiSession> behaviorInitializationAction, IPersistentPropertySet persistentPropertySet, IFactory factory)
        {
            waapiSession_ = factory.CreateWaapiSession();
            mixingSessionSelectionRestorer_ = factory.CreateMixingSessionSelectionRestorer(
                waapiSession_.MixingSessionManager, persistentPropertySet);
            
            behaviorInitializationAction.Invoke(waapiSession_);
            waapiSession_.Start();
            mixingSessionSelectionRestorer_.Restore();
        }

        public void Dispose()
        {
            mixingSessionSelectionRestorer_.Store();
            waapiSession_.Stop();
        }
    }
}
