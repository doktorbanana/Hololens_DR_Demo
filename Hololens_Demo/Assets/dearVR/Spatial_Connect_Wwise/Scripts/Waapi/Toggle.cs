using System;
using OVRSimpleJSON;
using SpatialConnect.Wwise.Core;

namespace SpatialConnect.Wwise
{
    public class Toggle : IToggle
    {
        private readonly IWaapi waapi_;
        private readonly IToggleMessageChannel toggleMessageChannel_;
        private readonly string id_;
        private readonly string command_;

        private bool state_;
        public bool State
        {
            get => state_;
            private set
            {
                state_ = value;
                StateChanged?.Invoke(state_);
            }
        }

        public Toggle(IWaapi waapi, IToggleMessageChannel toggleMessageChannel, string id, string command)
        {
            waapi_ = waapi;
            toggleMessageChannel_ = toggleMessageChannel;
            id_ = id;
            command_ = command;

            toggleMessageChannel_.ToggleReceived += OnToggleReceived;
            toggleMessageChannel_.ResetReceived += OnResetReceived;
        }

        private void OnToggleReceived(string id)
        {
            if (id == id_)
                State = !State;
        }

        private void OnResetReceived()
        {
            State = false;
        }

        public void SwitchState()
        {
            var args = new JSONObject {["command"] = command_, ["objects"] = WaapiUtility.ToJSONArray(id_)};
            waapi_.Call("ak.wwise.ui.commands.execute", args.ToString(), "{}");
        }
        
        public void Dispose()
        {
            toggleMessageChannel_.ToggleReceived -= OnToggleReceived;
            toggleMessageChannel_.ResetReceived -= OnResetReceived;
        }
        
        public event Action<bool> StateChanged;
    }
}
