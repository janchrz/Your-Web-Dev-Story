using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Gyvr.Mythril2D
{
    public class UIAbility : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Image m_icon = null;

        protected AbilitySheet m_abilitySheet = null;
        protected int m_abilityIndex = -1;

        public virtual void SetAbility(AbilitySheet sheet, int index)
        {
            m_abilitySheet = sheet;
            m_abilityIndex = index;

            if (m_abilitySheet != null)
            {
                m_icon.enabled = true;
                m_icon.sprite = m_abilitySheet.icon;
            }
            else
            {
                m_icon.enabled = false;
            }
        }
    }
}
