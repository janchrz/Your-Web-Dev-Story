using UnityEngine;

namespace Gyvr.Mythril2D
{
    [CreateAssetMenu(menuName = AssetMenuIndexer.Mythril2D_Characters + nameof(NPCSheet))]
    public class NPCSheet : CharacterSheet
    {
        public NPCSheet() : base(EAlignment.Neutral) { }
    }
}
