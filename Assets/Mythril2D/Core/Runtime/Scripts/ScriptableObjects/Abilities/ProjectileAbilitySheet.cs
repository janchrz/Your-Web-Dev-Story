using UnityEngine;

namespace Gyvr.Mythril2D
{
    [CreateAssetMenu(menuName = AssetMenuIndexer.Mythril2D_Abilities + nameof(ProjectileAbilitySheet))]
    public class ProjectileAbilitySheet : DamageAbilitySheet
    {
        [SerializeField] private float m_projectileSpeed = 5.0f;
        [SerializeField] private int m_projectileCount = 1;
        [SerializeField] private float m_spread = 0.0f;

        public float projectileSpeed => m_projectileSpeed;
        public int projectileCount => m_projectileCount;
        public float spread => m_spread;
    }
}
