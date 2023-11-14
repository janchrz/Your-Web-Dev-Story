using UnityEngine;

namespace Gyvr.Mythril2D
{
    public enum EGameConditionType
    {
        None,
        QuestActive,
        QuestAvailable,
        QuestFullfilled,
        QuestCompleted,
        TaskActive,
        GameFlagSet
    }

    public enum EGameConditionState
    {
        Is,
        Not
    }

    public enum EGameConditionOperation
    {
        AND,
        OR,
        XOR
    }

    [System.Serializable]
    public struct GameConditionSet
    {
        public EGameConditionOperation operation;
        public GameConditionData[] conditions;
    }

    [System.Serializable]
    public struct GameConditionData
    {
        public EGameConditionType type;
        public EGameConditionState state;
        public Quest quest;
        public QuestTask task;
        public string flagID;
    }

    public static class GameConditionChecker
    {
        public static bool IsConditionMet(GameConditionData data)
        {
            if (data.type == EGameConditionType.None)
            {
                return true;
            }

            bool isTrue = GetConditionState(data);
            return data.state == EGameConditionState.Not ? !isTrue : isTrue;
        }

        public static bool IsConditionSetMet(GameConditionSet conditionSet)
        {
            if (conditionSet.conditions == null)
            {
                Debug.LogWarningFormat("No conditons provided in the {0}", typeof(GameConditionSet).Name);
                return true;
            }

            switch (conditionSet.operation)
            {
                default:
                case EGameConditionOperation.AND: return IsConditionSetMet_AND(conditionSet);
                case EGameConditionOperation.OR: return IsConditionSetMet_OR(conditionSet);
                case EGameConditionOperation.XOR: return IsConditionSetMet_XOR(conditionSet);
            }
        }

        private static void ValidateConditionSet(GameConditionSet conditionSet)
        {
            Debug.AssertFormat(conditionSet.conditions != null, "Cannot evaluate a {0} without conditions", typeof(GameConditionSet).Name);
        }

        private static bool IsConditionSetMet_AND(GameConditionSet conditionSet)
        {
            ValidateConditionSet(conditionSet);

            foreach (GameConditionData condition in conditionSet.conditions)
            {
                if (!IsConditionMet(condition))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool IsConditionSetMet_OR(GameConditionSet conditionSet)
        {
            ValidateConditionSet(conditionSet);

            foreach (GameConditionData condition in conditionSet.conditions)
            {
                if (IsConditionMet(condition))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsConditionSetMet_XOR(GameConditionSet conditionSet)
        {
            ValidateConditionSet(conditionSet);

            bool found = false;

            foreach (GameConditionData condition in conditionSet.conditions)
            {
                if (IsConditionMet(condition))
                {
                    if (found)
                    {
                        return false;
                    }

                    found = true;
                }
            }

            return found;
        }

        private static bool GetConditionState(GameConditionData data)
        {
            switch (data.type)
            {
                default:
                case EGameConditionType.GameFlagSet: return EvaluateBoolean(data);
                case EGameConditionType.QuestActive: return EvaluateQuestActive(data);
                case EGameConditionType.QuestAvailable: return EvaluateQuestAvailable(data);
                case EGameConditionType.QuestFullfilled: return EvaluateQuestFullfilled(data);
                case EGameConditionType.QuestCompleted: return EvaluateQuestCompleted(data);
                case EGameConditionType.TaskActive: return EvaluateTaskActive(data);
            }
        }

        private static bool EvaluateQuestActive(GameConditionData data) => GameManager.JournalSystem.IsQuestActive(data.quest);
        private static bool EvaluateQuestAvailable(GameConditionData data) => GameManager.JournalSystem.IsQuestAvailable(data.quest);
        private static bool EvaluateQuestFullfilled(GameConditionData data) => GameManager.JournalSystem.HasFullfilledQuest(data.quest);
        private static bool EvaluateQuestCompleted(GameConditionData data) => GameManager.JournalSystem.HasCompletedQuest(data.quest);
        private static bool EvaluateTaskActive(GameConditionData data) => GameManager.JournalSystem.IsTaskActive(data.task);
        private static bool EvaluateBoolean(GameConditionData data) => GameManager.GameFlagSystem.Get(data.flagID);
    }
}
