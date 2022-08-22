using System;

namespace SpatialConnect.Wwise.Core
{
    public class TogglePresenter : IDisposable
    {
        private readonly IToggleable toggle_;
        private readonly IToggleBehaviour toggleBehaviour_;
        
        public TogglePresenter(IToggleBehaviour toggleBehaviour, IToggleable toggle)
        {
            toggleBehaviour_ = toggleBehaviour;
            toggle_ = toggle;
            
            toggle_.StateChanged += OnToggleValueChanged;
            toggleBehaviour_.Toggled += OnToggleComponentChanged;
            
            OnToggleValueChanged(toggle_.State);
        }

        private void OnToggleValueChanged(bool state)
        {
            toggleBehaviour_.State = state;
        }

        private void OnToggleComponentChanged()
        {
            toggle_.SwitchState();
        }

        public void Dispose()
        {
            toggle_.StateChanged -= OnToggleValueChanged;
            toggleBehaviour_.Toggled -= OnToggleComponentChanged;
        }
    }
}
