using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

namespace Gyvr.Mythril2D
{
    public enum EAlignment
    {
        Good,
        Evil,
        Neutral,
        Default = Neutral
    }

    [Flags]
    public enum EActionFlags
    {
        None = 0,
        Move = 1,
        Interact = 2,
        UseAbility = 4,
        All = ~0
    }

    public enum EMovementMode
    {
        Bidirectional,
        Polydirectional
    }

    public enum EDirection
    {
        Left,
        Right,
        Default = Left
    }

    public struct CharacterAbilitySlot
    {
        public AbilitySheet sheet;
        public AbilityBase instance;
    }

    public abstract class CharacterBase : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] protected Animator m_animator = null;
        [SerializeField] private Rigidbody2D m_rigidbody = null;
        [SerializeField] private SpriteRenderer m_spriteRenderer = null;

        [Header("General Settings")]
        [Range(Stats.MinLevel, Stats.MaxLevel)]
        [SerializeField] protected int m_level = Stats.MinLevel;
        [SerializeField] private bool m_invincibleOnHit = false;
        [SerializeField] private Transform m_abilitiesRoot = null;
        [SerializeField] private AbilitySheet[] m_additionalAbilities = null;

        [Header("Movement Settings")]
        [SerializeField] private float m_moveSpeed = 4.0f;
        [SerializeField] private float m_pushIntensityScale = 1.0f;
        [SerializeField] private float m_pushResistanceScale = 1.0f;
        [SerializeField] private EMovementMode m_movementMode = EMovementMode.Bidirectional;

        [Header("Animation Parameters")]
        [SerializeField] private string m_hitAnimationParameter = "hit";
        [SerializeField] private string m_deathAnimationParameter = "death";
        [SerializeField] private string m_invincibleAnimationParameter = "invincible";
        [SerializeField] private string m_moveXAnimationParameter = "moveX";
        [SerializeField] private string m_moveYAnimationParameter = "moveY";
        [SerializeField] private string m_isMovingAnimationParameter = "isMoving";

        // Public Events
        [HideInInspector] public UnityEvent<EDirection> directionChangedEvent = new UnityEvent<EDirection>();

        public Animator animator => m_animator;
        public IEnumerable<AbilitySheet> abilitySheets => m_abilitiesInstances.Keys;
        public IEnumerable<AbilityBase> abilityInstances => m_abilitiesInstances.Values;
        public IEnumerable<ITriggerableAbility> triggerableAbilities => m_triggerableAbilities;
        public abstract CharacterSheet characterSheet { get; }
        public bool dead => m_currentStats[EStat.Health] == 0;
        public int level => m_level;
        public bool invincible => characterSheet.alignment == EAlignment.Neutral || m_invincibleAnimationPlaying || dead;
        public Stats currentStats => m_currentStats.stats;
        public Stats stats => m_stats.stats;
        public UnityEvent<Stats> currentStatsChanged => m_currentStats.changed;
        public UnityEvent<Stats> statsChanged => m_stats.changed;
        public EActionFlags actionFlags => m_actionFlags;
        public Vector2 movementDirection => m_movementDirection;

        // Character Base Private Members
        private EActionFlags m_actionFlags = EActionFlags.All;
        private bool m_hasDeathAnimation = false;
        private bool m_hasHitAnimation = false;
        private bool m_hasInvincibleAnimation = false;
        private bool m_invincibleAnimationPlaying = false;
        private Dictionary<AbilitySheet, int> m_abilities = new Dictionary<AbilitySheet, int>();
        private Dictionary<AbilitySheet, AbilityBase> m_abilitiesInstances = new Dictionary<AbilitySheet, AbilityBase>();
        private HashSet<ITriggerableAbility> m_triggerableAbilities = new HashSet<ITriggerableAbility>();
        protected ObservableStats m_currentStats = new ObservableStats();
        protected ObservableStats m_stats = new ObservableStats();

        // Move Private Members
        private List<RaycastHit2D> m_castCollisions = new List<RaycastHit2D>();
        private Vector2 m_movementDirection;
        private Vector2 m_lastSuccessfullMoveDirection;
        private bool m_pushed = false;
        private Vector2 m_pushDirection;
        private float m_pushIntensity = 0.0f;
        private float m_pushResistance = 0.0f;

        protected virtual void Awake()
        {
            Debug.Assert(m_animator, ErrorMessages.InspectorMissingComponentReference<Animator>());
            Debug.Assert(m_spriteRenderer, ErrorMessages.InspectorMissingComponentReference<SpriteRenderer>());
            Debug.Assert(m_rigidbody, ErrorMessages.InspectorMissingComponentReference<Rigidbody2D>());

            m_stats.changed.AddListener(OnStatsChanged);
            m_currentStats.changed.AddListener(OnCurrentStatsChanged);

            CheckForAnimations();
            InitializeAbilities();
        }

        private void OnStatsChanged(Stats previous)
        {
            Stats difference = m_stats.stats - previous;
            Stats newCurrentStats = m_currentStats.stats + difference;
            // Make sure we don't kill the character when updating its maximum stats
            newCurrentStats[EStat.Health] = math.max(newCurrentStats[EStat.Health], 1);
            m_currentStats.Set(newCurrentStats);
        }

        private void OnCurrentStatsChanged(Stats previous)
        {
            if (m_currentStats[EStat.Health] == 0)
            {
                Die();
            }
        }

        private void CheckForAnimations()
        {
            if (m_animator)
            {
                m_hasHitAnimation = AnimationUtils.HasParameter(m_animator, m_hitAnimationParameter);
                m_hasDeathAnimation = AnimationUtils.HasParameter(m_animator, m_deathAnimationParameter);
                m_hasInvincibleAnimation = AnimationUtils.HasParameter(m_animator, m_invincibleAnimationParameter);
            }
        }

        private void InitializeAbilities()
        {
            IEnumerable<AbilitySheet> characterSpecificAvailableAbilities = characterSheet.GetAvailableAbilitiesAtLevel(m_level);

            foreach (AbilitySheet ability in characterSpecificAvailableAbilities)
            {
                AddAbility(ability);
            }

            if (m_additionalAbilities != null)
            {
                foreach (AbilitySheet ability in m_additionalAbilities)
                {
                    AddAbility(ability);
                }
            }
        }

        protected void AddAbility(AbilitySheet ability)
        {
            if (m_abilities.ContainsKey(ability))
            {
                ++m_abilities[ability];
            }
            else
            {
                AbilityBase abilityInstance = InstantiateAbilityPrefab(ability);

                m_abilities.Add(ability, 1);
                m_abilitiesInstances.Add(ability, abilityInstance);

                if (abilityInstance is ITriggerableAbility)
                {
                    m_triggerableAbilities.Add((ITriggerableAbility)abilityInstance);
                }
            }
        }

        protected void RemoveAbility(AbilitySheet ability)
        {
            if (!m_abilities.ContainsKey(ability))
            {
                Debug.LogAssertion("Cannot remove an ability that hasn't been added in the first place.");
            }
            else
            {
                --m_abilities[ability];

                if (m_abilities[ability] == 0)
                {
                    m_abilities.Remove(ability);

                    AbilityBase abilityInstance = m_abilitiesInstances[ability];

                    if (abilityInstance is ITriggerableAbility)
                    {
                        m_triggerableAbilities.Remove((ITriggerableAbility)abilityInstance);
                    }

                    m_abilitiesInstances.Remove(ability);
                }
            }
        }

        public ITriggerableAbility GetTriggerableAbility(AbilitySheet sheet)
        {
            return (ITriggerableAbility)m_abilitiesInstances[sheet];
        }

        private AbilityBase InstantiateAbilityPrefab(AbilitySheet sheet)
        {
            GameObject instance = Instantiate(sheet.prefab, m_abilitiesRoot);
            instance.name = sheet.displayName;
            AbilityBase ability = instance.GetComponent<AbilityBase>();
            Debug.AssertFormat(ability, "The provided ability prefab doesn't have a behaviour of type {0} attached to its root", typeof(AbilityBase).Name);
            ability.Init(this, sheet);

            switch (sheet.abilityStateManagementMode)
            {
                case AbilitySheet.EAbilityStateManagementMode.AlwaysOn:
                    ability.gameObject.SetActive(true);
                    break;

                case AbilitySheet.EAbilityStateManagementMode.AlwaysOff:
                    ability.gameObject.SetActive(false);
                    break;

                case AbilitySheet.EAbilityStateManagementMode.Automatic:
                    if (ability is ITriggerableAbility)
                    {
                        ability.gameObject.SetActive(false);
                    }
                    break;
            }

            return ability;
        }

        public void FireAbility(AbilitySheet sheet)
        {
            AbilityBase abilityBase = null;

            if (m_abilitiesInstances.TryGetValue(sheet, out abilityBase) && abilityBase is ITriggerableAbility)
            {
                FireAbility((ITriggerableAbility)abilityBase);
            }
            else
            {
                Debug.LogError($"Could not find triggerable ability matching ability sheet [{sheet.name}]");
            }
        }

        public void FireAbility(ITriggerableAbility ability)
        {
            AbilityBase abilityBase = ability.GetAbilityBase();

            if (ability.CanFire())
            {
                GameManager.NotificationSystem.audioPlaybackRequested.Invoke(abilityBase.abilitySheet.fireAudio);

                bool isAbilityStateAutomaticallyManaged = abilityBase.abilitySheet.abilityStateManagementMode == AbilitySheet.EAbilityStateManagementMode.Automatic;

                if (isAbilityStateAutomaticallyManaged)
                {
                    abilityBase.gameObject.SetActive(true);
                    ability.Fire(() => abilityBase.gameObject.SetActive(false));
                }
                else
                {
                    ability.Fire(null);
                }
            }
        }

        public bool TryPlayHitAnimation()
        {
            if (m_animator && m_hasHitAnimation)
            {
                m_animator.SetTrigger(m_hitAnimationParameter);
                return true;
            }

            return false;
        }

        public bool TryPlayDeathAnimation()
        {
            if (m_animator && m_hasDeathAnimation)
            {
                m_animator.SetTrigger(m_deathAnimationParameter);
                return true;
            }

            return false;
        }

        public bool TryPlayInvincibleAnimation()
        {
            if (m_animator && m_hasInvincibleAnimation)
            {
                m_animator.SetTrigger(m_invincibleAnimationParameter);
                return true;
            }

            return false;
        }

        private bool TryPush(DamageOutputDescriptor damageOutput)
        {
            if (damageOutput.source == EDamageSource.Character)
            {
                CharacterBase attacker = damageOutput.attacker as CharacterBase;
                float3 hitDir3d = math.normalize(transform.position - attacker.transform.position);
                float2 hitDir2d = new float2(hitDir3d.x, hitDir3d.y);

                Push(hitDir2d);

                return true;
            }

            return false;
        }

        public bool CanBeAttackedBy(DamageOutputDescriptor damageOutput)
        {
            if (damageOutput.source == EDamageSource.Character)
            {
                return CanBeAttackedBy((CharacterBase)damageOutput.attacker);
            }

            return true;
        }

        public bool CanBeAttackedBy(CharacterBase attacker)
        {
            return attacker.characterSheet.alignment != characterSheet.alignment;
        }

        private void InterruptActions()
        {
            BroadcastMessage("OnActionInterrupted", SendMessageOptions.DontRequireReceiver);
        }

        public bool Damage(DamageOutputDescriptor damageOutput)
        {
            if (!invincible && CanBeAttackedBy(damageOutput))
            {
                DamageInputDescriptor damageInput = DamageSolver.SolveDamageInput(this, damageOutput);

                TryPush(damageOutput);

                if (damageInput.damage > 0)
                {
                    InterruptActions();
                    TryPlayHitAnimation();

                    m_currentStats[EStat.Health] -= math.min(damageInput.damage, m_currentStats[EStat.Health]);

                    GameManager.NotificationSystem.audioPlaybackRequested.Invoke(characterSheet.hitAudio);

                    if (!dead)
                    {
                        if (m_invincibleOnHit)
                        {
                            TryPlayInvincibleAnimation();
                        }
                    }
                }

                GameManager.NotificationSystem.damageApplied.Invoke(this, damageInput);

                return true;
            }

            return false;
        }

        public void Heal(int value)
        {
            if (!dead)
            {
                int missingHealth = m_stats[EStat.Health] - m_currentStats[EStat.Health];
                m_currentStats[EStat.Health] += math.min(value, missingHealth);
                GameManager.NotificationSystem.healthRecovered.Invoke(this, value);
            }
        }

        public void RecoverMana(int value)
        {
            int missingMana = m_stats[EStat.Mana] - m_currentStats[EStat.Mana];
            m_currentStats[EStat.Mana] += math.min(value, missingMana);
            GameManager.NotificationSystem.manaRecovered.Invoke(this, value);
        }

        public void ConsumeMana(int value)
        {
            m_currentStats[EStat.Mana] -= math.min(value, m_currentStats[EStat.Mana]);
            GameManager.NotificationSystem.manaConsumed.Invoke(this, value);
        }

        public void EnableActions(EActionFlags actions)
        {
            m_actionFlags |= actions;
        }

        public void DisableActions(EActionFlags actions)
        {
            m_actionFlags &= ~actions;
        }

        public bool Can(EActionFlags actions)
        {
            return m_actionFlags.HasFlag(actions);
        }

        protected virtual void Die()
        {
            GameManager.NotificationSystem.audioPlaybackRequested.Invoke(characterSheet.deathAudio);

            if (!TryPlayDeathAnimation())
            {
                OnDeath();
            }
            else
            {
                Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
                Array.ForEach(colliders, (collider) => collider.enabled = false);
            }
        }
        
        protected virtual void OnDeath()
        {
            Destroy(gameObject);
        }

        [Obsolete("Deprecated, shouldn't be used anymore")]
        private void OnDamageReceived(DamageOutputDescriptor damageOutput)
        {
            Damage(damageOutput);
        }

        private void OnHealReceived(int amount)
        {
            Heal(amount);
        }

        private void OnInvincibleAnimationStart()
        {
            m_invincibleAnimationPlaying = true;
        }

        private void OnInvincibleAnimationEnd()
        {
            m_invincibleAnimationPlaying = false;
        }

        private void OnDeathAnimationStart()
        {
            DisableActions(EActionFlags.All);
        }

        private void OnDeathAnimationEnd()
        {
            OnDeath();
        }

        #region Movement
        // TODO: Make prettier
        private void FixedUpdate()
        {
            if (m_pushed)
            {
                if (m_pushIntensity > 0.2f)
                {
                    TryMove(m_pushDirection, m_pushIntensity);
                    m_pushIntensity = Mathf.Lerp(m_pushIntensity, 0.0f, Time.fixedDeltaTime * m_pushResistance);
                }
                else
                {
                    m_pushed = false;
                }
            }

            m_lastSuccessfullMoveDirection = Vector2.zero;

            if (Can(EActionFlags.Move) && !m_pushed)
            {
                // If movement input is not 0, try to move
                if (m_movementDirection != Vector2.zero)
                {
                    bool success = TryMove(m_movementDirection, m_moveSpeed);

                    if (!success)
                    {
                        success = TryMove(new Vector2(m_movementDirection.x, 0), m_moveSpeed);
                    }

                    if (!success)
                    {
                        TryMove(new Vector2(0, m_movementDirection.y), m_moveSpeed);
                    }
                }

                SetLookAtDirection(m_movementDirection.x);
            }

            m_animator.SetBool(m_isMovingAnimationParameter, m_lastSuccessfullMoveDirection.magnitude > 0.0f);

            if (m_movementMode == EMovementMode.Polydirectional)
            {
                m_animator.SetFloat(m_moveXAnimationParameter, m_lastSuccessfullMoveDirection.x);
                m_animator.SetFloat(m_moveYAnimationParameter, m_lastSuccessfullMoveDirection.y);
            }
        }

        public void SetMovementDirection(Vector2 direction)
        {
            m_movementDirection = direction;
        }

        public bool IsMovingUp() => m_movementDirection.y > 0.0f || (m_pushed && m_pushDirection.y > 0.0f);
        public bool IsMovingDown() => m_movementDirection.y < 0.0f || (m_pushed && m_pushDirection.y < 0.0f);
        public bool IsMovingLeft() => m_movementDirection.x < 0.0f || (m_pushed && m_pushDirection.x < 0.0f);
        public bool IsMovingRight() => m_movementDirection.x > 0.0f || (m_pushed && m_pushDirection.x > 0.0f);
        public bool IsMoving() => m_movementDirection.magnitude > 0.0f || (m_pushed && m_pushDirection.magnitude > 0.0f);

        public void SetLookAtDirection(Transform target)
        {
            Vector3 direction = target.position - transform.position;
            SetLookAtDirection(direction.x);
        }

        public void SetLookAtDirection(float direction)
        {
            if (direction != 0.0f)
            {
                SetLookAtDirection(direction >= 0.0f ? EDirection.Right : EDirection.Left);
            }
        }

        public void SetLookAtDirection(EDirection direction)
        {
            if (m_spriteRenderer)
            {
                bool wasFlipped = m_spriteRenderer.flipX;

                if ((m_spriteRenderer.flipX = direction == EDirection.Left) != wasFlipped)
                {
                    directionChangedEvent.Invoke(direction);
                }
            }
        }

        public EDirection GetLookAtDirection()
        {
            if (m_spriteRenderer)
            {
                return m_spriteRenderer.flipX ? EDirection.Left : EDirection.Right;
            }

            return EDirection.Default;
        }

        // TODO: Make Prettier
        private bool TryMove(Vector2 direction, float speed)
        {
            if (direction != Vector2.zero)
            {
                // Check for potential collisions
                int count = m_rigidbody.Cast(
                    direction, // X and Y values between -1 and 1 that represent the direction from the body to look for collisions
                    GameManager.Config.collisionContactFilter, // The settings that determine where a collision can occur on such as layers to collide with
                    m_castCollisions, // List of collisions to store the found collisions into after the Cast is finished
                    speed * Time.fixedDeltaTime + Constants.CollisionOffset); // The amount to cast equal to the movement plus an offset

                if (count == 0)
                {
                    m_lastSuccessfullMoveDirection = direction * speed;
                    m_rigidbody.MovePosition(m_rigidbody.position + direction * speed * Time.fixedDeltaTime);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                // Can't move if there's no direction to move in
                return false;
            }
        }

        public bool IsPushable()
        {
            return m_pushIntensityScale > 0.0f;
        }

        public bool IsBeingPushed()
        {
            return m_pushed;
        }

        public void InterruptPush()
        {
            m_pushed = false;
        }

        public void Push(Vector2 direction, float intensity = 5.0f, float resistance = 10.0f, bool faceOppositeDirection = false)
        {
            if (IsPushable())
            {
                m_pushed = true;
                m_rigidbody.velocity = Vector2.zero;
                m_pushIntensity = intensity * m_pushIntensityScale;
                m_pushResistance = resistance * m_pushResistanceScale;
                m_pushDirection = direction;

                float directionToFace = Mathf.Sign(direction.x) >= 0.0f ? -1.0f : 1.0f;

                if (faceOppositeDirection)
                {
                    directionToFace *= -1.0f;
                }

                SetLookAtDirection(directionToFace > 0 ? EDirection.Right : EDirection.Left);
            }
        }
        #endregion
    }
}
