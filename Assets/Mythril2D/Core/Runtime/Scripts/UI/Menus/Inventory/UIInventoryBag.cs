using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    public class UIInventoryBag : MonoBehaviour
    {
        private UIInventoryBagSlot[] m_slots = null;

        public void Init()
        {
            m_slots = GetComponentsInChildren<UIInventoryBagSlot>();

            // Because we display slots from bottom right to top left, we need to reverse them here to make sure we fill
            // them from top left to bottom right.
            Array.Reverse(m_slots);
        }

        public void UpdateSlots()
        {
            ClearSlots();
            FillSlots();
        }

        private void ClearSlots()
        {
            foreach (UIInventoryBagSlot slot in m_slots)
            {
                slot.Clear();
            }
        }

        private void FillSlots()
        {
            int usedSlots = 0;

            SerializableDictionary<Item, int> items = GameManager.InventorySystem.items;

            foreach (KeyValuePair<Item, int> entry in items)
            {
                UIInventoryBagSlot slot = m_slots[usedSlots++];
                slot.SetItem(entry.Key, entry.Value);
            }
        }

        public UIInventoryBagSlot GetFirstSlot()
        {
            return m_slots.Length > 0 ? m_slots[0] : null;
        }

        public UINavigationCursorTarget FindNavigationTarget()
        {
            if (m_slots.Length > 0)
            {
                return m_slots[0].gameObject.GetComponentInChildren<UINavigationCursorTarget>();
            }

            return null;
        }
    }
}
