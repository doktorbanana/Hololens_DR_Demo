using UnityEngine;
using UnityEngine.UI;

namespace SpatialConnect.Wwise
{
    [RequireComponent(typeof(LineRenderer))]
    public class GridLineBehaviour : MonoBehaviour
    {
        [SerializeField] private LineRenderer lineRenderer = default;
        [SerializeField] private Text label = default;
        [SerializeField] private RectTransform labelRectTransform = default;
        
        public void SetVertices(Vector3 start, Vector3 end)
        {
            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, end);
        }

        public void DisableLine()
        {
            lineRenderer.enabled = false;
        }

        public Vector3 LabelPosition
        {
            set => labelRectTransform.localPosition = value;
        }

        public string Label
        {
            set => label.text = value;
        }
    }
}
