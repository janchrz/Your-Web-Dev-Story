using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gyvr.Mythril2D
{
    public class UICharacter : MonoBehaviour, IUIMenu
    {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI m_class = null;
        [SerializeField] private TextMeshProUGUI m_level = null;
        [SerializeField] private TextMeshProUGUI m_experience = null;
        [SerializeField] private TextMeshProUGUI m_skillPoints = null;
        [SerializeField] private TextMeshProUGUI m_currency = null;
        [SerializeField] private UICharacterStat[] m_stats = null;
        [SerializeField] private TextMeshProUGUI m_applyButtonText = null;

        private Hero m_character => GameManager.Player;

        private Stats m_tempStats;
        private int m_availablePoints = 0;
        private int m_totalAvailablePoints = 0;

        public void Init()
        {
            foreach (UICharacterStat stat in m_stats)
            {
                stat.RegisterCallbacks(OnRemoveButtonPressed, OnAddButtonPressed);
            }
        }

        public void Show(params object[] args)
        {
            m_tempStats = new Stats();
            m_availablePoints = m_character.availablePoints;
            m_totalAvailablePoints = m_availablePoints;
            UpdateUI();
            gameObject.SetActive(true);
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

        public void Apply()
        {
            if (m_tempStats.GetTotal() > 0)
            {
                m_character.LogUsedPoints(m_totalAvailablePoints - m_availablePoints);
                m_totalAvailablePoints = m_availablePoints;
                m_character.AddCustomStats(m_tempStats);
                m_tempStats = new Stats();

                UpdateUI();
            }
        }

        public GameObject FindSomethingToSelect()
        {
            return m_stats.Length > 0 ? m_stats[0].GetFirstButton().gameObject : null;
        }

        private void UpdateUI()
        {
            UpdateInfoSection();
            UpdateStatsSection();
            m_applyButtonText.text = $"Apply {m_tempStats.GetTotal()} points";
        }

        private void UpdateInfoSection()
        {
            m_class.text = m_character.characterSheet.displayName;
            m_level.text = m_character.level.ToString();
            m_experience.text = StringFormatter.Format("{0}", m_character.nextLevelExperience - m_character.experience);
            m_skillPoints.text = m_availablePoints.ToString();
            m_currency.text = StringFormatter.Format("{0}", GameManager.InventorySystem.money.ToString());
        }

        private void UpdateStatsSection()
        {
            foreach (UICharacterStat stat in m_stats)
            {
                stat.UpdateUI(m_character.stats, m_tempStats);
            }
        }

        public void OnAddButtonPressed(EStat stat, Button button)
        {
            if (m_availablePoints > 0)
            {
                m_tempStats[stat] += 1;
                --m_availablePoints;
                UpdateUI();
            }
        }

        public void OnRemoveButtonPressed(EStat stat, Button button)
        {
            if (m_tempStats[stat] > 0)
            {
                m_tempStats[stat] -= 1;
                ++m_availablePoints;
                UpdateUI();
            }
        }
    }
}
