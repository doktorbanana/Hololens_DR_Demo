using UnityEngine;

namespace SpatialConnect.Wwise
{
    public interface ICrossHairCursorPositioner
    {
        void SetActive(bool active);

        void SetPosition(Vector3 worldPosition);
        
        Vector3 LocalPosition { get; }
    }
    
    public class CrossHairCursorPositioner : ICrossHairCursorPositioner
    {
        private readonly GameObject crossHairCursor_;
        private readonly IProjector projector_;
        
        public CrossHairCursorPositioner(GameObject crossHairCursor, Transform parentTransform)
        {
            crossHairCursor_ = crossHairCursor;
            projector_ = new Projector(parentTransform);
        }
        
        public void SetActive(bool active)
        {
            crossHairCursor_.SetActive(active);
        }

        public void SetPosition(Vector3 worldPosition)
        {
            crossHairCursor_.transform.localPosition = projector_.GetProjectedPoint(worldPosition);
        }

        public Vector3 LocalPosition => crossHairCursor_.transform.localPosition;
    }
}
