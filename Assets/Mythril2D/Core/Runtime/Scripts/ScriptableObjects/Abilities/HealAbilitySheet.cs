using System.Collections.Generic;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    [CreateAssetMenu(menuName = AssetMenuIndexer.Mythril2D_Abilities + nameof(HealAbilitySheet))]
    public class HealAbilitySheet : AbilitySheet
    {
        [Header("Heal Ability Settings")]
        [SerializeField][Min(0)] private int m_healAmount = 1;

        public int healAmount => m_healAmount;

        public override void GenerateAdditionalDescriptionLines(List<AbilityDescriptionLine> lines)
        {
            lines.Add(new AbilityDescriptionLine
            {
                header = "Heal",
                content = m_healAmount.ToString()
            });
        }
    }
}
