using System;
using UnityEngine;
using UnityEngine.UI;
using SpatialConnect.Wwise.Core;

namespace SpatialConnect.Wwise
{
    public class ConnectionStateTextBehaviour : MonoBehaviour, ITextBehaviour
    {
        [SerializeField] private Text text = default;

        private IDisposable connectionStateTextPresenter_;

        public void Init(IWaapiSession waapiSession, IFactory factory = null)
        {
            factory ??= new Factory();
            connectionStateTextPresenter_ = factory.CreateConnectionStatePresenter(this, waapiSession);
        }
        
        public string Text
        {
            set => text.text = value;
        }

        public string TextWithEllipsis
        {
            set => text.SetTextWithEllipsis(value);
        }

        private void OnDestroy()
        {
            connectionStateTextPresenter_.Dispose();
        }
    }
}
