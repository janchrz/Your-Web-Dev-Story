using System;
using Unity.Mathematics;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    [Serializable]
    public struct InventoryDataBlock
    {
        public int money;
        public SerializableDictionary<Item, int> items;
    }

    public class InventorySystem : AGameSystem, IDataBlockHandler<InventoryDataBlock>
    {
        public int money => m_money;
        public SerializableDictionary<Item, int> items => m_items;

        private int m_money = 0;
        private SerializableDictionary<Item, int> m_items = new SerializableDictionary<Item, int>();

        public int GetItemCount(Item item)
        {
            if (items.TryGetValue(item, out int count))
            {
                return count;
            }

            return 0;
        }

        public void AddMoney(int value)
        {
            if (value > 0)
            {
                m_money += value;
                GameManager.NotificationSystem.moneyAdded.Invoke(value);
            }
        }

        public void RemoveMoney(int value)
        {
            if (value > 0)
            {
                m_money = math.max(money - value, 0);
                GameManager.NotificationSystem.moneyRemoved.Invoke(value);
            }
        }

        public bool TryRemoveMoney(int value)
        {
            if (HasSufficientFunds(value))
            {
                RemoveMoney(value);
                return true;
            }

            return false;
        }

        public bool HasSufficientFunds(int value)
        {
            return value <= money;
        }

        public Equipment GetEquipment(EEquipmentType type)
        {
            if (GameManager.Player.equipments.ContainsKey(type))
            {
                return GameManager.Player.equipments[type];
            }

            return null;
        }

        public void Equip(Equipment equipment)
        {
            Debug.Assert(equipment, "Cannot equip a null equipment");

            Equipment previousEquipment = GameManager.Player.Equip(equipment);

            RemoveFromBag(equipment, 1, true);

            if (previousEquipment)
            {
                AddToBag(previousEquipment, 1, true);
            }
        }

        public void UnEquip(EEquipmentType type)
        {
            Equipment previousEquipment = GameManager.Player.Unequip(type);

            if (previousEquipment)
            {
                AddToBag(previousEquipment, 1, true);
            }
        }

        public void AddToBag(Item item, int quantity = 1, bool forceNoEvent = false)
        {
            if (!items.ContainsKey(item))
            {
                items.Add(item, quantity);
            }
            else
            {
                items[item] += quantity;
            }

            if (!forceNoEvent)
            {
                GameManager.NotificationSystem.itemAdded.Invoke(item, quantity);
            }
        }

        public bool RemoveFromBag(Item item, int quantity = 1, bool forceNoEvent = false)
        {
            bool success = false;

            if (items.ContainsKey(item))
            {
                if (quantity >= items[item])
                {
                    items.Remove(item);
                }
                else
                {
                    items[item] -= quantity;
                }

                success = true;
            }

            if (!forceNoEvent)
            {
                GameManager.NotificationSystem.itemRemoved.Invoke(item, quantity);
            }

            return success;
        }

        public void LoadDataBlock(InventoryDataBlock block)
        {
            m_money = block.money;
            m_items = new SerializableDictionary<Item, int>(block.items);
        }

        public InventoryDataBlock CreateDataBlock()
        {
            return new InventoryDataBlock
            {
                money = m_money,
                items = new SerializableDictionary<Item, int>(m_items)
            };
        }
    }
}
