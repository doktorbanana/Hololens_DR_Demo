using UnityEngine;
using UnityEngine.UI;

namespace SpatialConnect.Wwise
{
    public class DecibelTextBehaviour : MonoBehaviour
    {
        [SerializeField] private Text text = default;

        public void OnValueChanged(float value)
        {
            var formattedString = value.ToString("F1", System.Globalization.CultureInfo.InvariantCulture);
            text.text = formattedString;
        }
    }
}
