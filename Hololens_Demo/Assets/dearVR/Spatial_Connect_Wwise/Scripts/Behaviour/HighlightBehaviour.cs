using UnityEngine;
using UnityEngine.UI;

namespace SpatialConnect.Wwise
{
    public class HighlightBehaviour : MonoBehaviour
    {
        [SerializeField] private Image highlight = default;

        private Color defaultColor_;

        private void Start()
        {
            defaultColor_ = highlight.color;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name != "SelectionSphere")
                return;
            
            highlight.color = Color.white;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.name != "SelectionSphere")
                return;
            
            highlight.color = defaultColor_;
        }
    }
}
