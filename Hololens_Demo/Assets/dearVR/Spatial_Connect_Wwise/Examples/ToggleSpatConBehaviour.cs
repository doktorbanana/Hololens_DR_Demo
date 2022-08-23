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
    
    private void Start()
    {
        centerEyeAnchor = GameObject.Find("UIRaycastCamera").transform;

    }

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

            if (GameObject.Find("Left_RiggedHandLeft(Clone)"))
            {
                leftControllerAnchor = GameObject.Find("Left_RiggedHandLeft(Clone)").transform;
            }
            else
            {
                Debug.LogWarning("Couldn't find Hand using something else as anchor");
                Vector3 pos = centerEyeAnchor.position;
                pos = pos + new Vector3(-1f, -1.5f, 0f);
                GameObject defaultLeftAnchor = new GameObject();
                defaultLeftAnchor.transform.position = pos;
                leftControllerAnchor = defaultLeftAnchor.transform;
            }

            if (GameObject.Find("Right_RiggedHandRight(Clone)"))
            {
                leftControllerAnchor = GameObject.Find("Right_RiggedHandRight(Clone)").transform;
            }
            else
            {
                Debug.LogWarning("Couldn't find Hand using something else as anchor");
                Vector3 pos = centerEyeAnchor.position;
                pos = pos + new Vector3(1f, -1.5f, 0f);
                GameObject defaultLeftAnchor = new GameObject();
                defaultLeftAnchor.transform.position = pos;
                leftControllerAnchor = defaultLeftAnchor.transform;
            }

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
