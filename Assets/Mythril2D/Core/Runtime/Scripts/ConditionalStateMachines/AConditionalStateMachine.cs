using UnityEngine;

namespace Gyvr.Mythril2D
{
    public enum EConditionalState
    {
        None,
        Met,
        NotMet
    }

    public abstract class AConditionalStateMachine : MonoBehaviour
    {
        [SerializeField] private GameConditionData m_condition;

        public EConditionalState state => m_state;

        private EConditionalState m_state = EConditionalState.None;

        protected virtual void OnConditionMet() { }
        protected virtual void OnConditionNotMet() { }

        private void Start()
        {
            UpdateState();
            Subscribe();
        }

        private void Subscribe()
        {
            switch (m_condition.type)
            {
                default:
                case EGameConditionType.GameFlagSet:
                    GameManager.NotificationSystem.gameFlagChanged.AddListener(OnBooleanChanged);
                    break;
                case EGameConditionType.QuestActive:
                    GameManager.NotificationSystem.questStarted.AddListener(OnQuestStarted);
                    break;
                case EGameConditionType.QuestAvailable:
                    GameManager.NotificationSystem.questAvailable.AddListener(OnQuestAvailable);
                    break;
                case EGameConditionType.QuestCompleted:
                    GameManager.NotificationSystem.questCompleted.AddListener(OnQuestCompleted);
                    break;
            }
        }

        private void UpdateState()
        {
            EConditionalState newState = GameConditionChecker.IsConditionMet(m_condition) ? EConditionalState.Met : EConditionalState.NotMet;

            if (newState != m_state)
            {
                m_state = newState;

                switch (m_state)
                {
                    case EConditionalState.Met:
                        OnConditionMet();
                        break;

                    case EConditionalState.NotMet:
                        OnConditionNotMet();
                        break;
                }
            }
        }

        private void OnQuestStarted(Quest quest) => UpdateState();
        private void OnQuestAvailable(Quest quest) => UpdateState();
        private void OnQuestCompleted(Quest quest) => UpdateState();
        private void OnBooleanChanged(string id, bool value) => UpdateState();
    }
}
