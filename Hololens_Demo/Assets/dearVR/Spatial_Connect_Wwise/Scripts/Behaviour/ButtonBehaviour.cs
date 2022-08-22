using UnityEngine;
using UnityEngine.Events;

namespace SpatialConnect.Wwise
{
    public class ButtonBehaviour : MonoBehaviour
    {
        [SerializeField] private UnityEvent buttonAction = default;
        
        private IInputBehaviour inputBehaviour_;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name != "SelectionSphere")
                return;

            inputBehaviour_ = other.gameObject.GetComponent<InputBehaviour>();
            inputBehaviour_.StandardInteractionStateChanged += OnStandardInteractionStateChanged;
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.name != "SelectionSphere")
                return;

            inputBehaviour_.StandardInteractionStateChanged -= OnStandardInteractionStateChanged;
        }
        
        private void OnStandardInteractionStateChanged(bool pressed)
        {
            if (pressed) 
                buttonAction.Invoke();
        }
    }
}
