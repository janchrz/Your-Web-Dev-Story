using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Gyvr.Mythril2D
{
    public class UIInventoryEquipmentSlot : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IDeselectHandler
    {
        [SerializeField] private EEquipmentType m_equipmentType = EEquipmentType.Head;
        [SerializeField] private Image m_placeholder = null;
        [SerializeField] private Image m_content = null;
        [SerializeField] private Button m_button = null;

        public EEquipmentType equipmentType => m_equipmentType;

        private Equipment m_equipment = null;
        private bool m_selected = false;

        public void OnPointerEnter(PointerEventData eventData)
        {
            m_button.Select();
        }

        public void OnSelect(BaseEventData eventData)
        {
            m_selected = true;
            GameManager.NotificationSystem.itemDetailsOpened.Invoke(m_equipment);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            m_selected = false;
            GameManager.NotificationSystem.itemDetailsClosed.Invoke();
        }

        public void SetEquipment(Equipment equipment)
        {
            m_equipment = equipment;

            if (equipment)
            {
                Debug.Assert(equipment.type == m_equipmentType, "Equipment type mismatch");

                m_placeholder.enabled = false;
                m_content.enabled = true;
                m_content.sprite = equipment.icon;
            }
            else
            {
                m_placeholder.enabled = true;
                m_content.enabled = false;
                m_content.sprite = null;
            }

            if (m_selected)
            {
                GameManager.NotificationSystem.itemDetailsOpened.Invoke(m_equipment);
            }
        }

        private void Awake()
        {
            m_button.onClick.AddListener(OnSlotClicked);
        }

        private void OnSlotClicked()
        {
            if (m_equipment != null)
            {
                SendMessageUpwards("OnEquipmentItemClicked", m_equipment, SendMessageOptions.RequireReceiver);
            }
        }
    }
}
