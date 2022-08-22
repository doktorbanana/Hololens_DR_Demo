using UnityEngine;
using SpatialConnect.Wwise;

public class ToggleSpatConBehaviour : MonoBehaviour
{
    [SerializeField] private SpatialConnectHandler spatialConnectHandler = default;
    [SerializeField] private Transform centerEyeAnchor = default;
    [SerializeField] private Transform leftControllerAnchor = default;
    [SerializeField] private Transform rightControllerAnchor = default;
    [SerializeField] private KeyCode toggleKey = KeyCode.LeftControl;
    [SerializeField] private KeyCode toggleVisibilityKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode remoteConnectKey = KeyCode.Tab;
    [SerializeField] private bool showOnEnable = false;

    private readonly RemoteConnector remoteConnector_ = new RemoteConnector();

    private bool overlayActive_;
    private bool overlayVisible_;

    private void Update()
    {
        EvaluateToggleKey();
        EvaluateRemoteConnectKey();
        EvaluateToggleVisibiltyKey();
        
        void EvaluateRemoteConnectKey()
        {
            if (!overlayActive_ || !UnityEngine.Input.GetKeyDown(remoteConnectKey)) 
                return;
            remoteConnector_.Toggle();
        }
        
        void EvaluateToggleKey()
        {
            if (!UnityEngine.Input.GetKeyDown(toggleKey)) 
                return;
            if (!overlayActive_)
            {
                Enable();
                if (showOnEnable) 
                    Show();
            }
            else
                Disable();
        }

        void EvaluateToggleVisibiltyKey()
        {
            if (!overlayActive_ || !UnityEngine.Input.GetKeyDown(toggleVisibilityKey))
                return;
            if (!overlayVisible_)
                Show();
            else
                Hide();
        }

        void Enable()
        {
            spatialConnectHandler.EnableOverlay(centerEyeAnchor, leftControllerAnchor, rightControllerAnchor);
            overlayActive_ = true;
            overlayVisible_ = false;
        }
        
        void Disable()
        {
            spatialConnectHandler.DisableOverlay();
            overlayActive_ = false;
        }
        
        void Show()
        {
            spatialConnectHandler.ShowOverlay();
            overlayVisible_ = true;
        }
        
        void Hide()
        {
            spatialConnectHandler.HideOverlay();
            overlayVisible_ = false;
        }
    }
}
