using UnityEngine;

namespace Gyvr.Mythril2D
{
    public abstract class Character<CharacterSheetDerivation> : CharacterBase where CharacterSheetDerivation : CharacterSheet
    {
        [Header("Character Settings")]
        [SerializeField] protected CharacterSheetDerivation m_sheet = null;

        public override CharacterSheet characterSheet => m_sheet;
    }
}

