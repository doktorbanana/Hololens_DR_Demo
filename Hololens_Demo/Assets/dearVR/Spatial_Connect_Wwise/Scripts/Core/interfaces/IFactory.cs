using System;
using System.Collections.Generic;
using SpatialConnect.Wwise.Core.Wwu;

namespace SpatialConnect.Wwise.Core
{
    public interface IFactory
    {
        IDisposable CreateSpatialConnectWwise(Action<IWaapiSession> behaviourInitializationAction, IPersistentPropertySet persistentPropertySet,
            IFactory factory = null);

        IParser CreateParser(string path, ILoader loader);

        IMixingSessionManager CreateMixingSessionManager(IMessageBroadcaster messageBroadcaster, IFactory factory = null);

        IEventManager CreateEventManager(ICaptureLog captureLog, IMessageBroadcaster messageBroadcaster, IFactory factory = null);

        IRestorer CreateMixingSessionSelectionRestorer
            (IMixingSessionManager mixingSessionManager, IPersistentPropertySet persistentPropertySet);

        IRestorer CreateShortenLabelNameRestorer(IToggleable toggleable, IPersistentPropertySet persistentPropertySet);
        
        IMixingSession CreateMixingSession(MixingSessionPropertySet mixingSessionPropertySet, IMessageBroadcaster messageBroadcaster);

        IAudioObjectPropertySet CreateAudioObjectPropertySet(string id, string name, int maxNumDescendants);

        IAudioObjectPropertySet CreateAudioObjectPropertySetTreeRoot(IAudioObjectPropertySet[] children);

        IAudioObject CreateMixerAudioObject(string audioObjectId, IMessageBroadcaster messageBroadcaster = null,
            IFactory factory = null);

        IAudioObject CreateEventAudioObject(string audioObjectId, IMessageBroadcaster messageBroadcaster, IFactory factory= null);

        IWaapiSession CreateWaapiSession(IFactory factory = null);

        IDisposable CreateConnectionStatePresenter(ITextBehaviour textBehaviour, IWaapiSession waapiSession);

        IDisposable CreateMixingSessionHandlerPresenter(IMixingSessionHandlerBehaviour mixingSessionHandlerBehaviour, IMixingSessionManager mixingSessionManager);

        IDisposable CreateMixerPresenter(IMixerBehaviour mixerBehaviour,
            IMixingSessionManager mixingSessionManager);

        IDisposable CreateMixingSessionPresenter(IMixingSessionBehaviour mixingSessionBehaviour, ISliderBehaviour sliderBehaviour,
            IMixingSession mixingSession);

        IDisposable CreateTextPresenter(ITextBehaviour textBehaviour, IStringShortenerToggle stringShortenerToggle, IText text);

        IDisposable CreateTogglePresenter(IToggleBehaviour toggleBehaviour, IToggleable toggle);

        IDisposable CreateSliderPresenter(ISliderBehaviour sliderBehaviour, IValue value);

        IDisposable CreateMaxDistanceSliderPresenter(ISliderBehaviour maxDistanceSliderBehaviour,
            IPositioningPropertySet positioningPropertySet);

        IDisposable CreateMaxDistanceTextPresenter(ITextBehaviour maxDistanceTextBehaviour,
            IPositioningPropertySet positioningPropertySet);

        IDisposable CreateAttenuationCurvePresenter(IAttenuationCurveEditorBehaviour attenuationCurveEditorBehaviour,
            IPositioningPropertySet positioningPropertySet, IFactory factory = null);

        IDisposable CreateNodeValuePresenter(IAttenuationCurveEditorBehaviour attenuationCurveEditorBehaviour, IAttenuationOption attenuationOption, IFactory factory = null);

        IDisposable CreateInspectorHandlerPresenter(IInspectorBehaviour inspectorBehaviour,
            IMixingSessionManager mixingSessionManager, IEventManager eventManager,
            IProjectPropertySet projectPropertySet);

        IDisposable CreateAttenuationCurveEditorHandlerPresenter(
            IAttenuationCurveEditorHandlerBehaviour attenuationCurveEditorHandlerBehaviour,
            IPositioningPropertySet attenuationPropertySet);

        IDisposable CreateDistanceLinePresenter(IDistanceLineBehaviour distanceLineBehaviour, IStringShortener stringShortener, 
            IPositioningPropertySet positioningPropertySet, IEvent @event);

        IDisposable CreateAttenuationCurveMessagePresenter(ITextBehaviour textBehaviour, IPositioningPropertySet positioningPropertySet);

        IDisposable CreateMixingSessionDropDownPresenter(IDropDownBehaviour dropDownBehaviour, IMixingSessionManager mixingSessionManager);

        IDisposable CreateGridPresenter(IGridBehaviour gridBehaviour, IPositioningPropertySet positioningPropertySet);

        IDisposable CreateMixerMessagePresenter(ITextBehaviour errorMessageTextBehaviour, IMixingSessionManager mixingSessionManager);

        IDisposable CreateAttenuationOptionDropdownPresenter(IDropDownBehaviour dropDownBehaviour, IPositioningPropertySet positioningPropertySet, IProjectPropertySet projectPropertySet);

        IDisposable CreateEventSpherePresenter(IEventSphereBehaviour eventSphereBehaviour, IEvent @event);

        IDisposable CreateEventSphereHandlerPresenter(IEventSphereHandlerBehaviour eventSphereHandlerBehaviour,
            IEventManager eventManager, IEventFilter eventFilter, IToggleable filterEventSpheresToggleable);

        IDisposable CreateEventListItemPresenter(IEventListItemBehaviour eventListItemBehaviour, IStringShortenerToggle stringShortenerToggle, IEvent @event);

        IDisposable CreateEventListHandlerPresenter(IEventListHandlerBehaviour eventListHandlerBehaviour,
            IEventManager eventManager, IEventFilter eventFilter);

        IDisposable CreateEventInspectorHandlerPresenter(
            IEventInspectorHandlerBehaviour eventInspectorHandlerBehaviour, IEventManager eventManager);

        IDisposable CreateAudioObjectListPresenter(
            IAudioObjectListBehaviour audioObjectListBehaviour, IEvent @event);

        IDisposable CreateGhostingPresenter(IDelayedExecutionBehaviour delayedExecutionBehaviour,
            IEventManager eventManager, float ghostingDuration);

        IDisposable CreateSourcePositionPresenter(ISourcePosition sourcePosition, IEvent @event);

        IDisposable CreateSourceObjectFollowerPresenter(ISourceObjectFollowerBehaviour sourceObjectFollowerBehaviour,
            IEventManager eventManager);

        IDisposable CreateSphereLabelPresenter(ITextBehaviour textBehaviour,
            IStringShortenerToggle stringShortenerToggle, IEvent @event);

        IDisposable CreateAudioObjectNameTextPresenter(ITextBehaviour textBehaviour,
            IStringShortenerToggle stringShortenerToggle, string audioObjectName);
        
        INodePointConverter CreateNodePointConverter();

        IWaapiClient CreateWaapiClient();

        IWaapi CreateWaapi();

        IProjectPropertySet CreateProjectPropertySet(IFactory factory = null);

        IText CreateNameText(string id);

        IToggle CreateSoloToggle(string id, IToggleMessageChannel toggleMessageChannel, IWaapi waapi = null);

        IToggle CreateMuteToggle(string id, IToggleMessageChannel toggleMessageChannel, IWaapi waapi = null);

        IChildSet CreateChildSet(string id, string[] returnRequests, Func<string, IAudioObjectPropertySet[]> postProcess);

        IChildSet CreateEventChildSet(string eventId, int maxNumDescendants);

        IChildSet CreateAudioObjectChildSet(string audioObjectId, int maxNumDescendants);

        IValue CreateVoiceVolumeValue(string id, IWaapi waapi = null);

        IPositioningPropertySet CreatePositioningPropertySet(string id, IMessageBroadcaster messageBroadcaster, IFactory factory = null);

        IAttenuationOptionPropertySet CreateAttenuationOptionPropertySet(string id, string name);

        IAttenuationOption CreateAttenuationOption(string id, string name, IAttenuationCurveChangedMessageChannel attenuationCurveChangedMessageChannel);

        ILoader CreateLoader();

        IInput CreateInput();

        IStateChangeNotifier<T> CreateStateChangeNotifier<T>();

        IReset CreateSoloMuteReset(IWaapi waapi = null);

        INodePositioner CreateNodePositioner();

        IMessageBroadcaster CreateMessageBroadcaster(IWaapi waapi = null);

        IToggleMessageChannel CreateSoloToggleMessageChannel();

        IToggleMessageChannel CreateMuteToggleMessageChannel();

        IReferenceChangedMessageChannel CreateAttenuationReferenceChangedMessageChannel();

        IAttenuationCurveChangedMessageChannel CreateAttenuationCurveChangedMessageChannel();

        IExecutionLimiter CreateExecutionLimiter(uint ratio);

        IEvent CreateEvent(EventPropertySet eventPropertySet);

        ICaptureLog CreateCaptureLog();

        IOverrideParent CreateOverridePositioning(string id);

        IClosestTargetSelector CreateClosestTargetFinder();

        IKeyword CreateKeyword(string name);

        IEventFilter CreateEventFilter(IFilterKeywordSet toggleableFilterKeywordSet, IFilterKeywordSet permanentFilterKeywordSet = null);

        IPausableList<IEvent> CreatePausableEventList();

        IListManager<IEventListItemBehaviour> CreateEventListManager(IEnumerable<IEventListItemBehaviour> eventListItemBehaviours);

        IListManager<IAudioObjectListItemBehaviour> CreateAudioObjectListManager(List<IAudioObjectListItemBehaviour> audioObjectListItemBehaviours);

        IToggleable CreateToggleValue();

        IEventStateChangeFinder CreateEventStateChangeFinder();

        ICacheConsumer<IEnumerable<IEvent>> CreateEventCacheConsumer(Action<IEnumerable<IEvent>> action);

        IStringShortenerToggle CreateStringShortenerToggle(int maxCharacters);
    }
}
