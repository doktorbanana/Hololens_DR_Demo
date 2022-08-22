using System;

namespace SpatialConnect.Wwise.Core
{
    public readonly struct Node
    {
        public Node(float x = 0f, float y = 0f)
        {
            X = x;
            Y = y;
        }

        public float Distance(Node other)
        {
            var dif = other - this;
            return (float)Math.Sqrt(dif.X * dif.X + dif.Y * dif.Y);
        }

        public bool IsNearTo(Node node, float tolerance)
        {
            var dist = Distance(node);
            return dist <= tolerance;
        }

        public bool IsBetween(Node nodeA, Node nodeB, float tolerance)
        {
            var vectorAToThis = nodeA - this;
            var vectorAToB = nodeB - nodeA;
            var crossProduct = vectorAToThis.Cross(vectorAToB);
            return Math.Abs(crossProduct) <= tolerance;
        }

        private float Cross(Node vector)
        {
            return X * vector.Y - Y * vector.X;
        }

        public static Node operator +(Node a, Node b) => new Node(a.X + b.X, a.Y + b.Y);
        public static Node operator -(Node a, Node b) => new Node(a.X - b.X, a.Y - b.Y);

        public readonly float X;
        public readonly float Y;
    }
}
