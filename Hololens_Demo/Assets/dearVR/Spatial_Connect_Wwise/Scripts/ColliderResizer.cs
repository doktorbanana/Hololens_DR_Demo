using UnityEngine;

namespace SpatialConnect.Wwise
{
    public interface IColliderResizer
    {
        void Fit2dObject(BoxCollider boxCollider, Rect rect, float thickness = 0.0f);
    }

    public class ColliderResizer : IColliderResizer
    {
        public void Fit2dObject(BoxCollider boxCollider, Rect rect, float thickness)
        {
            var height = rect.height;
            var width = rect.width;

            var colCenter = boxCollider.center;
            colCenter.y = -(rect.size.y/2);
            boxCollider.center = colCenter;

            boxCollider.size = new Vector3(width, height, thickness);
        }
    }
}
