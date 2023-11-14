using System.Collections.Generic;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    public enum EGameState
    {
        None,
        Menu,
        Dialogue,
        Gameplay
    }

    public class GameStateSystem : AGameSystem
    {
        [SerializeField] private EGameState m_startupState = EGameState.Gameplay;

        public EGameState currentState => m_stateStack.Count > 0 ? m_stateStack.Peek() : EGameState.None;

        private Stack<EGameState> m_stateStack = new Stack<EGameState>();

        public override void OnSystemStart()
        {
            AddLayer(m_startupState);
        }

        public void RemoveLayer(EGameState state)
        {
            Debug.AssertFormat(m_stateStack.Peek() == state, "Failed removing layer {0}. Make sure the layer you tried removing is at the top of the state stack", state);
            OnExitState(m_stateStack.Pop());

            if (m_stateStack.Count > 0)
            {
                OnEnterState(m_stateStack.Peek());
            }
        }

        public void AddLayer(EGameState state)
        {
            m_stateStack.Push(state);
            OnEnterState(m_stateStack.Peek());
        }

        private void OnEnterState(EGameState state)
        {
            OnStateChanged();

            switch (state)
            {
                case EGameState.Menu: OnEnterMenuState(); break;
                case EGameState.Dialogue: OnEnterDialogueState(); break;
                case EGameState.Gameplay: OnEnterGameplayState(); break;
            }
        }

        private void OnExitState(EGameState state)
        {
            OnStateChanged();

            switch (state)
            {
                case EGameState.Menu: OnExitMenuState(); break;
                case EGameState.Dialogue: OnExitDialogueState(); break;
                case EGameState.Gameplay: OnExitGameplayState(); break;
            }
        }

        private void OnStateChanged()
        {
            // Prevent action map change when using shared action keys between action maps to toggle multiple actions in one frame
            StartCoroutine(CoroutineHelpers.ExecuteInXFrames(1, () =>
            {
                switch (currentState)
                {
                    case EGameState.Gameplay:
                        GameManager.InputSystem.SetActionMap(EActionMap.Gameplay);
                        break;

                    case EGameState.Dialogue:
                        GameManager.InputSystem.SetActionMap(EActionMap.UI);
                        break;

                    case EGameState.Menu:
                        GameManager.InputSystem.SetActionMap(EActionMap.UI);
                        break;
                }
            }));
        }

        private void OnEnterMenuState() => Time.timeScale = 0.0f;
        private void OnEnterGameplayState() => Time.timeScale = 1.0f;

        private void OnEnterDialogueState() { }
        private void OnExitMenuState() { }
        private void OnExitGameplayState() { }
        private void OnExitDialogueState() { }
    }
}
