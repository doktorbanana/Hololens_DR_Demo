using UnityEngine;

namespace SpatialConnect.Wwise
{
    public interface IListTarget
    {
        int Index { get; }

        int UpdateIndex(Vector3 selectionSpherePosition);
    }
    
    public class ListTarget : IListTarget
    {
        private readonly IProjector projector_;
        private readonly float rowHeight_;

        public int Index { get; private set; }

        public ListTarget(IProjector projector, float rowHeight)
        {
            projector_ = projector;
            rowHeight_ = rowHeight;
        }

        public int UpdateIndex(Vector3 selectionSpherePosition)
        {
            Index = (int) (-projector_.GetProjectedPoint(selectionSpherePosition).y / rowHeight_);
            return Index;
        }
    }
}
