using UnityEngine;

namespace SpatialConnect.Wwise
{
    public static class LabelPositioner
    {
        private const float THRESHOLD = 15f;
        private const float SCALE_RATIO = 0.1f;
        private const float VERTICAL_OFFSET = 0.45f;
        
        public static (Vector3 labelPosition, Vector3 labelScale) Place(Vector3 cameraPosition, Vector3 objectPosition)
        {
            var distance = Vector3.Distance(objectPosition, cameraPosition);
            return distance > THRESHOLD ? ProcessFarObject() : ProcessNearObject();

            (Vector3 labelPosition, Vector3 labelScale) ProcessFarObject()
            {
                var labelPosition = (objectPosition - cameraPosition).normalized * THRESHOLD + cameraPosition;
                labelPosition.y += VERTICAL_OFFSET;
                return (labelPosition, Vector3.one);
            }

            (Vector3 labelPosition, Vector3 labelScale) ProcessNearObject()
            {
                var labelPosition = objectPosition;
                labelPosition.y += VERTICAL_OFFSET;
                var scale = 1f - (1f - distance / THRESHOLD) * SCALE_RATIO;
                return (labelPosition, new Vector3(scale, scale, 1f));
            }
        }
    }
}
