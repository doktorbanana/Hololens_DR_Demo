using System;
using UnityEngine;
using UnityEngine.UI;
using SpatialConnect.Wwise.Core;

namespace SpatialConnect.Wwise
{
    public class ToggleBehaviour : MonoBehaviour, IToggleBehaviour
    {
        [SerializeField] private Color pressedColor = default;
        [SerializeField] private Image backgroundImage = default;

        private IInputBehaviour inputBehaviour_;

        public event Action Toggled;
        private bool state_;

        private IDisposable togglePresenter_;

        public bool State
        {
            get => state_;
            set
            {
                state_ = value;
                backgroundImage.color = state_ ? pressedColor : Color.white;
            }
        }

        public void Init(IToggleable toggle, IFactory factory = null)
        {
            factory ??= new Factory();
            togglePresenter_ = factory.CreateTogglePresenter(this, toggle);
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
                Toggled?.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.name != "SelectionSphere")
                return;

            inputBehaviour_.StandardInteractionStateChanged -= OnStandardInteractionStateChanged;
        }

        private void OnDestroy()
        {
            togglePresenter_?.Dispose();
        }
    }
}
