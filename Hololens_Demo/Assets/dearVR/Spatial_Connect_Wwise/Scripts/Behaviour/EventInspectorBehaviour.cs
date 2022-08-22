using System;
using SpatialConnect.Wwise.Core;
using UnityEngine;
using UnityEngine.UI;

namespace SpatialConnect.Wwise
{
    public class EventInspectorBehaviour : MonoBehaviour, IEventInspectorHandlerBehaviour
    {
        [SerializeField] private Text eventNameText = default;
        [SerializeField] private AudioObjectListBehaviour audioObjectListBehaviour = default;
       
        private IDisposable eventInspectorHandlerPresenter_;

        public void Init(IEventManager eventManager, IStringShortenerToggle stringShortenerToggle, IFactory factory = null)
        {
            factory ??= new Factory();
            eventInspectorHandlerPresenter_ = factory.CreateEventInspectorHandlerPresenter(this, eventManager);
            audioObjectListBehaviour.Init(stringShortenerToggle);
        }
        
        public void UpdateList(IEvent @event)
        {
            eventNameText.text = @event.Name;
            
            audioObjectListBehaviour.UpdateList(@event);
        }

        private void OnDestroy()
        {
            eventInspectorHandlerPresenter_.Dispose();
        }
    }
}
