using System;
using SpatialConnect.Wwise.Core;
using UnityEngine;
using UnityEngine.UI;

namespace SpatialConnect.Wwise
{
    public class AudioObjectListItemBehaviour : MonoBehaviour, IAudioObjectListItemBehaviour
    {
        [SerializeField] private TextBehaviour textBehaviour = default;
        [SerializeField] private RectTransform rectTransform = default;
        [SerializeField] private RectTransform textRectTransform = default;
        [SerializeField] private GameObject outline = default;
        [SerializeField] private SelectionOutlineBehaviour selectionOutlineBehaviour = default;
        
        private Image image_;
        private Color defaultColor_;
        private CanvasGroup canvasGroup_;
        private const float ROW_WIDTH = 450f;
        private const float TEXT_MARGIN = 10f;
        private const float TEXT_WIDTH = ROW_WIDTH - TEXT_MARGIN;
        private const float ROW_HEIGHT = 30f;
        private const float INDENT = 24f;
        private IDisposable audioObjectListItemPresenter_;

        public int[] Index { get; private set; }

        public bool HighlightActive
        {
            set => image_.color = value ? Color.white : defaultColor_;
        }

        public bool OutlineActive
        {
            set => outline.SetActive(value);
        }

        public void Init(string audioObjectName, int[] index, uint row, Transform listTransform, IStringShortenerToggle stringShortenerToggle, IFactory factory = null)
        {
            factory ??= new Factory();
            Index = index;

            var marginFromLeft = INDENT * (index.Length - 1);
            var localPositionInList = new Vector3(marginFromLeft, -row * ROW_HEIGHT, 0f); 
            transform.position = listTransform.TransformPoint(localPositionInList);
            rectTransform.sizeDelta = new Vector2(ROW_WIDTH - marginFromLeft, ROW_HEIGHT);
            textRectTransform.sizeDelta = new Vector2(TEXT_WIDTH - marginFromLeft, ROW_HEIGHT);
            selectionOutlineBehaviour.Init(rectTransform, ROW_HEIGHT);
            audioObjectListItemPresenter_ = factory.CreateAudioObjectNameTextPresenter(textBehaviour, stringShortenerToggle, audioObjectName);
            canvasGroup_.alpha = 1;
        }

        public void Select()
        {
            Selected?.Invoke(Index);
        }
        
        private void Start()
        {
            canvasGroup_ = GetComponent<CanvasGroup>();
            image_ = GetComponent<Image>();
            defaultColor_ = image_.color;
        }

        private void OnDestroy()
        {
            audioObjectListItemPresenter_?.Dispose();
        }

        public event Action<int[]> Selected;
    }
}
