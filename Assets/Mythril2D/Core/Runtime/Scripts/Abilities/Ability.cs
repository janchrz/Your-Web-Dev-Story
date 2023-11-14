namespace Gyvr.Mythril2D
{
    public abstract class Ability<SheetType> : AbilityBase where SheetType : AbilitySheet
    {
        protected SheetType m_sheet = null;

        public override AbilitySheet abilitySheet => m_sheet;

        public override void Init(CharacterBase character, AbilitySheet settings)
        {
            base.Init(character, settings);
            m_sheet = (SheetType)settings;
        }
    }
}
