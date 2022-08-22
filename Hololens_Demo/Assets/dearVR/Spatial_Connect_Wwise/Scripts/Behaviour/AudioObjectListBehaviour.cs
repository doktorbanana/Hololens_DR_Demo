using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SpatialConnect.Wwise.Core;

namespace SpatialConnect.Wwise
{
    public class AudioObjectListBehaviour : MonoBehaviour, IAudioObjectListBehaviour
    {
        [SerializeField] private GameObject audioObjectListItemPrefab = default;

        private const float ROW_HEIGHT = 30f;
        private const int MAX_ROW = 25;

        private IFactory factory_;
        private IListManager<IAudioObjectListItemBehaviour> audioObjectListManager_;
        private IInputBehaviour inputBehaviour_;
        private IListTarget listTarget_;
        private IObjectPool<AudioObjectListItemBehaviour> objectPool_;
        private IAutoDisposer audioObjectListPresenter_;
        private IStringShortenerToggle stringShortenerToggle_;

        private readonly List<IAudioObjectListItemBehaviour> audioObjectListItemBehaviours_ =
            new List<IAudioObjectListItemBehaviour>();

        public void Init(IStringShortenerToggle stringShortenerToggle)
        {
            stringShortenerToggle_ = stringShortenerToggle;

            listTarget_ = new ListTarget(new Projector(transform), ROW_HEIGHT);
            objectPool_ = new ObjectPool<AudioObjectListItemBehaviour>(MAX_ROW, audioObjectListItemPrefab, transform);
            audioObjectListPresenter_ = new AutoDisposer();
            
            factory_ = new Factory();
            audioObjectListManager_ = factory_.CreateAudioObjectListManager(audioObjectListItemBehaviours_);
        }
        
        public void UpdateList(IEvent @event)
        {
            audioObjectListItemBehaviours_.Clear();
		
            audioObjectListPresenter_.Update(factory_.CreateAudioObjectListPresenter(this, @event));
            foreach (Transform child in transform)
                child.gameObject.SetActive(false);
            
            var row = 0U;
            CreateRow(@event.TreeRoot, Array.Empty<int>());
            
            void CreateRow(IAudioObjectPropertySet audioObjectPropertySet, int[] index)
            {
                if (row == MAX_ROW)
                    return;

                if (index.Length != 0)
                {
                    var (listItem, audioObjectListItemBehaviour) = objectPool_.Get();
                    listItem.SetActive(true);
                    audioObjectListItemBehaviour.Init(audioObjectPropertySet.Name, index, row, transform, stringShortenerToggle_);
                    audioObjectListItemBehaviour.Selected += OnItemSelected;
                    audioObjectListItemBehaviours_.Add(audioObjectListItemBehaviour);
                    row += 1;
                }

                for (var i = 0; i < audioObjectPropertySet.Children.Length; i++)
                    CreateRow(audioObjectPropertySet.Children[i], index.Concat(new[] { i }).ToArray());
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name != "SelectionSphere")
                return;
            
            inputBehaviour_ = other.gameObject.GetComponent<InputBehaviour>();
            inputBehaviour_.StandardInteractionStateChanged += OnStandardInteractionStateChanged;
        }
        
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.name != "SelectionSphere")
                return;

            audioObjectListManager_.Highlight(listTarget_.UpdateIndex(other.transform.position));
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.name != "SelectionSphere")
                return;

            inputBehaviour_.StandardInteractionStateChanged -= OnStandardInteractionStateChanged;
            audioObjectListManager_.Highlight(null);
        }

        private void OnStandardInteractionStateChanged(bool pressed)
        {
            if (!pressed)
                return;
                    
            audioObjectListManager_.Select(listTarget_.Index);
        }

        public void Select(int[] index)
        {
            audioObjectListManager_.ForEach( audioObjectListItemBehaviour =>
                audioObjectListItemBehaviour.OutlineActive = audioObjectListItemBehaviour.Index.SequenceEqual(index));
        }

        private void OnItemSelected(int[] index)
        {
            AudioObjectListSelectionChanged?.Invoke(index);
        }

        private void OnDestroy()
        {
            audioObjectListPresenter_.Dispose();
        }

        public event Action<int[]> AudioObjectListSelectionChanged;
    }
}
