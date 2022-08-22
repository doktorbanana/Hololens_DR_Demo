using UnityEngine;
using UnityEngine.UI;

namespace SpatialConnect.Wwise
{
    public class GridDropdownItemBehaviour : MonoBehaviour
    {
        [SerializeField] private Text nameText = default;
        [SerializeField] private GameObject outline = default;
        [SerializeField] private Image background = default;
        
        public string NameText
        {
            get => nameText.text;
            set => nameText.text = value;
        }
        
        public bool OutlineActive
        {
            set => outline.SetActive(value);
        }
        
        public Color BackgroundColor
        {
            set => background.color = value;
        }
    }
}
