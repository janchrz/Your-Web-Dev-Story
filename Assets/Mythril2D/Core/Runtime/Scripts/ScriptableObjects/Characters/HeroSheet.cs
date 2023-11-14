using UnityEngine;

namespace Gyvr.Mythril2D
{
    [CreateAssetMenu(menuName = AssetMenuIndexer.Mythril2D_Characters + nameof(HeroSheet))]
    public class HeroSheet : CharacterSheet
    {
        [Header("Hero")]
        public Stats baseStats;
        public int pointsPerLevel = 5;
        public LevelScaledInteger experience = new LevelScaledInteger();

        public HeroSheet() : base(EAlignment.Good) { }
    }
}
