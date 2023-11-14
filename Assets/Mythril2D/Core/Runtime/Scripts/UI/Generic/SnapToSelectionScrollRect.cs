using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Gyvr.Mythril2D
{
    [RequireComponent(typeof(ScrollRect))]
    public class SnapToSelectionScrollRect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        // Inspector Settings
        [SerializeField] private float m_snappingSpeed = 20.0f;

        private ScrollRect m_scrollRect = null;
        private GameObject m_selection = null;
        private Vector2 m_destination = Vector2.zero;

        private bool m_hovered = false;

        public void OnPointerEnter(PointerEventData e)
        {
            m_hovered = true;
        }

        public void OnPointerExit(PointerEventData e)
        {
            m_hovered = false;
        }

        private void Awake()
        {
            m_scrollRect = GetComponent<ScrollRect>();
        }

        private GameObject GetSelectedChild()
        {
            GameObject selection = GameManager.EventSystem.currentSelectedGameObject;

            if (selection != null && selection.transform.IsChildOf(m_scrollRect.content.transform))
            {
                return selection;
            }

            return null;
        }

        private void Update()
        {
            GameObject selection = GetSelectedChild();

            // If the selection changed.
            if (selection != m_selection)
            {
                m_selection = selection;

                // If the new selected child is valid.
                if (m_selection)
                {
                    RectTransform selectionRectTransform = (RectTransform)m_selection.transform;
                    RectTransform contentRectTransform = m_scrollRect.content;

                    float itemPositonInViewport = selectionRectTransform.anchoredPosition.y;
                    float viewportHalfHeight = m_scrollRect.viewport.rect.height / 2.0f;

                    // If the ScrollRect is hovered, do not update the destination.
                    if (!m_hovered)
                    {
                        m_destination.x = contentRectTransform.anchoredPosition.x;
                        m_destination.y = math.abs(itemPositonInViewport) - viewportHalfHeight;
                        m_destination.y = math.clamp(m_destination.y, 0, contentRectTransform.sizeDelta.y / 2.0f);
                    }
                }
            }

            // If the ScrollRect is hovered, lock the destination to the current position
            // This way, the position is kept even after hovering, until the selection changes.
            if (m_hovered)
            {
                m_destination = m_scrollRect.content.anchoredPosition;
            }
            // If something is selected, move towards the destination.
            else if (m_selection)
            {
                m_scrollRect.content.anchoredPosition = math.lerp(m_scrollRect.content.anchoredPosition, m_destination, Time.unscaledDeltaTime * m_snappingSpeed);
            }
        }
    }
}
