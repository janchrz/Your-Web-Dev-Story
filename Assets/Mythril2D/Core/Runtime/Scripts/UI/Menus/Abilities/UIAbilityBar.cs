namespace Gyvr.Mythril2D
{
    public class UIAbilityBar : InstancePool
    {
        protected UIAbilityBarEntry[] m_abilities = null;

        public void Init()
        {
            m_abilities = new UIAbilityBarEntry[m_poolSize];

            for (int i = 0; i < m_poolSize; ++i)
            {
                m_abilities[i] = m_instances[i].GetComponent<UIAbilityBarEntry>();
                m_abilities[i].SetAbility(null, i);
            }

            GameManager.Player.equippedAbilitiesChanged.AddListener(FillAbilityBar);
        }

        public void SelectFirstElement()
        {
            m_abilities[0].ForceSelection();
        }

        public void UpdateUI()
        {
            FillAbilityBar(GameManager.Player.equippedAbilities);
        }

        private void FillAbilityBar(AbilitySheet[] abilities)
        {
            for (int i = 0; i < m_abilities.Length; ++i)
            {
                AbilitySheet ability = abilities[i];
                m_abilities[i].SetAbility(ability, i);
            }
        }
    }
}