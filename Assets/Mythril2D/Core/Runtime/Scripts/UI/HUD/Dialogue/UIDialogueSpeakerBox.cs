using TMPro;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    public class UIDialogueSpeakerBox : MonoBehaviour
    {
        // Inspector Settings
        [SerializeField] private TextMeshProUGUI m_text = null;

        public void Show() => SetVisible(true);
        public void Hide() => SetVisible(false);

        public void SetText(string text)
        {
            SetVisible(!string.IsNullOrWhiteSpace(text));
            m_text.text = text;
        }

        private void SetVisible(bool visible)
        {
            gameObject.SetActive(visible);
        }
    }
}
