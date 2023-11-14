using TMPro;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    public class FloatingText : MonoBehaviour
    {
        // Component References
        private TextMeshProUGUI m_textMesh = null;
        private Animator m_animator = null;

        private void Awake()
        {
            m_textMesh = GetComponentInChildren<TextMeshProUGUI>();
            m_animator = GetComponentInChildren<Animator>();
        }

        public void OnFloatingTextAnimationEnd()
        {
            Stop();
        }

        public void Play(string text, Vector2 position, Color color, string animationTrigger)
        {
            gameObject.SetActive(true);
            transform.position = position;
            m_textMesh.color = color;
            m_textMesh.text = text;
            m_animator.SetTrigger(animationTrigger);
        }

        public void Stop()
        {
            gameObject.SetActive(false);
        }
    }
}
