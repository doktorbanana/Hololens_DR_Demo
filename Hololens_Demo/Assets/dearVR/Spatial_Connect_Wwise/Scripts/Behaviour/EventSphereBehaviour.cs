using System;
using UnityEngine;
using SpatialConnect.Wwise.Core;

namespace SpatialConnect.Wwise
{
    [RequireComponent(typeof(MeshRenderer))]
    public class EventSphereBehaviour : MonoBehaviour, IEventSphereBehaviour
    {
        [SerializeField] private Renderer glowRingRenderer = default;
        [SerializeField] private SphereLabelBehaviour sphereLabelBehaviour = default;
        
        private const float SPHERE_RADIUS = 0.35f;
        private Renderer renderer_;
        private Color defaultColor_;
        private Color ghostingColor_;
        private RaycastResult raycastResult_;
        private Transform centerEyeAnchorTransform_;
        private IRaycaster raycaster_;
        private IInputBehaviour inputBehaviour_;
        private IDisposable eventSpherePresenter_;
        private IClosestTargetSelector closestTargetSelector_;

        public bool Pointed => raycastResult_.hit;
        
        public uint PlayingId { get; private set; }
        
        public Position Position
        {
            set => transform.localPosition = new Vector3(value.x, value.y, value.z);
        }

        public int FontSize
        {
            set => sphereLabelBehaviour.FontSize = value;
        }

        public bool OutlineActive
        {
            set
            {
                glowRingRenderer.enabled = value;
                sphereLabelBehaviour.OutlineActive = value;
            }
        }
        
        public Color SphereColor
        {
            set
            {
                defaultColor_ = value;
                ghostingColor_ = new Color(value.r, value.g, value.b, 0.25f);
            }
        }

        public void Init(IVrInteraction vrInteraction, IEvent @event, IStringShortenerToggle stringShortenerToggle, IClosestTargetSelector closestTargetSelector, IFactory factory = null)
        {
            factory ??= new Factory();
            centerEyeAnchorTransform_ = vrInteraction.CenterEyeAnchorTransform;
            raycaster_ = new Raycaster(vrInteraction.ControllerAnchorTransform, transform, SPHERE_RADIUS);
            inputBehaviour_ = vrInteraction.InputBehaviour;
            closestTargetSelector_ = closestTargetSelector;
            renderer_ = GetComponent<Renderer>();
            
            eventSpherePresenter_ = factory.CreateEventSpherePresenter(this, @event);
            closestTargetSelector_.StartListening(this);
            inputBehaviour_.StandardInteractionStateChanged += OnStandardInteractionStateChanged;
            renderer_.material.color = @event.Ghosting ? ghostingColor_ : defaultColor_;

            sphereLabelBehaviour.Init(@event, stringShortenerToggle, centerEyeAnchorTransform_, transform);
            PlayingId = @event.PlayingId;
        }

        private void Update()
        {
            raycastResult_ = raycaster_.Check();
            
            if (glowRingRenderer.enabled)
                transform.eulerAngles = new Vector3(0f, centerEyeAnchorTransform_.rotation.eulerAngles.y, 0f);
        }

        private void OnStandardInteractionStateChanged(bool state)
        {
            if (Pointed && state)
                Shot?.Invoke(this, raycastResult_.distance);
        }

        public void Select()
        {
            Selected?.Invoke(this);
        }

        public void TurnIntoGhost()
        {
            renderer_.material.color = ghostingColor_;
        }

        public void Deactivate()
        {
            inputBehaviour_.StandardInteractionStateChanged -= OnStandardInteractionStateChanged;
            eventSpherePresenter_.Dispose();
            closestTargetSelector_.StopListening(this);
            gameObject.SetActive(false);
        }
        
        public event Action<object, float> Shot;
        public event Action<object> Selected;
    }
}
