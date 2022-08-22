using System;

namespace SpatialConnect.Wwise.Core
{
    public class ConnectionStatePresenter : IDisposable
    {
        private readonly IWaapiSession waapiSession_;
        private readonly ITextBehaviour connectionStateTextBehaviour_;

        private const string NO_CONNECTION_MESSAGE = "No Wwise project connected!";
        
        public ConnectionStatePresenter(ITextBehaviour connectionStateTextBehaviour,
            IWaapiSession waapiSession)
        {
            connectionStateTextBehaviour_ = connectionStateTextBehaviour;
            waapiSession_ = waapiSession;

            waapiSession_.ConnectionAttempted += OnConnectionAttempted;
            waapiSession_.ProjectNameReceived += OnProjectNameReceived;
        }

        private void OnConnectionAttempted(bool success)
        {
            if (!success)
                connectionStateTextBehaviour_.Text = NO_CONNECTION_MESSAGE;
        }
        
        private void OnProjectNameReceived(string projectName)
        {
            connectionStateTextBehaviour_.Text = projectName;
        }
        
        public void Dispose()
        {
            waapiSession_.ConnectionAttempted -= OnConnectionAttempted;
            waapiSession_.ProjectNameReceived -= OnProjectNameReceived;
        }
    }
}
