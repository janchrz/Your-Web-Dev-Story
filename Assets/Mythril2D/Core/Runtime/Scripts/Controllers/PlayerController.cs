using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gyvr.Mythril2D
{
    [RequireComponent(typeof(CharacterBase))]
    public class PlayerController : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] private Transform m_overrideInteractionPivot = null;
        [SerializeField] private float m_interactionDistance = 0.75f;

        [Header("Audio")]
        [SerializeField] private AudioClipResolver m_interactionSound;

        public GameObject interactionTarget => m_interactionTarget;

        private CharacterBase m_character = null;

        private Vector2 m_movementDirection;
        private Transform m_interactionPivot = null;

        private GameObject m_interactionTarget = null;

        private void Awake()
        {
            m_character = GetComponent<CharacterBase>();

            if (m_overrideInteractionPivot)
            {
                m_interactionPivot = m_overrideInteractionPivot;
            }
            else
            {
                m_interactionPivot = transform;
            }

            m_movementDirection = Vector2.right;
        }

        private void Start()
        {
            GameManager.InputSystem.gameplay.interact.performed += OnInteract;
            GameManager.InputSystem.gameplay.fireAbility1.performed += OnFireAbility1;
            GameManager.InputSystem.gameplay.fireAbility2.performed += OnFireAbility2;
            GameManager.InputSystem.gameplay.fireAbility3.performed += OnFireAbility3;
            GameManager.InputSystem.gameplay.move.performed += OnMove;
            GameManager.InputSystem.gameplay.move.canceled += OnStoppedMoving;
            GameManager.InputSystem.gameplay.openGameMenu.performed += OnOpenGameMenu;
        }

        private void OnDestroy()
        {
            GameManager.InputSystem.gameplay.interact.performed -= OnInteract;
            GameManager.InputSystem.gameplay.fireAbility1.performed -= OnFireAbility1;
            GameManager.InputSystem.gameplay.fireAbility2.performed -= OnFireAbility2;
            GameManager.InputSystem.gameplay.fireAbility3.performed -= OnFireAbility3;
            GameManager.InputSystem.gameplay.move.performed -= OnMove;
            GameManager.InputSystem.gameplay.move.canceled -= OnStoppedMoving;
            GameManager.InputSystem.gameplay.openGameMenu.performed -= OnOpenGameMenu;
        }

        protected AbilitySheet GetAbilityAtIndex(int index)
        {
            return GameManager.Player.equippedAbilities[index];
        }

        private void Update()
        {
            m_interactionTarget = GetInteractibleObject();
        }

        private GameObject GetInteractibleObject()
        {
            if (m_character.Can(EActionFlags.Interact))
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(m_interactionPivot.position, m_interactionDistance, LayerMask.GetMask(GameManager.Config.interactionLayer));
                Array.Sort(colliders, (x, y) =>
                {
                    return Vector3.Distance(m_interactionPivot.position, x.transform.position).CompareTo(
                        Vector3.Distance(m_interactionPivot.position, y.transform.position));
                });
                foreach (Collider2D collider in colliders)
                {
                    Vector3 a = m_movementDirection;
                    Vector3 b = (collider.transform.position + new Vector3(collider.offset.x, collider.offset.y, 0)) - m_interactionPivot.position;
                    if (Vector3.Dot(a, b) > 0)
                    {
                        return collider.gameObject;
                    }
                }
            }

            return null;
        }

        private bool TryInteracting()
        {
            GameObject interactionTarget = GetInteractibleObject();

            if (interactionTarget)
            {
                GameManager.NotificationSystem.audioPlaybackRequested.Invoke(m_interactionSound);
                interactionTarget.SendMessageUpwards("OnInteract", m_character);
                return true;
            }

            return false;
        }

        private void OnOpenGameMenu(InputAction.CallbackContext context)
        {
            GameManager.NotificationSystem.gameMenuRequested.Invoke();
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            Vector2 direction = context.ReadValue<Vector2>();

            m_character.SetMovementDirection(direction);

            if (direction.magnitude > 0.01f)
            {
                m_movementDirection = direction;
            }
        }

        private void OnStoppedMoving(InputAction.CallbackContext context)
        {
            m_character.SetMovementDirection(Vector2.zero);
        }

        private void OnInteract(InputAction.CallbackContext context) => TryInteracting();

        private void OnFireAbility1(InputAction.CallbackContext context) => FireAbilityAtIndex(0);
        private void OnFireAbility2(InputAction.CallbackContext context) => FireAbilityAtIndex(1);
        private void OnFireAbility3(InputAction.CallbackContext context) => FireAbilityAtIndex(2);

        private void FireAbilityAtIndex(int i)
        {
            AbilitySheet selectedAbility = GetAbilityAtIndex(i);

            if (selectedAbility != null)
            {
                m_character.FireAbility(selectedAbility);
            }
        }
    }
}
