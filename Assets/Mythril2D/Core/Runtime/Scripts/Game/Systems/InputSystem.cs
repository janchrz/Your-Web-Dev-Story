using UnityEngine;
using UnityEngine.InputSystem;

namespace Gyvr.Mythril2D
{
    public struct GameplayActions
    {
        public InputAction move;
        public InputAction interact;
        public InputAction fireAbility1;
        public InputAction fireAbility2;
        public InputAction fireAbility3;
        public InputAction openGameMenu;
    }

    public struct UIActions
    {
        public InputAction cancel;
        public InputAction skip;
        public InputAction navigate;
    }

    public enum EActionMap
    {
        Gameplay,
        UI
    }

    [RequireComponent(typeof(PlayerInput))]
    public class InputSystem : AGameSystem
    {
        public GameplayActions gameplay => m_gameplayActions;
        public UIActions ui => m_uiActions;
        public PlayerInput playerInput => m_playerInput;

        private PlayerInput m_playerInput = null;

        private GameplayActions m_gameplayActions;
        private UIActions m_uiActions;

        public override void OnSystemInit()
        {
            m_playerInput = GetComponent<PlayerInput>();
            SetupGameplayActions(m_playerInput.actions.FindActionMap("Gameplay"));
            SetupUIActions(m_playerInput.actions.FindActionMap("UI"));
        }

        public override void OnSystemStart()
        {
            GameManager.NotificationSystem.mapTransitionStarted.AddListener(LockInputs);
            GameManager.NotificationSystem.mapTransitionCompleted.AddListener(UnlockInputs);
        }

        public override void OnSystemStop()
        {
            GameManager.NotificationSystem.mapTransitionStarted.RemoveListener(LockInputs);
            GameManager.NotificationSystem.mapTransitionCompleted.RemoveListener(UnlockInputs);
        }

        private void LockInputs()
        {
            m_playerInput.DeactivateInput();
        }

        private void UnlockInputs()
        {
            m_playerInput.ActivateInput();
        }

        public void SetActionMap(EActionMap actionMap)
        {
            m_playerInput.SwitchCurrentActionMap(actionMap.ToString());
        }

        private void SetupGameplayActions(InputActionMap actions)
        {
            m_gameplayActions = new GameplayActions
            {
                interact = actions.FindAction("Interact"),
                fireAbility1 = actions.FindAction("FireAbility1"),
                fireAbility2 = actions.FindAction("FireAbility2"),
                fireAbility3 = actions.FindAction("FireAbility3"),
                move = actions.FindAction("Move"),
                openGameMenu = actions.FindAction("OpenGameMenu")
            };
        }

        private void SetupUIActions(InputActionMap actions)
        {
            m_uiActions = new UIActions
            {
                cancel = actions.FindAction("Cancel"),
                skip = actions.FindAction("Skip"),
                navigate = actions.FindAction("Navigate")
            };
        }
    }
}
