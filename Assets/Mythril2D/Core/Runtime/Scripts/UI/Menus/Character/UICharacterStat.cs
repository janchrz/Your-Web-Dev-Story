using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Gyvr.Mythril2D
{
    public class UICharacterStat : UIStat
    {
        [Header("References")]
        [SerializeField] private Button m_decreaseButton;
        [SerializeField] private Button m_increaseButton;

        public void RegisterCallbacks(UnityAction<EStat, Button> decrease, UnityAction<EStat, Button> increase)
        {
            m_decreaseButton.onClick.AddListener(() => decrease(m_stat, m_decreaseButton));
            m_increaseButton.onClick.AddListener(() => increase(m_stat, m_increaseButton));
        }

        public void UpdateUI(Stats stats, Stats tempStats)
        {
            if (tempStats[m_stat] > 0)
            {
                m_value.text = string.Format("{0} (+{1})", stats[m_stat], tempStats[m_stat]);
            }
            else
            {
                m_value.text = string.Format("{0}", stats[m_stat]);
            }
        }

        // Used for selection
        public Button GetFirstButton() => m_decreaseButton;
    }
}
