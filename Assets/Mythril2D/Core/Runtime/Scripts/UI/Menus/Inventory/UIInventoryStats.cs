using UnityEngine;

namespace Gyvr.Mythril2D
{
    public class UIInventoryStats : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private UIStat[] m_stats = null;

        private Hero m_target = null;

        private void OnEnable()
        {
            UpdateUI();
        }

        private void Start()
        {
            UpdateUI();
        }

        public void UpdateUI()
        {
            m_target = GameManager.Player;

            foreach (UIStat stat in m_stats)
            {
                stat.UpdateUI(m_target.stats);
            }
        }
    }
}
