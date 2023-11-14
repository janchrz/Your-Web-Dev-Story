using UnityEngine;

namespace Gyvr.Mythril2D
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectile : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Rigidbody2D m_rigidbody = null;
        [SerializeField] private Animator m_animator = null;

        [Header("Settings")]
        [SerializeField] private bool m_reverseRotation = false;
        [SerializeField] private float m_maxDuration = 2.0f;

        [Header("Animation Parameters")]
        [SerializeField] private string m_destroyAnimationParameter = "destroy";

        [Header("Audio")]
        [SerializeField] private AudioClipResolver m_collisionSound;

        private DamageOutputDescriptor m_damageOutputDescriptor;
        private Vector2 m_direction;
        private float m_speed;
        private float m_timer;
        private bool m_hasDestroyAnimation;
        private bool m_operating = false;

        private void Awake()
        {
            m_hasDestroyAnimation = m_animator && AnimationUtils.HasParameter(m_animator, m_destroyAnimationParameter);
        }

        public void Throw(DamageOutputDescriptor damageOutputDescriptor, Vector2 direction, float speed)
        {
            m_damageOutputDescriptor = damageOutputDescriptor;
            m_direction = direction;
            m_speed = speed;
            m_rigidbody.velocity = m_direction * m_speed;
            m_timer = 0.0f;
            transform.rotation = Quaternion.LookRotation(Vector3.forward, direction * (m_reverseRotation ? -1.0f : 1.0f));
            m_operating = true;
        }

        public void OnDestroyAnimationEnd()
        {
            Terminate(true);
        }

        private void Terminate(bool forceNoAnimation = false)
        {
            m_operating = false;
            m_rigidbody.velocity = Vector3.zero;

            if (!forceNoAnimation && m_hasDestroyAnimation)
            {
                m_animator?.SetTrigger(m_destroyAnimationParameter);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        private void OnBecameInvisible()
        {
            Terminate(true);
        }

        private void Update()
        {
            m_timer += Time.deltaTime;

            if (m_timer >= m_maxDuration)
            {
                Terminate();
            }
        }

        private void OnCollision()
        {
            GameManager.NotificationSystem.audioPlaybackRequested.Invoke(m_collisionSound);
            Terminate();
        }

        private void HandleCollision(GameObject target)
        {
            CharacterBase character = target.GetComponentInParent<CharacterBase>();

            if (character)
            {
                if (DamageDispatcher.Send(character.gameObject, m_damageOutputDescriptor))
                {
                    // Successfull collision with a valid character target 
                    OnCollision();
                }
            }
            else
            {
                // Successfull collision with anything else than a character target
                OnCollision();
            }
        }

        private bool TryColliding(GameObject target)
        {
            if (target.layer == LayerMask.NameToLayer(GameManager.Config.hitboxLayer))
            {
                if (m_operating && target != gameObject)
                {
                    HandleCollision(target);
                    return true;
                }
            }

            return false;
        }

        private bool IsProperCollider(int layer)
        {
            int layermask = GameManager.Config.collisionContactFilter.layerMask;
            return layermask == (layermask | (1 << layer));
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!TryColliding(collision.gameObject) && IsProperCollider(collision.gameObject.layer))
            {
                OnCollision();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            TryColliding(collision.gameObject);
        }
    }
}
