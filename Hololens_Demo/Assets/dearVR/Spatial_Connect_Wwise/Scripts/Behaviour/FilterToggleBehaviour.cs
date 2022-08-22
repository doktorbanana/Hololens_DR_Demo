using UnityEngine;
using UnityEngine.UI;
using SpatialConnect.Wwise.Core;

namespace SpatialConnect.Wwise
{
    public class FilterToggleBehaviour : MonoBehaviour
    {
        [SerializeField] private Text filterKeywordText = default;
        [SerializeField] private Color pressedColor = default;
        [SerializeField] private Image backgroundImage = default;

        private IKeyword keyword_;
        private IInputBehaviour inputBehaviour_;
        
        public void Init(IKeyword keyword)
        {
            keyword_ = keyword;
            filterKeywordText.text = keyword.Name;
            keyword_.StateChanged += OnStateChanged;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name != "SelectionSphere")
                return;

            inputBehaviour_ = other.gameObject.GetComponent<InputBehaviour>();
            inputBehaviour_.StandardInteractionStateChanged += OnStandardInteractionStateChanged;
        }

        private void OnStandardInteractionStateChanged(bool pressed)
        {
            if (pressed)
                keyword_.SwitchState();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.name != "SelectionSphere")
                return;

            inputBehaviour_.StandardInteractionStateChanged -= OnStandardInteractionStateChanged;
        }

        private void OnStateChanged(bool state)
        {
            backgroundImage.color = state ? pressedColor : Color.white;
        }
    }
}
