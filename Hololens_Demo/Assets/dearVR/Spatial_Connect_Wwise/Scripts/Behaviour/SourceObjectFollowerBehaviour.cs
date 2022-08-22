using System;
using System.Collections.Generic;
using SpatialConnect.Wwise.Core;
using UnityEngine;

namespace SpatialConnect.Wwise
{
    public class SourceObjectFollowerBehaviour : MonoBehaviour, ISourceObjectFollowerBehaviour
    {
        private List<ISourcePosition> sourcePositions_ = new List<ISourcePosition>();

        private IDisposable sourceObjectFollowerPresenter_;
        
        public void Init(IEventManager eventManager, IFactory factory = null)
        {
            factory ??= new Factory();
            sourceObjectFollowerPresenter_ = factory.CreateSourceObjectFollowerPresenter(this, eventManager);
        }
        
        public void Subscribe(IEvent @event)
        {
            sourcePositions_.Add(new SourcePosition(@event));
        }

        public void Unsubscribe(IEvent @event)
        {
            sourcePositions_.RemoveAll(sourcePosition => sourcePosition.PlayingId == @event.PlayingId);
        }
        
        private void Update()
        {
            sourcePositions_.RemoveAll(sourcePosition => !sourcePosition.IsSourceValid);

            foreach(var sourcePosition in sourcePositions_)
                sourcePosition.Update();
        }
    }
}
