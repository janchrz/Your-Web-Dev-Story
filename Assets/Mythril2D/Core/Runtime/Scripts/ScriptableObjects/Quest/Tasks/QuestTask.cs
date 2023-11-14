using UnityEngine;

namespace Gyvr.Mythril2D
{
    public abstract class QuestTask : ScriptableObject
    {
        [SerializeField] protected string m_title = string.Empty;
        [SerializeField] protected bool m_requirePreviousTasksCompletion = false;

        public bool requirePreviousTaskCompletion => m_requirePreviousTasksCompletion;

        public abstract QuestTaskProgress CreateTaskProgress();
        public abstract string GetTitle();
        public abstract string GetTitle(QuestTaskProgress progress);
    }
}