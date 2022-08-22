using UnityEngine;

namespace SpatialConnect.Wwise
{
    public class SelectionOutlineBehaviour : MonoBehaviour
    {
        [SerializeField] private RectTransform leftRectTransform = default;
        [SerializeField] private RectTransform rightRectTransform = default;
        [SerializeField] private RectTransform topRectTransform = default;
        [SerializeField] private RectTransform bottomRectTransform = default;

        private const float LINE_WIDTH = 2f;

        public void Init(RectTransform parentRectTransform, float rowHeight)
        {
            var parentRect = parentRectTransform.rect;
            
            SetBoundary(leftRectTransform, new Rect(0, rowHeight, LINE_WIDTH, rowHeight));
            SetBoundary(rightRectTransform, new Rect(parentRect.width, rowHeight, LINE_WIDTH, rowHeight));
            SetBoundary(topRectTransform.GetComponent<RectTransform>(), new Rect(0, rowHeight, parentRect.width, LINE_WIDTH));
            SetBoundary(bottomRectTransform.GetComponent<RectTransform>(), new Rect(0, 0, parentRect.width, LINE_WIDTH));

            void SetBoundary(RectTransform target, Rect rect)
            {
                target.anchoredPosition = new Vector2(rect.x, rect.y);
                target.sizeDelta = new Vector2(rect.width, rect.height);
            }
        }
    }
}
