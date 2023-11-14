using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    public class UIAbilities : MonoBehaviour, IUIMenu
    {
        [Header("References")]
        [SerializeField] private GameObject m_abilityBarEntryPrefab = null;
        [SerializeField] private GameObject m_abilityListRoot = null;
        [SerializeField] private CanvasGroup m_listCanvasGroup = null;
        [SerializeField] private TextMeshProUGUI m_abilityDescription = null;
        [SerializeField] private UIAbilityBar m_abilityBar = null;

        private UIAbilityListEntry[] m_entries = null;
        private UIAbilityListEntry m_abilitySelected = null;

        public void Init() { }

        public void Show(params object[] args)
        {
            Debug.Assert(args.Length == 0, "SpellBook panel invoked with incorrect arguments");
            gameObject.SetActive(true);
            m_abilityBar.Init();
            UpdateUI();
            m_entries[0].ForceSelection();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void EnableInteractions(bool enable)
        {
            CanvasGroup canvasGroup = GetComponent<CanvasGroup>();

            if (canvasGroup)
            {
                canvasGroup.interactable = enable;
            }
        }

        public GameObject FindSomethingToSelect()
        {
            return null;
        }

        private void UpdateUI()
        {
            ClearSpellBookList();
            FillSpellBookList();
            m_abilityBar.UpdateUI();
        }

        private void ClearSpellBookList()
        {
            for (int i = 0; i < m_abilityListRoot.transform.childCount; ++i)
            {
                Transform child = m_abilityListRoot.transform.GetChild(i);
                Destroy(child.gameObject);
            }
        }

        private void FillSpellBookList()
        {
            ITriggerableAbility[] abilities = GameManager.Player.triggerableAbilities.ToArray();
            m_entries = new UIAbilityListEntry[abilities.Length];

            for (int i = 0; i < abilities.Length; ++i)
            {
                ITriggerableAbility ability = abilities[i];

                GameObject entryInstance = Instantiate(m_abilityBarEntryPrefab, m_abilityListRoot.transform);
                UIAbilityListEntry entry = entryInstance.GetComponent<UIAbilityListEntry>();

                if (entry)
                {
                    entry.Initialize(ability.GetAbilityBase().abilitySheet);
                    m_entries[i] = entry;
                }
            }
        }

        private void OnAbilityHovered(AbilitySheet sheet)
        {
            m_abilityDescription.text = sheet.description;

            var lines = new List<AbilitySheet.AbilityDescriptionLine>();
            sheet.GenerateAdditionalDescriptionLines(lines);

            // Add a line break before printing additional lines
            if (lines.Count > 0)
            {
                m_abilityDescription.text += "\n";
            }

            foreach (var line in lines)
            {
                m_abilityDescription.text += $"\n<u>{line.header}</u>: {line.content}";
            }
        }

        private void OnNullAbilityHovered()
        {
            m_abilityDescription.text = string.Empty;
        }

        // Select an ability from the list
        private void OnAbilitySelectedFromList(UIAbilityListEntry ability)
        {
            m_abilitySelected = ability;
            m_abilityBar.SelectFirstElement();
            m_listCanvasGroup.interactable = false;
        }

        // Select a slot to place the selected ability
        private void OnAbilityClicked(int abilityIndex)
        {
            if (m_abilitySelected == null)
            {
                GameManager.Player.Unequip(abilityIndex);
            }
            else
            {
                GameManager.Player.Equip(m_abilitySelected.GetTarget(), abilityIndex);
                m_abilitySelected.ForceSelection();
                m_abilitySelected = null;
            }

            m_listCanvasGroup.interactable = true;
        }
    }
}
