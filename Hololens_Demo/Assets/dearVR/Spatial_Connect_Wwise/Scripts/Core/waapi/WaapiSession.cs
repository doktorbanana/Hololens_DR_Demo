using System;
using System.Collections.Generic;
using System.IO;

namespace SpatialConnect.Wwise.Core
{
    public interface IWaapiSessionPropertySet
    {
        IMixingSessionManager MixingSessionManager { get; }
        IEventManager EventManager { get; }
        IProjectPropertySet ProjectPropertySet { get; }
    }
    
    public interface IWaapiSession : IWaapiSessionPropertySet
    {
        void Start();
        void Stop();

        event Action<bool> ConnectionAttempted;
        event Action<string> ProjectNameReceived;
    }

    public class WaapiSession : IWaapiSession
    {
        public IMixingSessionManager MixingSessionManager { get; }
        public IEventManager EventManager { get; }
        public IProjectPropertySet ProjectPropertySet { get; }

        private readonly List<ISubscriber> subscribers_ = new List<ISubscriber>();
        private readonly IFactory factory_;
        private readonly IWaapiClient waapiClient_;

        public WaapiSession(IFactory factory)
        {
            factory_ = factory;
            waapiClient_ = factory_.CreateWaapiClient();
            ProjectPropertySet = factory_.CreateProjectPropertySet();

            var messageBroadcaster = factory_.CreateMessageBroadcaster();
            subscribers_.Add(messageBroadcaster); 

            var captureLog = factory_.CreateCaptureLog();
            subscribers_.Add(captureLog);
            
            MixingSessionManager = factory_.CreateMixingSessionManager(messageBroadcaster);
            EventManager = factory_.CreateEventManager( captureLog, messageBroadcaster);
        }

        public void Start()
        {
            var success = waapiClient_.Connect();
            ConnectionAttempted?.Invoke(success);
            if (!success)
                return;
            
            ProjectNameReceived?.Invoke(ProjectPropertySet.Name);

            var wwuFilePath = Path.GetDirectoryName(ProjectPropertySet.ProjectPath) +
                              "\\Mixing Sessions\\Default Work Unit.wwu";

            MixingSessionManager.Load(wwuFilePath);
            factory_.CreateSoloMuteReset();

            foreach (var subscriber in subscribers_)
                subscriber.Subscribe();
        }

        public void Stop()
        {
            waapiClient_.Disconnect();
        }

        public event Action<bool> ConnectionAttempted;
        public event Action<string> ProjectNameReceived;
    }
}
