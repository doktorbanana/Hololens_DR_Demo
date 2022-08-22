using System;
using OVRSimpleJSON;
using SpatialConnect.Wwise.Core;

namespace SpatialConnect.Wwise
{
    public class ToggleMessageChannel : IToggleMessageChannel
    {
        private readonly string toggleType_;
        private readonly string resetCommand_;
        
        public ToggleMessageChannel(string toggleType)
        {
            toggleType_ = toggleType;
            resetCommand_ = "ResetAll" + toggleType + "s";
        }

        public void Stream(string content)
        {
            var json = JSON.Parse(content);
            var command = json["command"];
                
            if (command == toggleType_)
                ToggleReceived?.Invoke(json["objects"][0]["id"]);
            if (command == resetCommand_)
                ResetReceived?.Invoke();
        }

        public event Action<string> ToggleReceived;
        public event Action ResetReceived;
    }
}
