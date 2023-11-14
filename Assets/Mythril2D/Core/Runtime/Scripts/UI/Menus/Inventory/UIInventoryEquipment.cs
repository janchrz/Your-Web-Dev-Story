using UnityEngine;

namespace Gyvr.Mythril2D
{
    public class UIInventoryEquipment : MonoBehaviour
    {
        [SerializeField] private UIInventoryEquipmentSlot[] m_slots = null;

        public void UpdateSlots()
        {
            InventorySystem inventorySystem = GameManager.GetSystem<InventorySystem>();

            foreach (UIInventoryEquipmentSlot slot in m_slots)
            {
                slot.SetEquipment(inventorySystem.GetEquipment(slot.equipmentType));
            }
        }

        public UINavigationCursorTarget FindNavigationTarget()
        {
            if (m_slots != null && m_slots.Length > 0)
            {
                return m_slots[0].gameObject.GetComponentInChildren<UINavigationCursorTarget>();
            }

            return null;
        }
    }
}
