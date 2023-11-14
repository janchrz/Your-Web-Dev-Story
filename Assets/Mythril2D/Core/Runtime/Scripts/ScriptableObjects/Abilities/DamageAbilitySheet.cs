using System.Collections.Generic;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    [CreateAssetMenu(menuName = AssetMenuIndexer.Mythril2D_Abilities + nameof(DamageAbilitySheet))]
    public class DamageAbilitySheet : AbilitySheet
    {
        [Header("Damage Ability Settings")]
        [SerializeField] private DamageDescriptor m_damageDescriptor;

        public DamageDescriptor damageDescriptor => m_damageDescriptor;

        public override void GenerateAdditionalDescriptionLines(List<AbilityDescriptionLine> lines)
        {
            lines.Add(new AbilityDescriptionLine
            {
                header = "Damage",
                content = m_damageDescriptor.scale.ToString()
            });

            lines.Add(new AbilityDescriptionLine
            {
                header = "Type",
                content = m_damageDescriptor.type.ToString()
            });
        }
    }
}
