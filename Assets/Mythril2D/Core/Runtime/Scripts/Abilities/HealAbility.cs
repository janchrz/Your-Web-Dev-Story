using UnityEngine;

namespace Gyvr.Mythril2D
{
    public class HealAbility : ActiveAbility<HealAbilitySheet>
    {
        [Header("Reference")]
        [SerializeField] private Animator m_animator = null;

        [Header("Animation Parameters")]
        [SerializeField] private string m_fireAnimationParameter = "fire";

        public override void Init(CharacterBase character, AbilitySheet settings)
        {
            base.Init(character, settings);

            Debug.Assert(m_animator, ErrorMessages.InspectorMissingComponentReference<Animator>());
            Debug.Assert(m_animator.GetBehaviour<StateMessageDispatcher>(), string.Format("{0} not found on the melee attack animator controller", typeof(StateMessageDispatcher).Name));
        }

        protected override void Fire()
        {
            m_animator?.SetTrigger(m_fireAnimationParameter);
        }

        public void OnHealAnimationStart() { }

        public void OnHealAnimationEnd()
        {
            if (!m_character.dead)
            {
                TerminateCasting();
            }
        }

        public void ApplyHeal()
        {
            if (!m_character.dead)
            {
                m_character.Heal(m_sheet.healAmount);
            }
        }
    }
}
