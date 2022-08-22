using UnityEngine;

namespace SpatialConnect.Wwise
{
    public interface IProjector
    { 
        Vector3 GetProjectedPoint(Vector3 pointPosition);
    }
    
    public class Projector : IProjector
    {
        private readonly Transform planeTransform_;

        public Projector(Transform planeTransform)
        {
            planeTransform_ = planeTransform;
        }
        
        public Vector3 GetProjectedPoint(Vector3 pointPosition) 
        {
            var delta = pointPosition - planeTransform_.position;
            var project = Vector3.Project(delta, planeTransform_.forward.normalized);
            var projectedPoint = pointPosition - project;
            return planeTransform_.InverseTransformPoint(projectedPoint);
        }
    }
}
