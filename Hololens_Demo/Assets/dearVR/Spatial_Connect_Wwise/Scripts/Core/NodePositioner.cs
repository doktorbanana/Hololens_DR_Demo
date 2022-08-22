using System;
using System.Collections.Generic;

namespace SpatialConnect.Wwise.Core
{
    public interface INodePositioner
    {
        Node this[int index] { set; get; }
        Node ApplyNodeConstraint(int index, Node proposedNode);

        List<Node> Nodes { set; get; }

        int? FindNodeAt(Node node);
        void AddNodeAt(Node node);
        void RemoveNodeAt(Node node);

        event Action<List<Node>> NodeChanged;
    }

    public class NodePositioner : INodePositioner
    {
        private const float PROXIMITY_TOLERANCE = 0.03f;
        private const float LINE_TOLERANCE = 0.01f;
        private const float MIN_DISTANCE_BETWEEN_NODES = 0.00001f;

        public NodePositioner()
        {
            Nodes = new List<Node>();
        }
        
        public Node this[int index]
        {
            get => Nodes[index];
            set => Nodes[index] = ApplyNodeConstraint(index, value);
        }
        
        public Node ApplyNodeConstraint(int index, Node proposedNode)
        {
            bool IsNodeFirstOrLast()
            {
                return index == 0 || index == Nodes.Count - 1;
            }

            Node ApplyVerticalConstraint()
            {
                return new Node(Nodes[index].X, proposedNode.Y);
            }

            Node ApplyAdjacentNodesConstraint()
            {
                var leftLimit = Nodes[index - 1].X;
                var rightLimit = Nodes[index + 1].X;

                var x = proposedNode.X;
                if (x <= leftLimit)
                    x = leftLimit + MIN_DISTANCE_BETWEEN_NODES;
                if (rightLimit <= x)
                    x = rightLimit - MIN_DISTANCE_BETWEEN_NODES;
                return new Node(x, proposedNode.Y);
            }

            return IsNodeFirstOrLast() ? ApplyVerticalConstraint() : ApplyAdjacentNodesConstraint();
        }

        public List<Node> Nodes { get; set; }

        public int? FindNodeAt(Node node)
        {
            var index = Nodes.FindIndex(otherNode => otherNode.IsNearTo(node, PROXIMITY_TOLERANCE));
            if (index < 0)
                return null;
            return index;
        }

        public void AddNodeAt(Node node)
        {
            if(Nodes.Count < 2)
                return;

            var index = FindNodeBefore(node.X);
            var leftNode = Nodes[index];
            var rightNode = Nodes[index + 1];
            if (!node.IsBetween(leftNode, rightNode, LINE_TOLERANCE))
                return;

            Nodes.Insert(index+1, node);
            NodeChanged?.Invoke(Nodes);
        }

        private int FindNodeBefore(float x)
        {
            for (var i = 0; i < Nodes.Count - 1; ++i)
            {
                var leftNodeX = Nodes[i].X;
                var rightNodeX = Nodes[i+1].X;
                if (leftNodeX < x && x < rightNodeX)
                    return i;
            }
            throw new ArgumentOutOfRangeException("the x value of the given node should be between 0f and 1f");
        }

        public void RemoveNodeAt(Node node)
        {
            var index = Nodes.FindIndex(otherNode => otherNode.IsNearTo(node, PROXIMITY_TOLERANCE));
            if(index <= 0 || index == (Nodes.Count-1))
                return;

            Nodes.RemoveAt(index);
            NodeChanged?.Invoke(Nodes);
        }
        
        public event Action<List<Node>> NodeChanged;
    }
}
