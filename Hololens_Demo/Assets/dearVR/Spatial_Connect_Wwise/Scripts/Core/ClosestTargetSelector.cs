using System.Collections.Generic;
using System.Linq;

namespace SpatialConnect.Wwise.Core
{
    public interface IClosestTargetSelector
    {
        void StartListening(IEventSphereBehaviour eventSphereBehaviour);

        void StopListening(IEventSphereBehaviour eventSphereBehaviour);

        void SelectClosest();
    }
    
    public class ClosestTargetSelector : IClosestTargetSelector
    {
        private class ShotDetail
        {
            public ShotDetail(object target, float distance)
            {
                this.target = target;
                this.distance = distance;
            }
            public readonly object target;
            public readonly float distance;
        }
        
        private readonly List<ShotDetail> shotSpheres_ = new List<ShotDetail>();

        public void StartListening(IEventSphereBehaviour eventSphereBehaviour)
        {
            eventSphereBehaviour.Shot += OnShot;
        }

        public void StopListening(IEventSphereBehaviour eventSphereBehaviour)
        {
            eventSphereBehaviour.Shot -= OnShot;
        }

        public void SelectClosest()
        {
            if (shotSpheres_.Count == 0)
                return;
            
            var closestSphere = (IEventSphereBehaviour) shotSpheres_.Aggregate((current, shotDetail) =>
                current == null || shotDetail.distance < current.distance ? shotDetail : current).target;
            closestSphere.Select();
            shotSpheres_.Clear(); 
        }

        private void OnShot(object eventSphereBehaviour, float distance)
        {
            shotSpheres_.Add(new ShotDetail(eventSphereBehaviour, distance));
        }
    }
}
