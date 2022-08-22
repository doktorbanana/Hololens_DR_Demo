using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SpatialConnect.Wwise
{
    public class GridDropdownContentBehaviour : MonoBehaviour
    {
        private readonly IList<GridDropdownItemBehaviour> itemBehaviours_ = new List<GridDropdownItemBehaviour>();
        private readonly INearestGameObjectFinder nearestGameObjectFinder_ = new NearestGameObjectFinder();
        private readonly IColliderResizer colliderResizer_ = new ColliderResizer();

        private readonly Color normalColor_ = new Color(0.8f, 0.8f, 0.8f);
        private readonly Color highlightColor_ = Color.white;
        
        private IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
            
            colliderResizer_.Fit2dObject(GetComponent<BoxCollider>(), GetComponent<RectTransform>().rect, 100);

            foreach (Transform child in transform)
                itemBehaviours_.Add(child.GetComponent<GridDropdownItemBehaviour>());
        }
        
        private void ResetItemBackground()
        {
            foreach (var item in itemBehaviours_)
                item.BackgroundColor = normalColor_;
        }
        
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.name != "SelectionSphere")
                return;

            var nearest = nearestGameObjectFinder_.Find(other.gameObject, itemBehaviours_.Select(item => item.gameObject));
            var dropdownItem = itemBehaviours_.First(item => item.gameObject == nearest);
            
            HoverStateChanged?.Invoke(itemBehaviours_.IndexOf(dropdownItem));
            ResetItemBackground();
            dropdownItem.BackgroundColor = highlightColor_;
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.name != "SelectionSphere")
                return;

            HoverStateChanged?.Invoke(null);
            ResetItemBackground();
        }

        private void OnDisable()
        {
            ResetItemBackground();
        }

        public event Action<int?> HoverStateChanged;
    }
}
