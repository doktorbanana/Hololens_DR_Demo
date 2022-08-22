using System;
using System.Collections.Generic;
using SpatialConnect.Wwise.Wwu;
using SpatialConnect.Wwise.Core;
using SpatialConnect.Wwise.Core.Wwu;

namespace SpatialConnect.Wwise
{
    public class Factory : IFactory
    {
        public IDisposable CreateSpatialConnectWwise(Action<IWaapiSession> behaviourInitializationAction, IPersistentPropertySet persistentPropertySet,
            IFactory factory)
        {
            factory ??= this;
            return new SpatialConnectWwise(behaviourInitializationAction, persistentPropertySet, factory);
        }

        public IParser CreateParser(string path, ILoader loader)
        {
            return new Parser(path, loader);
        }

        public IMixingSessionManager CreateMixingSessionManager(IMessageBroadcaster messageBroadcaster, IFactory factory = null)
        {
            factory ??= this;
            return new MixingSessionManager(messageBroadcaster, factory);
        }

        public IEventManager CreateEventManager(ICaptureLog captureLog, IMessageBroadcaster messageBroadcaster, IFactory factory = null)
        {
            factory ??= this;
            return new EventManager(captureLog, messageBroadcaster, factory);
        }

        public IRestorer CreateMixingSessionSelectionRestorer(
            IMixingSessionManager mixingSessionManager, IPersistentPropertySet persistentPropertySet)
        {
            return new MixingSessionSelectionRestorer(mixingSessionManager, persistentPropertySet);
        }

        public IRestorer CreateShortenLabelNameRestorer(IToggleable toggleable, IPersistentPropertySet persistentPropertySet)
        {
            return new ShortenNameLabelRestorer(toggleable, persistentPropertySet);
        }

        public IMixingSession CreateMixingSession(MixingSessionPropertySet mixingSessionPropertySet,
            IMessageBroadcaster messageBroadcaster)
        {
            return new MixingSession(mixingSessionPropertySet, messageBroadcaster, this);
        }

        public IAudioObjectPropertySet CreateAudioObjectPropertySet(string id, string name, int maxNumDescendants)
        {
            return new AudioObjectPropertySet(id, name, maxNumDescendants, this);
        }

        public IAudioObjectPropertySet CreateAudioObjectPropertySetTreeRoot(IAudioObjectPropertySet[] children)
        {
            return new AudioObjectPropertySet(children);
        }

        public IAudioObject CreateMixerAudioObject(string audioObjectId, IMessageBroadcaster messageBroadcaster,
            IFactory factory = null)
        {
            factory ??= this;
            return new MixerAudioObject(audioObjectId, messageBroadcaster, factory);
        }

        public IAudioObject CreateEventAudioObject(string audioObjectId, IMessageBroadcaster messageBroadcaster, IFactory factory = null)
        {
            factory ??= this;
            return new EventAudioObject(audioObjectId, messageBroadcaster, factory);
        }

        public IWaapiSession CreateWaapiSession(IFactory factory = null)
        {
            factory ??= this;
            return new WaapiSession(factory);
        }

        public IDisposable CreateConnectionStatePresenter(ITextBehaviour connectionStateTextBehaviour,
            IWaapiSession waapiSession)
        {
            return new ConnectionStatePresenter(connectionStateTextBehaviour, waapiSession);
        }

        public IDisposable CreateMixingSessionHandlerPresenter(IMixingSessionHandlerBehaviour mixingSessionHandlerBehaviour,
            IMixingSessionManager mixingSessionManager)
        {
            return new MixingSessionHandlerPresenter(mixingSessionHandlerBehaviour, mixingSessionManager);
        }

        public IDisposable CreateMixerPresenter(IMixerBehaviour mixerBehaviour,
            IMixingSessionManager mixingSessionManager)
        {
            return new MixerPresenter(mixerBehaviour, mixingSessionManager);
        }

        public IDisposable CreateMixingSessionPresenter(IMixingSessionBehaviour mixingSessionBehaviour, ISliderBehaviour sliderBehaviour, IMixingSession mixingSession)
        {
            return new MixingSessionPresenter(mixingSessionBehaviour, sliderBehaviour, mixingSession);
        }

        public IDisposable CreateTextPresenter(ITextBehaviour textBehaviour, IStringShortenerToggle stringShortenerToggle, IText text)
        {
            return new TextPresenter(textBehaviour, stringShortenerToggle, text);
        }

        public IDisposable CreateSliderPresenter(ISliderBehaviour sliderBehaviour, IValue value)
        {
            return new SliderPresenter(sliderBehaviour, value);
        }

        public IDisposable CreateMaxDistanceSliderPresenter(ISliderBehaviour maxDistanceSliderBehaviour,
            IPositioningPropertySet positioningPropertySet)
        {
            return new MaxDistanceSliderPresenter(maxDistanceSliderBehaviour, positioningPropertySet);
        }

        public IDisposable CreateMaxDistanceTextPresenter(ITextBehaviour maxDistanceTextBehaviour,
            IPositioningPropertySet positioningPropertySet)
        {
            return new MaxDistanceTextPresenter(maxDistanceTextBehaviour, positioningPropertySet);
        }

        public IDisposable CreateTogglePresenter(IToggleBehaviour toggleBehaviour, IToggleable toggle)
        {
            return new TogglePresenter(toggleBehaviour, toggle);
        }

        public IDisposable CreateAttenuationCurvePresenter(
            IAttenuationCurveEditorBehaviour attenuationCurveEditorBehaviour,
            IPositioningPropertySet positioningPropertySet, IFactory factory = null)
        {
            factory ??= this;
            return new AttenuationCurvePresenter(attenuationCurveEditorBehaviour, positioningPropertySet, factory);
        }

        public IDisposable CreateNodeValuePresenter(IAttenuationCurveEditorBehaviour attenuationCurveEditorBehaviour,
            IAttenuationOption attenuationOption, IFactory factory = null)
        {
            factory ??= this;
            return new NodeValuePresenter(attenuationCurveEditorBehaviour, attenuationOption, factory);
        }

        public IDisposable CreateAttenuationCurveEditorHandlerPresenter(
            IAttenuationCurveEditorHandlerBehaviour attenuationCurveEditorHandlerBehaviour,
            IPositioningPropertySet positioningPropertySet)
        {
            return new AttenuationCurveEditorHandlerPresenter(attenuationCurveEditorHandlerBehaviour,
                positioningPropertySet);
        }

        public IDisposable CreateDistanceLinePresenter(IDistanceLineBehaviour distanceLineBehaviour, IStringShortener stringShortener, 
            IPositioningPropertySet positioningPropertySet, IEvent @event)
        {
            return new DistanceLinePresenter(distanceLineBehaviour, stringShortener, positioningPropertySet,  @event);
        }

        public IDisposable CreateAttenuationCurveMessagePresenter(ITextBehaviour textBehaviour, IPositioningPropertySet positioningPropertySet)
        {
            return new AttenuationCurveMessagePresenter(textBehaviour, positioningPropertySet);
        }

        public IDisposable CreateInspectorHandlerPresenter(IInspectorBehaviour inspectorBehaviour,
            IMixingSessionManager mixingSessionManager, IEventManager eventManager, IProjectPropertySet projectPropertySet)
        {
            return new InspectorHandlerPresenter(inspectorBehaviour, mixingSessionManager, eventManager, projectPropertySet);
        }

        public IDisposable CreateMixingSessionDropDownPresenter(IDropDownBehaviour dropDownBehaviour,
            IMixingSessionManager mixingSessionManager)
        {
            return new MixingSessionDropDownPresenter(dropDownBehaviour, mixingSessionManager);
        }

        public IDisposable CreateAttenuationOptionDropdownPresenter(IDropDownBehaviour dropDownBehaviour, IPositioningPropertySet positioningPropertySet,
            IProjectPropertySet projectPropertySet)
        {
            return new AttenuationOptionDropdownPresenter(dropDownBehaviour, positioningPropertySet, projectPropertySet);
        }

        public IDisposable CreateEventSpherePresenter(IEventSphereBehaviour eventSphereBehaviour, IEvent @event)
        {
            return new EventSpherePresenter(eventSphereBehaviour, @event);
        }

        public IDisposable CreateEventSphereHandlerPresenter(IEventSphereHandlerBehaviour eventSphereHandlerBehaviour,
            IEventManager eventManager, IEventFilter eventFilter, IToggleable filterEventSpheresToggleable)
        {
            return new EventSphereHandlerPresenter(eventSphereHandlerBehaviour, eventManager, eventFilter, filterEventSpheresToggleable);
        }

        public IDisposable CreateEventListItemPresenter(IEventListItemBehaviour eventListItemBehaviour, IStringShortenerToggle stringShortenerToggle, IEvent @event)
        {
            return new EventListItemPresenter(eventListItemBehaviour,stringShortenerToggle, @event);
        }

        public IDisposable CreateEventInspectorHandlerPresenter(IEventInspectorHandlerBehaviour eventInspectorHandlerBehaviour,
            IEventManager eventManager)
        {
            return new EventInspectorHandlerPresenter(eventInspectorHandlerBehaviour, eventManager);
        }

        public IDisposable CreateAudioObjectListPresenter(IAudioObjectListBehaviour audioObjectListBehaviour,
            IEvent @event)
        {
            return new AudioObjectListPresenter(audioObjectListBehaviour, @event);
        }

        public IDisposable CreateGhostingPresenter(IDelayedExecutionBehaviour delayedExecutionBehaviour, IEventManager eventManager, float ghostingDuration)
        {
            return new GhostingPresenter(delayedExecutionBehaviour, eventManager, ghostingDuration);
        }

        public IDisposable CreateSourcePositionPresenter(ISourcePosition sourcePosition, IEvent @event)
        {
            return new SourcePositionPresenter(sourcePosition, @event);
        }

        public IDisposable CreateSourceObjectFollowerPresenter(ISourceObjectFollowerBehaviour sourceObjectFollowerBehaviour,
            IEventManager eventManager)
        {
            return new SourceObjectFollowerPresenter(sourceObjectFollowerBehaviour, eventManager);
        }

        public IDisposable CreateEventListHandlerPresenter(IEventListHandlerBehaviour eventListHandlerBehaviour,
            IEventManager eventManager, IEventFilter eventFilter)
        {
            return new EventListHandlerPresenter(eventListHandlerBehaviour, eventManager, eventFilter);
        }

        public IDisposable CreateGridPresenter(IGridBehaviour gridBehaviour, IPositioningPropertySet positioningPropertySet)
        {
            return new GridPresenter(gridBehaviour, positioningPropertySet);
        }

        public IDisposable CreateMixerMessagePresenter(ITextBehaviour errorMessageTextBehaviour,
            IMixingSessionManager mixingSessionManager)
        {
            return new MixerMessagePresenter(errorMessageTextBehaviour, mixingSessionManager);
        }
        
        public IDisposable CreateSphereLabelPresenter(ITextBehaviour textBehaviour, IStringShortenerToggle stringShortenerToggle,
            IEvent @event)
        {
            return new SphereLabelPresenter(textBehaviour, stringShortenerToggle, @event);
        }

        public IDisposable CreateAudioObjectNameTextPresenter(ITextBehaviour textBehaviour,
            IStringShortenerToggle stringShortenerToggle, string audioObjectName)
        {
            return new AudioObjectNameTextPresenter(textBehaviour, stringShortenerToggle, audioObjectName);
        }

        public INodePointConverter CreateNodePointConverter()
        {
            return new CoordinateConverter();
        }

        public IWaapiClient CreateWaapiClient()
        {
            return new WaapiClient();
        }

        public IWaapi CreateWaapi()
        {
            return CreateWaapiClient();
        }

        public IProjectPropertySet CreateProjectPropertySet(IFactory factory = null)
        {
            factory ??= this;
            return new ProjectPropertySet(factory);
        }

        public IText CreateNameText(string id)
        {
            return new NameText(id);
        }

        public IToggle CreateSoloToggle(string id, IToggleMessageChannel toggleMessageChannel, IWaapi waapi = null)
        {
            waapi ??= CreateWaapiClient();
            return new Toggle(waapi, toggleMessageChannel, id, "Solo");
        }

        public IToggle CreateMuteToggle(string id, IToggleMessageChannel toggleMessageChannel, IWaapi waapi = null)
        {
            waapi ??= CreateWaapiClient();
            return new Toggle(waapi, toggleMessageChannel, id, "Mute");
        }

        public IChildSet CreateChildSet(string id, string[] returnRequests, Func<string, IAudioObjectPropertySet[]> postProcess)
        {
            return new ChildSet(id, returnRequests, postProcess);
        }

        public IChildSet CreateEventChildSet(string eventId, int maxNumDescendants)
        {
            return new EventChildSet(eventId, maxNumDescendants);
        }

        public IChildSet CreateAudioObjectChildSet(string audioObjectId, int maxNumDescendants)
        {
            return new AudioObjectChildSet(audioObjectId, maxNumDescendants);
        }

        public IValue CreateVoiceVolumeValue(string id, IWaapi waapi = null)
        {
            waapi ??= CreateWaapiClient();
            return new VoiceVolumeValue(waapi, id);
        }

        public IPositioningPropertySet CreatePositioningPropertySet(string id, IMessageBroadcaster messageBroadcaster, IFactory factory = null)
        {
            return new PositioningPropertySet(id, messageBroadcaster, factory);
        }

        public IAttenuationOptionPropertySet CreateAttenuationOptionPropertySet(string id, string name)
        {
            return new AttenuationOptionPropertySet(id, name);
        }

        public IAttenuationOption CreateAttenuationOption(string id, string name, IAttenuationCurveChangedMessageChannel attenuationCurveChangedMessageChannel)
        {
            return new AttenuationOption(id, name, attenuationCurveChangedMessageChannel);
        }

        public ILoader CreateLoader()
        {
            return new Loader();
        }

        public IInput CreateInput()
        {
            return new Input();
        }

        public IStateChangeNotifier<T> CreateStateChangeNotifier<T>()
        {
            return new StateChangeNotifier<T>();
        }

        public IReset CreateSoloMuteReset(IWaapi waapi = null)
        {
            waapi ??= CreateWaapi();
            return new SoloMuteReset(waapi);
        }

        public INodePositioner CreateNodePositioner()
        {
            return new NodePositioner();
        }

        public IMessageBroadcaster CreateMessageBroadcaster(IWaapi waapi = null)
        {
            waapi ??= CreateWaapiClient();

            return new MessageBroadcaster(waapi);
        }

        public IToggleMessageChannel CreateSoloToggleMessageChannel()
        {
            return new ToggleMessageChannel("Solo");
        }

        public IToggleMessageChannel CreateMuteToggleMessageChannel()
        {
            return new ToggleMessageChannel("Mute");
        }

        public IReferenceChangedMessageChannel CreateAttenuationReferenceChangedMessageChannel()
        {
            return new AttenuationReferenceChangedMessageChannel();
        }

        public IAttenuationCurveChangedMessageChannel CreateAttenuationCurveChangedMessageChannel()
        {
            return new AttenuationCurveChangedMessageChannel();
        }

        public IExecutionLimiter CreateExecutionLimiter(uint ratio)
        {
            return new ExecutionLimiter(ratio);
        }

        public IEvent CreateEvent(EventPropertySet eventPropertySet)
        {
            return new Event(eventPropertySet, this);
        }

        public ICaptureLog CreateCaptureLog()
        {
            return new CaptureLog();
        }

        public IOverrideParent CreateOverridePositioning(string id)
        {
            return new OverridePositioning(id);
        }

        public IClosestTargetSelector CreateClosestTargetFinder()
        {
            return new ClosestTargetSelector();
        }

        public IKeyword CreateKeyword(string name)
        {
            return new Keyword(name);
        }

        public IEventFilter CreateEventFilter(IFilterKeywordSet toggleableFilterKeywordSet, IFilterKeywordSet permanentFilterKeywordSet)
        {
            return new EventFilter(toggleableFilterKeywordSet, permanentFilterKeywordSet);
        }

        public IPausableList<IEvent> CreatePausableEventList()
        {
            return new PausableList<IEvent>();
        }

        public IListManager<IEventListItemBehaviour> CreateEventListManager(IEnumerable<IEventListItemBehaviour> eventListItemBehaviours)
        {
            return new ListManager<IEventListItemBehaviour>(eventListItemBehaviours);
        }

        public IListManager<IAudioObjectListItemBehaviour> CreateAudioObjectListManager(List<IAudioObjectListItemBehaviour> audioObjectListItemBehaviours)
        {
            return new ListManager<IAudioObjectListItemBehaviour>(audioObjectListItemBehaviours);
        }

        public IToggleable CreateToggleValue()
        {
            return new ToggleValue();
        }

        public IEventStateChangeFinder CreateEventStateChangeFinder()
        {
            return new EventStateChangeFinder();
        }

        public ICacheConsumer<IEnumerable<IEvent>> CreateEventCacheConsumer(Action<IEnumerable<IEvent>> action)
        {
            return new CacheConsumer<IEnumerable<IEvent>>(action);
        }

        public IStringShortenerToggle CreateStringShortenerToggle(int maxCharacters)
        {
            return new StringShortenerToggle(maxCharacters);
        }
    }
}
