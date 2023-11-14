using UnityEngine;
using UnityEngine.UI;

namespace Gyvr.Mythril2D
{
    public class UINavigationCursor : MonoBehaviour
    {
        [SerializeField] private Image m_image = null;

        private UINavigationCursorTarget m_target = null;

        private Vector3 GetTargetPosition()
        {
            return m_target.transform.position + m_target.totalPositionOffset;
        }

        private Vector2 GetTargetSize()
        {
            return ((RectTransform)m_target.transform).sizeDelta + m_target.totalSizeOffset;
        }

        private void Update()
        {
            UINavigationCursorTarget previousTarget = m_target;
            UINavigationCursorTarget currentTarget = GameManager.EventSystem.currentSelectedGameObject?.GetComponent<UINavigationCursorTarget>();

            // Ignore disabled targets
            if (currentTarget == null || !currentTarget.isActiveAndEnabled)
            {
                currentTarget = null;
            }

            // If target changed
            if (currentTarget != previousTarget)
            {
                m_target = currentTarget;

                // New valid target found
                if (m_target != null)
                {
                    ((RectTransform)transform).sizeDelta = GetTargetSize();
                    transform.position = GetTargetPosition();

                    m_image.enabled = true;
                    m_image.sprite = m_target.navigationCursorStyle.sprite;
                    m_image.color = m_target.navigationCursorStyle.color;
                }
                else
                // No target
                {
                    m_image.enabled = false;
                }
            }
        }
    }
}
