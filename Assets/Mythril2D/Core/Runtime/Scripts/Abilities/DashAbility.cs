using System.Collections;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    public class DashAbility : ActiveAbility<DashAbilitySheet>
    {
        [Header("Reference")]
        [SerializeField] private ParticleSystem m_particleSystem = null;

        public override void Init(CharacterBase character, AbilitySheet settings)
        {
            base.Init(character, settings);
        }

        public override bool CanFire()
        {
            return base.CanFire() && !m_character.IsBeingPushed();
        }

        protected override void Fire()
        {
            Vector2 direction =
                m_character.IsMoving() ?
                m_character.movementDirection :
                (m_character.GetLookAtDirection() == EDirection.Right ? Vector2.right : Vector2.left);

            m_character.Push(direction, m_sheet.dashStrength, m_sheet.dashResistance, faceOppositeDirection: true);
            m_particleSystem.Play();

            TerminateCasting();
        }
    }
}
