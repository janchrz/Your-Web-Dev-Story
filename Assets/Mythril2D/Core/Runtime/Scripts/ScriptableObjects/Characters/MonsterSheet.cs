using UnityEngine;

namespace Gyvr.Mythril2D
{
    [CreateAssetMenu(menuName = AssetMenuIndexer.Mythril2D_Characters + nameof(MonsterSheet))]
    public class MonsterSheet : CharacterSheet
    {
        [Header("Monster")]
        public LevelScaledStats stats = new LevelScaledStats();

        [Header("Rewards")]
        public LevelScaledInteger experience = new LevelScaledInteger();
        public LevelScaledInteger money = new LevelScaledInteger();
        public Loot[] potentialLoot;

        [Header("Actions")]
        public ActionHandler[] executeOnDeath;

        public MonsterSheet() : base(EAlignment.Evil) { }
    }
}
