using System.Collections.Generic;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    public class ContactDamageAbility : Ability<DamageAbilitySheet>
    {
        [Header("Settings")]
        [SerializeField] private ContactFilter2D m_contactFilter2D;

        private Collider2D m_hitbox = null;

        public override void Init(CharacterBase character, AbilitySheet settings)
        {
            base.Init(character, settings);

            // Use the character collider as the contact damage hitbox
            m_hitbox = character.GetComponent<Collider2D>();
        }

        private void Awake()
        {
            m_hitbox = GetComponentInParent<Collider2D>();
        }

        private void FixedUpdate()
        {
            DamageOutputDescriptor damageOutput = DamageSolver.SolveDamageOutput(m_character, m_sheet.damageDescriptor);

            List<Collider2D> colliders = new List<Collider2D>();

            Physics2D.OverlapCollider(m_hitbox, m_contactFilter2D, colliders);

            // If it hits something...
            foreach (Collider2D collider in colliders)
            {
                GameObject target = collider.gameObject;

                if (target != gameObject)
                {
                    DamageDispatcher.Send(target, damageOutput);
                }
            }
        }
    }
}
