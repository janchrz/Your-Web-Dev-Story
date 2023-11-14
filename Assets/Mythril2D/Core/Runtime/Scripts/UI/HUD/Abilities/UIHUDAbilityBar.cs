namespace Gyvr.Mythril2D
{
    public class UIHUDAbilityBar : InstancePool
    {
        protected UIHUDAbilityBarEntry[] m_abilities = null;

        private void Start()
        {
            GameManager.Player.equippedAbilitiesChanged.AddListener(OnEquippedAbilitiesChanged);

            m_abilities = new UIHUDAbilityBarEntry[m_poolSize];

            for (int i = 0; i < m_poolSize; ++i)
            {
                m_abilities[i] = m_instances[i].GetComponent<UIHUDAbilityBarEntry>();
                m_abilities[i].SetAbility(null, i);
            }

            OnEquippedAbilitiesChanged(GameManager.Player.equippedAbilities);
        }

        private void OnEquippedAbilitiesChanged(AbilitySheet[] abilities)
        {
            for (int i = 0; i < m_abilities.Length; ++i)
            {
                AbilitySheet ability = abilities[i];
                m_abilities[i].SetAbility(ability, i);
            }
        }
    }
}