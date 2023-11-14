using TMPro;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    public class UIStat : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] protected TextMeshProUGUI m_value = null;

        [Header("Settings")]
        [SerializeField] protected EStat m_stat;

        public EStat stat => m_stat;

        public void UpdateUI(Stats stats)
        {
            m_value.text = stats[m_stat].ToString();
        }
    }
}
