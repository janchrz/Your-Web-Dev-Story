using UnityEngine;

namespace Gyvr.Mythril2D
{
    [CreateAssetMenu(menuName = AssetMenuIndexer.Mythril2D_UI + nameof(NavigationCursorStyle))]
    public class NavigationCursorStyle : ScriptableObject
    {
        public Sprite sprite = null;
        public Color color = Color.white;
        public Vector2 positionOffset = Vector2.zero;
        public Vector2 sizeOffset = Vector2.zero;
    }
}
