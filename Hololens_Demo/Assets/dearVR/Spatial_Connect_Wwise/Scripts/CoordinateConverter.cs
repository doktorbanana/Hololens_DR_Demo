using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SpatialConnect.Wwise.Core;

namespace SpatialConnect.Wwise
{
    public interface INodeVector3Converter
    {
        Node Vector3ToNode(Vector3 position, ref Rect rect);

        Vector3 NodeToVector3(Node node, ref Rect rect);
    }

    public class CoordinateConverter : INodePointConverter, INodeVector3Converter
    {
        private const float MIN = -200f;
        private const float MAX = 0f;
        private const float RANGE = MAX - MIN;
        private const float ABS_MAX = 200f;
        private const float FACTOR = 20f;

        public List<Node> PointsToNodes(IEnumerable<Point> points, uint maxDistance)
        {
            var mD = (float) maxDistance;
            return points.Select(point =>
            {
                var dB = point.y;
                var linear = Mathf.Clamp(Mathf.Pow( 10f, dB / FACTOR ) * ABS_MAX - ABS_MAX, MIN, MAX);
                var nY = (linear - MIN) / RANGE;
                return new Node(point.x / mD, nY);
            }).ToList();
        }

        public List<Point> NodesToPoints(IEnumerable<Node> nodes, uint maxDistance)
        {
            return nodes.Select(normalized => NodeToPoint(normalized, maxDistance)).ToList();
        }

        public Point NodeToPoint(Node node, uint maxDistance)
        {
            var mD = (float) maxDistance;
            var nY = node.Y;
            var linear = nY * RANGE + MIN;
            var dB = Mathf.Clamp(FACTOR * Mathf.Log10((linear + ABS_MAX) / ABS_MAX), MIN, MAX);
            return new Point(node.X * mD, dB);
        }

        public Node Vector3ToNode(Vector3 position, ref Rect rect)
        {
            var x = (position.x - rect.x) / rect.width;
            var y = (position.y - rect.y) / rect.height;
            return new Node(x, Mathf.Clamp(y, 0f, 1f));
        }

        public Vector3 NodeToVector3(Node node, ref Rect rect)
        {
            return new Vector3(node.X * rect.width, node.Y * rect.height, -1);
        }
    }
}
