using System;
using UnityEngine;
using SpatialConnect.Wwise.Core;

namespace SpatialConnect.Wwise
{
    public class MixingSessionHandlerBehaviour : MonoBehaviour, IMixingSessionHandlerBehaviour
    {
        [SerializeField] private GameObject mixingSessionPrefab = default;

        private IDisposable mixingSessionHandlerPresenter_;
        private IStringShortenerToggle stringShortenerToggle_;
        
        public void Init(IMixingSessionManager mixingSessionManager, IStringShortenerToggle stringShortenerToggle, IFactory factory = null)
        {
            factory ??= new Factory();
            stringShortenerToggle_ = stringShortenerToggle;
            mixingSessionHandlerPresenter_ = factory.CreateMixingSessionHandlerPresenter(this, mixingSessionManager);
        }
        
        public void UpdateMixingSession(IMixingSession mixingSession)
        {
            foreach(Transform child in transform)
                Destroy(child.gameObject);
            
            var newMixingSession = Instantiate(mixingSessionPrefab, transform);
            newMixingSession.GetComponent<MixingSessionBehaviour>().Init(mixingSession, stringShortenerToggle_);
        }
        
        private void OnDestroy()
        {
            mixingSessionHandlerPresenter_.Dispose();
        }
    }
}
