using UnityEngine;

namespace Gyvr.Mythril2D
{
    public class UINavigationCursorTarget : MonoBehaviour
    {
        public NavigationCursorStyle navigationCursorStyle => m_navigationCursorStyle;
        public Vector3 totalPositionOffset => m_navigationCursorStyle.positionOffset + m_positionOffset;
        public Vector2 totalSizeOffset => m_navigationCursorStyle.sizeOffset + m_sizeOffset;

        [SerializeField] private NavigationCursorStyle m_navigationCursorStyle = null;
        [SerializeField] private Vector2 m_positionOffset = Vector2.zero;
        [SerializeField] private Vector2 m_sizeOffset = Vector2.zero;
    }
}