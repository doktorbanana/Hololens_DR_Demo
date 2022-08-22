using System;
using UnityEngine;
using UnityEngine.UI;
using SpatialConnect.Wwise.Core;

namespace SpatialConnect.Wwise
{
    [RequireComponent(typeof(Image))]
    public class EventListItemBehaviour : MonoBehaviour, IEventListItemBehaviour
    {
        [SerializeField] private Text eventNameText = default;
        [SerializeField] private Text gameObjectNameText = default;
        [SerializeField] private GameObject outline = default;

        private IDisposable eventListItemPresenter_;

        private Image backgroundImage_;
        private Color backgroundColor_;
        private readonly Color defaultColor_ = new Color(0.8313f, 0.8313f, 0.8313f, 1f);

        public uint PlayingId { get; private set; }

        public bool HighlightActive
        {
            set => backgroundImage_.color = value ? Color.white : backgroundColor_;
        }
        
        public string EventName
        {
            set => eventNameText.SetTextWithEllipsis(value);
        }

        public string GameObjectName
        {
            set => gameObjectNameText.SetTextWithEllipsis(value);
        }
        
        public void TurnIntoGhost()
        {
            backgroundColor_ = Color.grey;
            backgroundImage_.color = backgroundColor_;
        }

        public bool OutlineActive
        {
            set => outline.SetActive(value);
        }

        public void Init(IEvent @event, IStringShortenerToggle stringShortenerToggle, IFactory factory = null)
        {
            factory ??= new Factory();

            backgroundColor_ = defaultColor_;
            backgroundImage_ = GetComponent<Image>();
            backgroundImage_.color = backgroundColor_; ;

            eventListItemPresenter_ = factory.CreateEventListItemPresenter(this, stringShortenerToggle, @event);
            PlayingId = @event.PlayingId;
        }

        public void Select()
        {
            Selected?.Invoke();
        }

        private void OnDisable()
        {
            eventListItemPresenter_.Dispose();
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }
        
        public event Action Selected;
    }
}
