using System.Collections.Generic;

namespace SpatialConnect.Wwise.Core
{
    public interface INodePointConverter
    {
        List<Node> PointsToNodes(IEnumerable<Point> point, uint maxDistance);

        List<Point> NodesToPoints(IEnumerable<Node> nodes, uint maxDistance);

        Point NodeToPoint(Node node, uint maxDistance);
    }
}
