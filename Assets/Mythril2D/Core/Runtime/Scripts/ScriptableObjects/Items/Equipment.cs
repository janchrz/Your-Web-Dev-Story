using UnityEngine;

namespace Gyvr.Mythril2D
{
    public enum EEquipmentType
    {
        Weapon,
        Head,
        Torso,
        Hands,
        Feet
    }

    [CreateAssetMenu(menuName = AssetMenuIndexer.Mythril2D_Items + nameof(Equipment))]
    public class Equipment : Item
    {
        [Header("Audio")]
        [SerializeField] private AudioClipResolver m_equipAudio;
        [SerializeField] private AudioClipResolver m_unequipAudio;

        [Header("Equipment")]
        [SerializeField] private EEquipmentType m_type;
        [SerializeField] private Stats m_bonusStats;

        public EEquipmentType type => m_type;
        public Stats bonusStats => m_bonusStats;

        public override void Use(CharacterBase user, EItemLocation location)
        {
            if (location == EItemLocation.Bag)
            {
                GameManager.InventorySystem.Equip(this);
                GameManager.NotificationSystem.audioPlaybackRequested.Invoke(m_equipAudio);
            }
            else
            {
                GameManager.InventorySystem.UnEquip(type);
                GameManager.NotificationSystem.audioPlaybackRequested.Invoke(m_unequipAudio);
            }
        }
    }
}
