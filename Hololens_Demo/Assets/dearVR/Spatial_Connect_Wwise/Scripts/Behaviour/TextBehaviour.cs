using System;
using UnityEngine;
using UnityEngine.UI;
using SpatialConnect.Wwise.Core;

namespace SpatialConnect.Wwise
{
    [RequireComponent(typeof(Text))]
    public class TextBehaviour : MonoBehaviour, ITextBehaviour
    {
        private Text textComponent_;

        private readonly IAutoDisposer textPresenter_ = new AutoDisposer();

        private void Awake()
        {
            textComponent_ = GetComponent<Text>();
        }

        public void Init(IDisposable textPresenter)
        {
            textPresenter_.Update(textPresenter);
        }

        public string Text
        {
            set => textComponent_.text = value;
        }

        public string TextWithEllipsis
        {
            set => textComponent_.SetTextWithEllipsis(value);
        }

        private void OnDestroy()
        {
            textPresenter_.Dispose();
        }
    }
}
