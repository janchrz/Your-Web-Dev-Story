using System.Collections;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    public class UIEventLogLine : MonoBehaviour
    {
        // Inspector Settings
        [SerializeField] private TextMeshProUGUI m_text = null;

        // Private Members
        private Coroutine m_animationCoroutine = null;

        private void Awake()
        {
            m_text = GetComponent<TextMeshProUGUI>();
        }

        public void Show(Color color, string text, float characterAnimationDuration, float displayDuration)
        {
            if (m_animationCoroutine != null)
            {
                StopCoroutine(m_animationCoroutine);
            }

            gameObject.SetActive(true);
            m_text.color = color;
            m_text.text = string.Empty;

            // Make sure the line is at the bottom of the hierarchy to get display in the correct order
            Transform previousParent = transform.parent;
            transform.SetParent(null);
            transform.SetParent(previousParent);

            m_animationCoroutine = StartCoroutine(Animate(text, characterAnimationDuration, displayDuration));
        }

        private IEnumerator Animate(string text, float characterAnimationDuration, float displayDuration)
        {
            float durationOffset = 0.0f;

            while (text.Length > 0)
            {
                char c = text[0];
                m_text.text += c;
                text = text.Substring(1, text.Length - 1);

                if (!char.IsWhiteSpace(c))
                {
                    durationOffset += characterAnimationDuration;
                    yield return new WaitForSecondsRealtime(characterAnimationDuration);
                }
            }

            yield return new WaitForSecondsRealtime(math.max(displayDuration - durationOffset, 0.0f));

            gameObject.SetActive(false);
        }
    }
}
