using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Gyvr.Mythril2D
{
    public class UIAbilityListEntry : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IDeselectHandler
    {
        [Header("References")]
        [SerializeField] private Image m_image = null;
        [SerializeField] private TextMeshProUGUI m_name = null;
        [SerializeField] private Button m_button = null;

        private AbilitySheet m_target = null;

        private void Awake()
        {
            m_button.onClick.AddListener(OnSlotClicked);
        }

        public AbilitySheet GetTarget()
        {
            return m_target;
        }

        public void ForceSelection()
        {
            m_button.Select();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (m_button.IsInteractable())
            {
                m_button.Select();
            }
        }

        public void OnSelect(BaseEventData eventData)
        {
            SendMessageUpwards("OnAbilityHovered", m_target);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            GameManager.NotificationSystem.itemDetailsClosed.Invoke();
        }

        public void OnSlotClicked()
        {
            if (m_target != null)
            {
                SendMessageUpwards("OnAbilitySelectedFromList", this, SendMessageOptions.RequireReceiver);
            }
        }

        public void Initialize(AbilitySheet ability)
        {
            m_target = ability;
            m_name.text = ability.displayName;
            m_image.sprite = ability.icon;
        }
    }
}
