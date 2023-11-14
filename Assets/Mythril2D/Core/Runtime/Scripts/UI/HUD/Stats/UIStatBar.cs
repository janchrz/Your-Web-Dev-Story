using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gyvr.Mythril2D
{
    public class UIStatBar : MonoBehaviour
    {
        // Inspector Settings
        [SerializeField] private TextMeshProUGUI m_label = null;
        [SerializeField] private Slider m_slider = null;
        [SerializeField] private TextMeshProUGUI m_sliderText = null;
        [SerializeField] private EStat m_stat;

        private CharacterBase m_target = null;

        private void Start()
        {
            m_target = GameManager.Player;

            m_target.statsChanged.AddListener(OnStatsChanged);
            m_target.currentStatsChanged.AddListener(OnStatsChanged);

            UpdateUI();
        }

        private void OnStatsChanged(Stats previous)
        {
            UpdateUI();
        }

        private void UpdateUI()
        {
            m_label.text = GameManager.Config.GetTermDefinition(m_stat).shortName;

            int current = m_target.currentStats[m_stat];
            int max = m_target.stats[m_stat];

            m_slider.minValue = 0;
            m_slider.maxValue = max;
            m_slider.value = current;

            m_sliderText.text = StringFormatter.Format("{0}/{1}", current, max);
        }
    }
}
