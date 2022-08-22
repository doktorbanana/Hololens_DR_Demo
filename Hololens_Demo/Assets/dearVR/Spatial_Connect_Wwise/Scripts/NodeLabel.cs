using UnityEngine;
using UnityEngine.UI;

namespace SpatialConnect.Wwise
{
    public interface INodeLabel
    {
        void ShowLabelAt(Vector3 position, string text);

        void Hide();
    }

    public class NodeLabel : INodeLabel
    {
        private const int HEIGHT_OFFSET = 30;

        private readonly GameObject nodeLabelGameObject_;

        public NodeLabel(GameObject nodeLabelGameObject)
        {
            nodeLabelGameObject_ = nodeLabelGameObject;
        }

        public void ShowLabelAt(Vector3 position, string text)
        {
            nodeLabelGameObject_.SetActive(true);

            var positionWithOffset = position;
            positionWithOffset.y += HEIGHT_OFFSET;
            nodeLabelGameObject_.transform.localPosition = positionWithOffset;
            nodeLabelGameObject_.GetComponent<Text>().text = text;
        }

        public void Hide()
        {
            nodeLabelGameObject_.SetActive(false);
        }
    }
}
