using UnityEngine;

namespace Gyvr.Mythril2D
{
    public class ProjectileAbility : ActiveAbility<ProjectileAbilitySheet>
    {
        [Header("References")]
        [SerializeField] private Animator m_animator = null;
        [SerializeField] private InstancePool m_projectilePool = null;
        [SerializeField] private Transform m_projectileSpawnPoint = null;

        [Header("Animation Parameters")]
        [SerializeField] private string m_fireAnimationParameter = "fire";

        public override void Init(CharacterBase character, AbilitySheet settings)
        {
            base.Init(character, settings);

            Debug.Assert(m_animator, ErrorMessages.InspectorMissingComponentReference<Animator>());
            Debug.Assert(m_animator.GetBehaviour<StateMessageDispatcher>(), string.Format("{0} not found on the projectile animator controller", typeof(StateMessageDispatcher).Name));
        }

        protected override void Fire()
        {
            m_animator?.SetTrigger(m_fireAnimationParameter);
        }

        public void OnChargeAnimationEnd()
        {
            if (!m_character.dead)
            {
                for (int i = 0; i < m_sheet.projectileCount; ++i)
                {
                    ThrowProjectile(i);
                }

                TerminateCasting();
            }
        }

        private void ThrowProjectile(int projectileIndex)
        {
            GameObject projectileInstance = m_projectilePool.GetAvailableInstance();
            projectileInstance.transform.position = m_projectileSpawnPoint.position;
            projectileInstance.SetActive(true);
            Projectile projectile = projectileInstance.GetComponent<Projectile>();
            bool lookAtLeft = m_character.GetLookAtDirection() == EDirection.Left;
            Vector3 direction =
                lookAtLeft ?
                Vector3.left :
                Vector3.right;

            float angleOffset = (m_sheet.spread / m_sheet.projectileCount) * projectileIndex - (m_sheet.spread / 2.0f);

            direction = Quaternion.AngleAxis(angleOffset, lookAtLeft ? Vector3.forward : Vector3.back) * direction;

            projectile.Throw(DamageSolver.SolveDamageOutput(m_character, m_sheet.damageDescriptor), direction, m_sheet.projectileSpeed);
        }
    }
}
