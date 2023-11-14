using UnityEngine;

namespace Gyvr.Mythril2D
{
    [CreateAssetMenu(menuName = AssetMenuIndexer.Mythril2D_Abilities + nameof(DashAbilitySheet))]
    public class DashAbilitySheet : AbilitySheet
    {
        [Header("Dash Ability Settings")]
        [SerializeField][Min(0)] private float m_dashStrength = 1;
        [SerializeField][Min(0)] private float m_dashResistance = 1;

        public float dashStrength => m_dashStrength;
        public float dashResistance => m_dashResistance;
    }
}
