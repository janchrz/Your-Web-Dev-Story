using UnityEngine;
using UnityEngine.UI;

namespace Gyvr.Mythril2D
{
    public class UIMonsterInfo : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Slider m_healthSlider = null;
        [SerializeField] private Slider m_manaSlider = null;
        [SerializeField] private Monster m_monster = null;

        private void Awake()
        {
            m_monster.statsChanged.AddListener(OnStatsChanged);
            m_monster.currentStatsChanged.AddListener(OnStatsChanged);
            Refresh();
        }

        public void Refresh()
        {
            if (m_healthSlider)
            {
                m_healthSlider.minValue = 0;
                m_healthSlider.maxValue = m_monster.stats[EStat.Health];
                m_healthSlider.value = m_monster.currentStats[EStat.Health];
            }

            if (m_manaSlider)
            {
                m_manaSlider.minValue = 0;
                m_manaSlider.maxValue = m_monster.stats[EStat.Mana];
                m_manaSlider.value = m_monster.currentStats[EStat.Mana];
            }
        }

        private void OnStatsChanged(Stats previous) => Refresh();
    }
}
