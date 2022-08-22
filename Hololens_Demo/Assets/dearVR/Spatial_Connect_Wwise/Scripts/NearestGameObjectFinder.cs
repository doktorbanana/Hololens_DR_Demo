using System.Collections.Generic;
using UnityEngine;

namespace SpatialConnect.Wwise
{
    public interface INearestGameObjectFinder
    {
        GameObject Find(GameObject reference, IEnumerable<GameObject> candidates);
    }

    public class NearestGameObjectFinder : INearestGameObjectFinder
    {
        public GameObject Find(GameObject reference, IEnumerable<GameObject> candidates)
        {
            GameObject nearest = default;

            foreach(var candidate in candidates)
            {
                if(nearest == null)
                {
                    nearest = candidate;
                }
                else
                {
                    var distanceToCandidate = Vector3.Distance(candidate.transform.position, reference.transform.position);
                    var distanceToNearest = Vector3.Distance(nearest.transform.position, reference.transform.position);

                    if (distanceToCandidate < distanceToNearest)
                    {
                        nearest = candidate;
                    }
                }

            }

            return nearest;
        }
    }

}
