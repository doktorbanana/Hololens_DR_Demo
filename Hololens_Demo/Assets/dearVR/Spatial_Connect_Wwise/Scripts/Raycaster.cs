using UnityEngine;
using SpatialConnect.Wwise.Core;

namespace SpatialConnect.Wwise
{
    public class Raycaster : IRaycaster
    {
        private readonly Transform rayOrigin_;
        private readonly Transform target_;
        private readonly float targetRadius_;
        
        public Raycaster(Transform rayOrigin, Transform target, float targetRadius)
        {
            rayOrigin_ = rayOrigin;
            target_ = target;
            targetRadius_ = targetRadius;
        }
        
        public RaycastResult Check()
        {
            var rayOriginPosition = rayOrigin_.position;
            var rayDirection = rayOrigin_.forward;
            var targetPosition = target_.position;
            
            var distanceToSphere = rayOriginPosition - targetPosition;
            var b= Vector3.Dot( rayDirection, distanceToSphere );
            var c= Vector3.Dot( distanceToSphere, distanceToSphere ) - targetRadius_ * targetRadius_;
            var bSquaredMinusC = b * b - c;
        
            if (bSquaredMinusC <= 0.0)
                return new RaycastResult(false);
        
            var tA= -b + Mathf.Sqrt( bSquaredMinusC );
            var tB= -b - Mathf.Sqrt( bSquaredMinusC );
            if ( tA < 0.0 ) 
            {
                if ( tB < 0.0 )
                    return new RaycastResult(false);
            } 
            else if ( tB < 0.0 )
                return new RaycastResult(true, tA);

            return new RaycastResult(true, tB);
        }
    }
}
