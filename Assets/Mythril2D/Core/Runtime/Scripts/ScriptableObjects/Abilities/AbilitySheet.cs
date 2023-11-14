using System.Collections.Generic;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    [CreateAssetMenu(menuName = AssetMenuIndexer.Mythril2D_Abilities + nameof(AbilitySheet))]
    public class AbilitySheet : ScriptableObject, INameable
    {
        public struct AbilityDescriptionLine
        {
            public string header;
            public string content;
        }

        public enum EAbilityStateManagementMode
        {
            Automatic,
            AlwaysOn,
            AlwaysOff
        }

        [Header("UI Settings")]
        [SerializeField] private Sprite m_icon = null;
        [SerializeField] private string m_displayName = string.Empty;
        [SerializeField] private string m_description = string.Empty;

        [Header("References")]
        [SerializeField] private GameObject m_prefab = null;

        [Header("Common Ability Settings")]
        [SerializeField] private int m_manaCost = 0;
        [SerializeField] private EAbilityStateManagementMode m_abilityStateManagementMode = EAbilityStateManagementMode.Automatic;
        [SerializeField] private bool m_canInterupt = false;
        [SerializeField] private EActionFlags m_disabledActionsWhileCasting = EActionFlags.All;

        [Header("Audio")]
        [SerializeField] private AudioClipResolver m_fireAudio;

        public Sprite icon => m_icon;
        public string description => m_description;
        public string displayName => DisplayNameUtils.GetNameOrDefault(this, m_displayName);
        public GameObject prefab => m_prefab;
        public bool canInterupt => m_canInterupt;
        public int manaCost => m_manaCost;
        public EAbilityStateManagementMode abilityStateManagementMode => m_abilityStateManagementMode;
        public EActionFlags disabledActionsWhileCasting => m_disabledActionsWhileCasting;
        public AudioClipResolver fireAudio => m_fireAudio;

        public virtual void GenerateAdditionalDescriptionLines(List<AbilityDescriptionLine> lines)
        {
            lines.Add(new AbilityDescriptionLine
            {
                header = "Mana Cost",
                content = m_manaCost.ToString()
            });
        }
    }
}
