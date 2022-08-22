using UnityEngine;
using UnityEngine.UI;

namespace SpatialConnect.Wwise
{
    public static class TextExtension
    {
        private static TextGenerator textGenerator_;

        public static void SetTextWithEllipsis(this Text textComponent, string value)
        {
            textGenerator_ ??= new TextGenerator();
            var rectTransform = textComponent.GetComponent<RectTransform>();
            var settings = textComponent.GetGenerationSettings(rectTransform.rect.size);
            textGenerator_.Populate(value, settings);

            var characterCountVisible = textGenerator_.characterCountVisible;
            var updatedText = value;

            if (value.Length > characterCountVisible)
            {
                updatedText = value.Substring(0, characterCountVisible - 1);
                updatedText += "…";
            }

            textComponent.text = updatedText;
        }
    }
}
