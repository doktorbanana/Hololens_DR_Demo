using System;
using UnityEngine;
using SpatialConnect.Wwise.Core;

namespace SpatialConnect.Wwise
{
    [RequireComponent(typeof(DelayedExecutionBehaviour))]
    public class SpatialConnectBehaviour : MonoBehaviour, IShowable
    {
        [SerializeField] private ConnectionStateTextBehaviour connectionStateTextBehaviour = default;
        [SerializeField] private MixerBehaviour mixerBehaviour = default;
        [SerializeField] private InspectorBehaviour inspectorBehaviour = default;
        [SerializeField] private EventPanelBehaviour eventPanelBehaviour = default;
        [SerializeField] private StringShortenerBehaviour stringShortenerBehaviour = default;
        [SerializeField] private PersistentPropertySet persistentPropertySet = default;
        [SerializeField] private GameObject eventSphereHandlerPrefab = default;

        private readonly IFactory factory_ = new Factory();
        private IDisposable spatialConnect_;

        private IVrInteraction vrInteraction_;
        private IUserPreferenceSet userPreferenceSet_;
        private IRestorer shortenNameLabelRestorer_;
        private EventSphereHandlerBehaviour eventSphereHandlerBehaviour_;
        private DelayedExecutionBehaviour delayedExecutionBehaviour_;

        private void Awake()
        {
            DontDestroyOnLoad(this);
            var eventSphereHandler = Instantiate(eventSphereHandlerPrefab);
            eventSphereHandler.name = "EventSphereHandler";
            eventSphereHandlerBehaviour_ = eventSphereHandler.GetComponent<EventSphereHandlerBehaviour>();
            delayedExecutionBehaviour_ = GetComponent<DelayedExecutionBehaviour>();
        }

        private void Start()
        {
            void BehaviourInitialization(IWaapiSession waapiSession)
            {
                connectionStateTextBehaviour.Init(waapiSession);
                mixerBehaviour.Init(waapiSession.MixingSessionManager, userPreferenceSet_.StringShortenerToggle);
                inspectorBehaviour.Init(vrInteraction_.CenterEyeAnchorTransform, userPreferenceSet_.StringShortenerToggle, waapiSession);
                eventSphereHandlerBehaviour_.Init(vrInteraction_, waapiSession.EventManager, userPreferenceSet_, 
                    factory_.CreateEventFilter(userPreferenceSet_.ToggleableFilterKeywordSet, userPreferenceSet_.PermanentFilterKeywordSet));
                eventPanelBehaviour.Init(waapiSession.EventManager, userPreferenceSet_);
                stringShortenerBehaviour.Init(userPreferenceSet_.StringShortenerToggle);

                var presenters = new[]
                {
                    factory_.CreateGhostingPresenter(delayedExecutionBehaviour_, waapiSession.EventManager,
                        userPreferenceSet_.GhostingDuration)
                };
                delayedExecutionBehaviour_.Init(presenters);
            }

            spatialConnect_ = factory_.CreateSpatialConnectWwise(BehaviourInitialization, persistentPropertySet);
        }

        public void Init(IVrInteraction vrInteraction, IUserPreferenceSet userPreferenceSet)
        {
            vrInteraction_ = vrInteraction;
            userPreferenceSet_ = userPreferenceSet;
            transform.GetChild(0).position = userPreferenceSet_.OverlayOffsetPosition;
            
            shortenNameLabelRestorer_ =
                factory_.CreateShortenLabelNameRestorer(userPreferenceSet_.StringShortenerToggle, persistentPropertySet);
            shortenNameLabelRestorer_.Restore();
        }

        public void Show()
        {
            eventSphereHandlerBehaviour_.Show();
        }

        public void Hide()
        {
            eventSphereHandlerBehaviour_.Hide();
        }

        private void OnDestroy()
        {
            shortenNameLabelRestorer_.Store();
            if (eventSphereHandlerBehaviour_)
                Destroy(eventSphereHandlerBehaviour_.gameObject);
            spatialConnect_.Dispose();
        }

        private void OnApplicationQuit()
        {
            spatialConnect_.Dispose();
        }
    }
}
