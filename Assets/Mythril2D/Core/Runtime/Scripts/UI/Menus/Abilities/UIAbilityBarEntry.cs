using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Gyvr.Mythril2D
{
    public class UIAbilityBarEntry : UIAbility, ISelectHandler, IPointerEnterHandler
    {
        [Header("References")]
        [SerializeField] private Button m_button = null;

        private void Awake()
        {
            if (m_button)
            {
                m_button.onClick.AddListener(OnClick);
            }
        }

        private void OnClick()
        {
            SendMessageUpwards("OnAbilityClicked", m_abilityIndex, SendMessageOptions.RequireReceiver);
        }

        public void ForceSelection()
        {
            m_button.Select();
        }

        public void OnSelect(BaseEventData eventData)
        {
            if (m_button.interactable)
            {
                if (m_button.navigation.mode != Navigation.Mode.None)
                {
                    if (m_abilitySheet)
                    {
                        SendMessageUpwards("OnAbilityHovered", m_abilitySheet);
                    }
                    else
                    {
                        // Cannot pass null objects to SendMessageUpwards, so added this instead
                        SendMessageUpwards("OnNullAbilityHovered");
                    }
                }
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            ForceSelection();
        }
    }
}
