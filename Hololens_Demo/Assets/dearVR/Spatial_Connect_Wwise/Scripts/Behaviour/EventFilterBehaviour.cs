using UnityEngine;
using SpatialConnect.Wwise.Core;

namespace SpatialConnect.Wwise
{
    public class EventFilterBehaviour : MonoBehaviour
    {
        [SerializeField] private GameObject filterTogglePrefab = default;
        [SerializeField] private GameObject filterToggleParent = default;

        public void Init(IFilterKeywordSet filterKeywordSet)
        {
            foreach(var keyword in filterKeywordSet.Keywords)
            {
                if (keyword.Name == "")
                    continue;
                
                var toggle = Instantiate(filterTogglePrefab, filterToggleParent.transform);
                toggle.GetComponent<FilterToggleBehaviour>().Init(keyword);
            }
        }
    }
}
