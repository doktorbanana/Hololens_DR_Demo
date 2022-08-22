using System;
using SpatialConnect.Wwise.Core;
using UnityEngine;
using UnityEngine.UI;

namespace SpatialConnect.Wwise
{
    [RequireComponent(typeof(Image))]
    public class IconToggleBehaviour : MonoBehaviour, IToggleBehaviour
    {
        [SerializeField] private Sprite defaultSprite = default;
        [SerializeField] private Sprite alternativeSprite = default;
        
        private Image image_;
        private IInputBehaviour inputBehaviour_;
        private IDisposable togglePresenter_;

        public void Init(IToggleable toggle, IFactory factory = null)
        {
            image_ = GetComponent<Image>();
            factory ??= new Factory();
            togglePresenter_ = factory.CreateTogglePresenter(this, toggle);
        }

        public bool State
        {
            set => image_.sprite = value ? alternativeSprite : defaultSprite;
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
            togglePresenter_.Dispose();
        }

        public event Action Toggled;
    }
}
