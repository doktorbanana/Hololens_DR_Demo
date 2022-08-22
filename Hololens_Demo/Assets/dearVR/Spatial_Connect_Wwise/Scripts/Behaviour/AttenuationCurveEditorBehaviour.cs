using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SpatialConnect.Wwise.Core;

namespace SpatialConnect.Wwise
{
    public class AttenuationCurveEditorBehaviour : MonoBehaviour, IAttenuationCurveEditorBehaviour
    {
        [SerializeField] private GameObject nodePrefab = default;
        [SerializeField] private uint updateRatePerSec = 12;

        [SerializeField] private SliderBehaviour maxDistanceSliderBehaviour = default;
        [SerializeField] private TextBehaviour maxDistanceSliderTextBehaviour = default;
        [SerializeField] private TextBehaviour attenuationCurveTextBehaviour = default;
        [SerializeField] private DistanceLineBehaviour distanceLineBehaviour = default;
        
        private GameObject selectionSphere_;
        private int? draggedNode_;
        private int? hoveredNode_;

        private IAttenuationCurveRenderer attenuationCurveRenderer_;
        private INodeLabel nodeLabelRenderer_;

        private ICrossHairCursorPositioner crossHairCursorPositioner_;
        private INodePositioner nodePositioner_;
        private INodeVector3Converter nodeVector3Converter_;
        private readonly IAutoDisposer attenuationCurvePresenter_ = new AutoDisposer();
        private readonly IAutoDisposer nodeValuePresenter_ = new AutoDisposer();
        private IExecutionLimiter executionLimiter_;
        private Action unsubscribeFromInputBehaviourAction_;
        private readonly IFactory factory_ = new Factory();
        private Rect editorRect_;

        public event Action<Node?> HoverStateUpdated;

        private void OnValidate()
        {
            updateRatePerSec = updateRatePerSec == 0 ? 1 : updateRatePerSec;
        }

        private void Awake()
        {
            var crossHairCursor = transform.Find("CrossHair").gameObject;
            var nodeParentTransform = transform.Find("Nodes");
            var nodeLabel = transform.Find("NodeLabel").gameObject;
            var lineRenderer = nodeParentTransform.GetComponent<LineRenderer>();

            attenuationCurveRenderer_ = new AttenuationCurveRenderer(nodeParentTransform, lineRenderer, nodePrefab);
            nodeLabelRenderer_ = new NodeLabel(nodeLabel);
            crossHairCursorPositioner_ = new CrossHairCursorPositioner(crossHairCursor, transform);
            nodeVector3Converter_ = new CoordinateConverter();
            editorRect_ = GetComponent<RectTransform>().rect;

            executionLimiter_ = factory_.CreateExecutionLimiter(updateRatePerSec);
            nodePositioner_ = factory_.CreateNodePositioner();
            nodePositioner_.NodeChanged += nodes => EditorChanged?.Invoke(nodes);
        }

        public void Init(IStringShortener stringShortener, IDistanceLinePropertySet distanceLinePropertySet, IPositioningPropertySet positioningPropertySet, IFactory factory= null)
        {
            factory ??= new Factory();
            
            maxDistanceSliderBehaviour.Init(factory.CreateMaxDistanceSliderPresenter(maxDistanceSliderBehaviour, positioningPropertySet));

            maxDistanceSliderTextBehaviour.Init(factory.CreateMaxDistanceTextPresenter(maxDistanceSliderTextBehaviour, positioningPropertySet));
            
            attenuationCurveTextBehaviour.Init(factory.CreateAttenuationCurveMessagePresenter(attenuationCurveTextBehaviour, positioningPropertySet));
            
            distanceLineBehaviour.Init(stringShortener, distanceLinePropertySet, positioningPropertySet);
            
            attenuationCurvePresenter_.Update(factory_.CreateAttenuationCurvePresenter(this, positioningPropertySet));

            nodeValuePresenter_.Update(factory_.CreateNodeValuePresenter(this, positioningPropertySet.AttenuationOption));

            GetComponent<GridBehaviour>().Init(positioningPropertySet);
        }

        public List<Node> Nodes
        {
            set
            {
                nodePositioner_.Nodes = value;
                if (attenuationCurveRenderer_.NodeCount == nodePositioner_.Nodes.Count)
                    UpdateNodePosition();
                else
                    RefreshAllNodes();

                void UpdateNodePosition()
                {
                    for (var i = 0; i < nodePositioner_.Nodes.Count; ++i)
                    {
                        attenuationCurveRenderer_.MoveNode(i,
                            nodeVector3Converter_.NodeToVector3(nodePositioner_[i], ref editorRect_));
                    }
                }

                void RefreshAllNodes()
                {
                    attenuationCurveRenderer_.Clear();
                    attenuationCurveRenderer_.NodePositions
                        = nodePositioner_.Nodes.Select(
                            node => nodeVector3Converter_.NodeToVector3(node, ref editorRect_)).ToList();
                }
            }
        }

        public string NodeHoverText
        {
            set
            {
                if(hoveredNode_.HasValue)
                    nodeLabelRenderer_.ShowLabelAt(nodeVector3Converter_.NodeToVector3(nodePositioner_[hoveredNode_.Value], ref editorRect_), value);
                else
                    nodeLabelRenderer_.Hide();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name != "SelectionSphere")
                return;

            crossHairCursorPositioner_.SetActive(true);

            selectionSphere_ = other.gameObject;
            IInputBehaviour inputBehaviour = selectionSphere_.GetComponent<InputBehaviour>();
            inputBehaviour.StandardInteractionStateChanged += OnStandardInteractionStateChanged;
            inputBehaviour.AddNodeInteractionStateChanged += OnAddNodeInteractionStateChanged;
            inputBehaviour.RemoveNodeInteractionStateChanged += OnRemoveNodeInteractionStateChanged;

            unsubscribeFromInputBehaviourAction_ = () =>
            {
                inputBehaviour.StandardInteractionStateChanged -= OnStandardInteractionStateChanged;
                inputBehaviour.AddNodeInteractionStateChanged -= OnAddNodeInteractionStateChanged;
                inputBehaviour.RemoveNodeInteractionStateChanged -= OnRemoveNodeInteractionStateChanged;
            };
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.name != "SelectionSphere")
                return;

            crossHairCursorPositioner_.SetActive(false);
            nodeLabelRenderer_.Hide();
            OnStandardInteractionStateChanged(false);
            unsubscribeFromInputBehaviourAction_.Invoke();
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.name != "SelectionSphere")
                return;

            crossHairCursorPositioner_.SetPosition(selectionSphere_.transform.position);
            var normalizedCursorPosition = nodeVector3Converter_.Vector3ToNode(crossHairCursorPositioner_.LocalPosition, ref editorRect_);

            if(draggedNode_.HasValue)
            {
                executionLimiter_.TryExecute(() =>
                {
                    nodePositioner_[draggedNode_.Value] = normalizedCursorPosition;
                    EditorChanged?.Invoke(nodePositioner_.Nodes);
                });

                hoveredNode_ = draggedNode_;
            }
            else
            {
                hoveredNode_ = nodePositioner_.FindNodeAt(normalizedCursorPosition);
            }

            HoverStateUpdated?.Invoke(hoveredNode_.HasValue ? (Node?)nodePositioner_[hoveredNode_.Value] : null);
        }

        private void OnStandardInteractionStateChanged(bool state)
        {
            if (draggedNode_.HasValue && !state)
            {
                nodePositioner_[draggedNode_.Value] =
                    nodeVector3Converter_.Vector3ToNode(crossHairCursorPositioner_.LocalPosition, ref editorRect_);
                EditorChanged?.Invoke(nodePositioner_.Nodes);
            }
            draggedNode_ = state ?
                nodePositioner_.FindNodeAt(
                    nodeVector3Converter_.Vector3ToNode(crossHairCursorPositioner_.LocalPosition, ref editorRect_)) : null;
        }

        private void OnAddNodeInteractionStateChanged(bool state)
        {
            if (state)
                nodePositioner_.AddNodeAt(
                    nodeVector3Converter_.Vector3ToNode(crossHairCursorPositioner_.LocalPosition, ref editorRect_));
        }

        private void OnRemoveNodeInteractionStateChanged(bool state)
        {
            if (state)
                nodePositioner_.RemoveNodeAt(
                    nodeVector3Converter_.Vector3ToNode(crossHairCursorPositioner_.LocalPosition, ref editorRect_));
        }

        private void OnDestroy()
        {
            unsubscribeFromInputBehaviourAction_?.Invoke();
            attenuationCurvePresenter_.Dispose();
            nodeValuePresenter_.Dispose();
        }

        public event Action<List<Node>> EditorChanged;
    }
}
