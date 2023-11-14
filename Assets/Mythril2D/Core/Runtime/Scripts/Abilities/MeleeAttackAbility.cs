using System.Collections.Generic;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    public class MeleeAttackAbility : ActiveAbility<DamageAbilitySheet>
    {
        [Header("References")]
        [SerializeField] private Animator m_animator = null;
        [SerializeField] private Collider2D m_hitbox = null;

        [Header("Settings")]
        [SerializeField] private ContactFilter2D m_contactFilter;

        [Header("Animation Parameters")]
        [SerializeField] private string m_fireAnimationParameter = "fire";
        [SerializeField] private string m_cancelAnimationParameter = "cancel";

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

        private void OnActionInterrupted()
        {
            if (m_sheet.canInterupt && m_animator)
            {
                m_animator.SetTrigger(m_cancelAnimationParameter);
                OnMeleeAttackAnimationEnd();
            }
        }

        public void OnMeleeAttackAnimationStart()
        {

        }

        public void OnMeleeAttackAnimationEnd()
        {
            if (!m_character.dead)
            {
                TerminateCasting();
            }
        }

        public void ApplyHit()
        {
            if (!m_character.dead)
            {
                List<Collider2D> colliders = new List<Collider2D>();

                Physics2D.OverlapCollider(m_hitbox, m_contactFilter, colliders);

                DamageOutputDescriptor damageOutput = DamageSolver.SolveDamageOutput(m_character, m_sheet.damageDescriptor);

                // If it hits something...
                foreach (Collider2D collider in colliders)
                {
                    GameObject target = collider.gameObject;

                    // We don't want to hit ourselves
                    if (target != gameObject)
                    {
                        DamageDispatcher.Send(target, damageOutput);
                    }
                }
            }
        }
    }
}
