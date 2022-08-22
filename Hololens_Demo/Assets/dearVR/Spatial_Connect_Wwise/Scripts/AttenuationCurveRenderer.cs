using System.Collections.Generic;
using UnityEngine;

namespace SpatialConnect.Wwise
{
    public interface IAttenuationCurveRenderer
    {
        void Clear();

        List<Vector3> NodePositions { set; }

        int NodeCount { get; }
        
        void MoveNode(int index, Vector3 newPosition);
    }
    
    public class AttenuationCurveRenderer : IAttenuationCurveRenderer
    {
        private readonly Transform nodeParentTransform_;
        private readonly LineRenderer lineRenderer_;
        private readonly GameObject nodePrefab_;
        
        private readonly Vector3 Z_OFFSET = new Vector3(0f, 0f, 0.2f);

        public AttenuationCurveRenderer(Transform nodeParenTransform, 
            LineRenderer lineRenderer, GameObject nodePrefab)
        {
            nodeParentTransform_ = nodeParenTransform;
            lineRenderer_ = lineRenderer;
            nodePrefab_ = nodePrefab;
        }
        
        public void Clear()
        {
            foreach (Transform child in nodeParentTransform_)
                Object.Destroy(child.gameObject);
        }

        public List<Vector3> NodePositions
        {
            set 
            {
                lineRenderer_.positionCount = value.Count;
                for (var i = 0; i < value.Count; ++i)
                {
                    var nodeObject = Object.Instantiate(nodePrefab_, nodeParentTransform_);
                    var nodePosition = value[i];
                    nodeObject.transform.localPosition = nodePosition;
                    lineRenderer_.SetPosition(i, nodePosition + Z_OFFSET);
                }
            }
        }

        public int NodeCount => lineRenderer_.positionCount;

        public void MoveNode(int index, Vector3 newPosition)
        {
            var draggedNode = nodeParentTransform_.GetChild(index);
            draggedNode.localPosition = newPosition;
            lineRenderer_.SetPosition(index, newPosition + Z_OFFSET);
        }
    }
}
