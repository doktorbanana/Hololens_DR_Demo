using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SpatialConnect.Wwise.Core;

namespace SpatialConnect.Wwise
{
    public class GridDropdownBehaviour : MonoBehaviour, IDropDownBehaviour
    {
        [SerializeField] private GameObject itemPrefab = default;
        [SerializeField] private Transform itemsParentTransform = default;
        [SerializeField] private GameObject notInteractablePanel = default;
        [SerializeField] private Text selectionText = default;
        [SerializeField] private GridDropdownContentBehaviour gridDropdownContentBehaviour = default;
        
        private IInputBehaviour inputBehaviour_;
        private BoxCollider boxCollider_;
        private Action onRelease_;

        private bool buttonDown_;
        private int? hoverState_;

        private IList<GridDropdownItemBehaviour> itemsBehaviours_;
        private IObjectPool<GridDropdownItemBehaviour> objectPool_;
        
        public string[] Items
        {
            set
            {
                itemsBehaviours_.Clear();
                
                foreach (Transform itemTransform in itemsParentTransform)
                    itemTransform.gameObject.SetActive(false);
                
                foreach (var itemName in value)
                {
                    var pooledItem = objectPool_.Get();
                    if (pooledItem == null)
                    {
                        Debug.LogWarning("Dropdown: No more instances available in the pool!");
                        return;
                    }
                    
                    var (dropdownItem, dropdownItemBehaviour) = pooledItem;
                    dropdownItem.SetActive(true);
                    dropdownItemBehaviour.NameText = itemName;
                    itemsBehaviours_.Add(dropdownItemBehaviour);
                }
            }
        }
        
        public int Selection
        {
            set
            {
                foreach (var item in itemsBehaviours_) 
                    item.OutlineActive = false;
                
                itemsBehaviours_[value].OutlineActive = true;
                selectionText.text = itemsBehaviours_[value].NameText;
            }
        }
        
        private void Awake()
        {
            boxCollider_ = GetComponent<BoxCollider>();
        }
        
        public void Init(uint instancesCount)
        {
            objectPool_ = new ObjectPool<GridDropdownItemBehaviour>(instancesCount, itemPrefab, itemsParentTransform);
            itemsBehaviours_ = new List<GridDropdownItemBehaviour>();
        }
        
        public void EnableDropdown()
        {
            boxCollider_.enabled = true;
            notInteractablePanel.SetActive(false);
        }
        
        public void DisableDropdown()
        {
            boxCollider_.enabled = false;
            notInteractablePanel.SetActive(true);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name != "SelectionSphere")
                return;
            
            if (buttonDown_)
                return;
            
            onRelease_ = () => { };
            inputBehaviour_ = other.gameObject.GetComponent<InputBehaviour>();
            inputBehaviour_.StandardInteractionStateChanged += OnStandardInteractionStateChanged;
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.name != "SelectionSphere")
                return;

            if (buttonDown_)
            {
                onRelease_ = () => { inputBehaviour_.StandardInteractionStateChanged -= OnStandardInteractionStateChanged; };
                return;
            }
            
            inputBehaviour_.StandardInteractionStateChanged -= OnStandardInteractionStateChanged;
        }

        private void OnStandardInteractionStateChanged(bool pressed)
        {
            buttonDown_ = pressed;

            if (buttonDown_)
            {
                itemsParentTransform.gameObject.SetActive(true);
                gridDropdownContentBehaviour.HoverStateChanged += OnHoverStateChanged;
            }
            else
            {
                itemsParentTransform.gameObject.SetActive(false);
                
                if (hoverState_.HasValue)
                    SelectionChanged?.Invoke(hoverState_.Value);
                
                onRelease_.Invoke();
                gridDropdownContentBehaviour.HoverStateChanged -= OnHoverStateChanged;
            }
        }
        
        private void OnHoverStateChanged(int? index)
        {
            hoverState_ = index;
        }
        
        public event Action<int> SelectionChanged;
    }
}
