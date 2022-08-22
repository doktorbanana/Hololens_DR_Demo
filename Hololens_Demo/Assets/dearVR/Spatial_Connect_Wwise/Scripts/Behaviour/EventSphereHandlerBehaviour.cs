using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SpatialConnect.Wwise.Core;

namespace SpatialConnect.Wwise
{
    public class EventSphereHandlerBehaviour : MonoBehaviour, IEventSphereHandlerBehaviour
    {
        [SerializeField] private GameObject eventSpherePrefab = default;
        [SerializeField] private Transform poolTransform = default;

        private const int NUM_INSTANCES_IN_POOL = 64; 
        private readonly Vector3 hidingPosition_ = new Vector3(0, 10000f, 0f);
        private IDisposable eventSphereHandlerPresenter_;
        private IVrInteraction vrInteraction_;
        private IClosestTargetSelector closestTargetSelector_;
        private ICacheConsumer<IEnumerable<IEvent>> eventCacheConsumer_;
        private IEventStateChangeFinder eventStateChangeFinder_;
        private IObjectPool<EventSphereBehaviour> objectPool_;

        public void Init(IVrInteraction vrInteraction, IEventManager eventManager, IUserPreferenceSet userPreferenceSet,
            IEventFilter eventFilter, IFactory factory = null)
        {
            vrInteraction_ = vrInteraction;
            factory ??= new Factory();
            eventSphereHandlerPresenter_ = factory.CreateEventSphereHandlerPresenter(this, eventManager, eventFilter, userPreferenceSet.FilterEventSpheresToggleable);
            closestTargetSelector_ = factory.CreateClosestTargetFinder();
            eventCacheConsumer_ = factory.CreateEventCacheConsumer(ConsumeEvents);
            eventStateChangeFinder_ = factory.CreateEventStateChangeFinder();
            objectPool_ = new ObjectPool<EventSphereBehaviour>(NUM_INSTANCES_IN_POOL, eventSpherePrefab, poolTransform, eventObject =>
            {
                var eventSphereBehaviour = eventObject.GetComponent<EventSphereBehaviour>();
                eventSphereBehaviour.FontSize = userPreferenceSet.EventSphereFontSize;
                eventSphereBehaviour.SphereColor = userPreferenceSet.SphereColor;
            });
            GetComponent<SourceObjectFollowerBehaviour>().Init(eventManager);
            
            void ConsumeEvents(IEnumerable<IEvent> events)
            {
                var changedState = eventStateChangeFinder_.Find(objectPool_.ActiveBehaviours, events);
                foreach (var deadRepresentation in changedState.DeadRepresentations)
                    deadRepresentation.Deactivate();

                foreach (var @event in changedState.NewEvents)
                {
                    var tuple = objectPool_.Get();
                    if (tuple == null)
                    {
                        Debug.LogWarning("EventSphere: no more instances available in the pool");
                        return;
                    }
                
                    var (eventSphere, eventSphereBehaviour) = tuple;
                    eventSphere.SetActive(true);
                    eventSphereBehaviour.Init(vrInteraction_, @event, userPreferenceSet.StringShortenerToggle, closestTargetSelector_);
                    var sourcePosition = @event.SourcePosition;
                    eventSphereBehaviour.Position = new Position(sourcePosition.x, sourcePosition.y, sourcePosition.z);
                }
            }
        }
        
        private void LateUpdate()
        {
            closestTargetSelector_.SelectClosest();

            if (objectPool_.ActiveBehaviours.Any(behaviour => behaviour.Pointed))
                vrInteraction_.LaserPointerBehaviour.EnableHighlight();
            else
                vrInteraction_.LaserPointerBehaviour.DisableHighlight();
            
            eventCacheConsumer_.Consume();
        }
        
        private void OnDestroy()
        {
            eventSphereHandlerPresenter_.Dispose();
        }

        public void UpdateSpheres(IEnumerable<IEvent> events)
        {
            eventCacheConsumer_.Value = events;
        }
        
        public void Show()
        {
            transform.position = Vector3.zero;
        }

        public void Hide()
        {
            transform.position = hidingPosition_;
        }
    }
}
