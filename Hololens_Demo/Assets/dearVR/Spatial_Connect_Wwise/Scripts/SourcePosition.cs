using System;
using SpatialConnect.Wwise.Core;
using UnityEditor;
using UnityEngine;

namespace SpatialConnect.Wwise
{
    public class SourcePosition : ISourcePosition
    {
        private Position previousPosition_;
        private readonly Transform transform_;
        private IDisposable sourcePositionPresenter_;

        public uint PlayingId { get; }
        
        public SourcePosition(IEvent @event, IFactory factory = null)
        {
            factory ??= new Factory();
            PlayingId = @event.PlayingId;
            sourcePositionPresenter_ = factory.CreateSourcePositionPresenter(this, @event);
            transform_= ((GameObject) EditorUtility.InstanceIDToObject(@event.GameObjectId)).transform;
        }

        public bool IsSourceValid => transform_ != null;

        public void Update()
        {
            var currentPosition = transform_.position;
            if (previousPosition_.x != currentPosition.x ||
                previousPosition_.y != currentPosition.y ||
                previousPosition_.z != currentPosition.z)
            {
                previousPosition_ = new Position(currentPosition.x, currentPosition.y, currentPosition.z);
                PositionUpdated?.Invoke(previousPosition_);
            }
        }

        public event Action<Position> PositionUpdated;
    }
}
