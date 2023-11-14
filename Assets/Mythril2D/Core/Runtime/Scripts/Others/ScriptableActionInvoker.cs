using UnityEngine;

namespace Gyvr.Mythril2D
{
    public class ScriptableActionInvoker : MonoBehaviour
    {
        public enum EActivationEvent
        {
            OnStart,
            OnEnable,
            OnDisable,
            OnDestroy,
            OnUpdate,
            OnPlayerEnterTrigger,
            OnPlayerExitTrigger,
            OnPlayerInteract,
        }

        [Header("Requirements")]
        [SerializeField] private EActivationEvent m_activationEvent;
        [SerializeField] private GameConditionSet m_conditions;

        [Header("Actions")]
        [SerializeField] private ActionHandler[] m_toExecute;

        [Header("Settings")]
        [SerializeField] private int m_frameDelay = 0;

        private void AttemptExecution(EActivationEvent currentEvent, GameObject go = null)
        {
            if (currentEvent == m_activationEvent && (!go || go == GameManager.Player.gameObject) && GameConditionChecker.IsConditionSetMet(m_conditions))
            {
                if (m_frameDelay <= 0)
                {
                    ExecuteActions();
                }
                else
                {
                    StartCoroutine(CoroutineHelpers.ExecuteInXFrames(m_frameDelay, ExecuteActions));
                }
            }
        }

        private void ExecuteActions()
        {
            if (m_toExecute != null)
            {
                foreach (ActionHandler handler in m_toExecute)
                {
                    handler.Execute();
                }
            }
        }

        private void Start() => AttemptExecution(EActivationEvent.OnStart);
        private void Update() => AttemptExecution(EActivationEvent.OnUpdate);
        private void OnEnable() => AttemptExecution(EActivationEvent.OnEnable);
        private void OnDisable() => AttemptExecution(EActivationEvent.OnDisable);
        private void OnDestroy() => AttemptExecution(EActivationEvent.OnDestroy);
        private void OnTriggerEnter2D(Collider2D collider) => AttemptExecution(EActivationEvent.OnPlayerEnterTrigger, collider.gameObject);
        private void OnTriggerExit2D(Collider2D collider) => AttemptExecution(EActivationEvent.OnPlayerExitTrigger, collider.gameObject);
        private void OnInteract(CharacterBase sender) => AttemptExecution(EActivationEvent.OnPlayerInteract, sender.gameObject);
    }
}