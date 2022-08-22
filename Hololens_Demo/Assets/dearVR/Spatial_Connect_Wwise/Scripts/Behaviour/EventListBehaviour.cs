using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SpatialConnect.Wwise.Core;

namespace SpatialConnect.Wwise
{
    public class EventListBehaviour : MonoBehaviour, IEventListHandlerBehaviour
    {
        [SerializeField] private GameObject eventListItemPrefab = default;
        [SerializeField] private Transform eventListItemsParentTransform = default;
        
        private const int MAX_NUMBER_OF_INSTANCES = 25;
        private const float ROW_HEIGHT = 30f;

        private IFactory factory_;
        private IDisposable eventListHandlerPresenter_;
        private IListManager<IEventListItemBehaviour> listManager_;
        private IObjectPool<EventListItemBehaviour> objectPool_;
        private IListTarget listTarget_;
        private IInputBehaviour inputBehaviour_;
        private ICacheConsumer<IEnumerable<IEvent>> eventCacheConsumer_;
        private IEventStateChangeFinder eventStateChangeFinder_;

        public void Init(IEventManager eventManager, IEventFilter eventFilter, IStringShortenerToggle stringShortenerToggle, IFactory factory = null)
        {
            factory_ = factory?? new Factory();
            eventListHandlerPresenter_ = factory_.CreateEventListHandlerPresenter(this, eventManager, eventFilter);
            listManager_ = factory_.CreateEventListManager(new List<IEventListItemBehaviour>());
            eventCacheConsumer_ = factory_.CreateEventCacheConsumer(ConsumeEvents);
            objectPool_ = new ObjectPool<EventListItemBehaviour>(MAX_NUMBER_OF_INSTANCES, eventListItemPrefab, eventListItemsParentTransform);
            listTarget_ = new ListTarget(new Projector(transform), ROW_HEIGHT);
            eventStateChangeFinder_ = new EventStateChangeFinder();

            void ConsumeEvents(IEnumerable<IEvent> fullEvents)
            {
                var events = fullEvents.ToList();
                var changedState = eventStateChangeFinder_.Find(objectPool_.ActiveBehaviours, events);
                foreach (var deadRepresentation in changedState.DeadRepresentations)
                    deadRepresentation.Deactivate();

                foreach (var newEvent in changedState.NewEvents)
                {
                    var (eventListItem, eventListItemBehaviour) = objectPool_.Get();
                    eventListItem.SetActive(true);
                    eventListItemBehaviour.Init(newEvent, stringShortenerToggle);
                }

                var activeBehaviours = objectPool_.ActiveBehaviours.ToList();
                activeBehaviours.Reverse();
                foreach (var @event in events)
                {
                    var targetBehaviour = activeBehaviours.First(activeBehaviour => activeBehaviour.PlayingId == @event.PlayingId);
                    var indexOf = activeBehaviours.IndexOf(targetBehaviour);
                    targetBehaviour.transform.localPosition = new Vector3(0f, -indexOf * ROW_HEIGHT, 0f);
                }

                listManager_ = factory_.CreateEventListManager(activeBehaviours);
            }
        }

        public void UpdateList(IEnumerable<IEvent> events)
        {
            eventCacheConsumer_.Value = events;
        }

        private void LateUpdate()
        {
            eventCacheConsumer_.Consume();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.name != "SelectionSphere")
                return;
            
            inputBehaviour_ = other.GetComponent<InputBehaviour>();
            inputBehaviour_.StandardInteractionStateChanged += OnStandardInteractionStateChanged;
        }
        
        private void OnTriggerStay(Collider other)
        {
            if (other.name != "SelectionSphere")
                return;
            
            listManager_.Highlight(listTarget_.UpdateIndex(other.transform.position));
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.name != "SelectionSphere")
                return;

            inputBehaviour_.StandardInteractionStateChanged -= OnStandardInteractionStateChanged;
            listManager_.Highlight(null);
        }
        
        private void OnStandardInteractionStateChanged(bool pressed)
        {
            if (!pressed)
                return;
            listManager_.Select(listTarget_.Index);
        }
        
        private void OnDestroy()
        {
            eventListHandlerPresenter_.Dispose();
        }
    }
}
