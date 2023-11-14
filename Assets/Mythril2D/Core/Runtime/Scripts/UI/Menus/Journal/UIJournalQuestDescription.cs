using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    public class UIJournalQuestDescription : MonoBehaviour
    {
        // Component References
        [SerializeField] private TextMeshProUGUI m_description = null;
        [SerializeField] private TextMeshProUGUI m_currentTasks = null;
        [SerializeField] private TextMeshProUGUI m_completedTasks = null;

        // Private Members
        private Quest m_targetQuest = null;
        private QuestInstance m_targetQuestInstance = null;

        private void Awake()
        {
            Clear();
        }

        private void OnEnable()
        {
            Clear();
        }

        public void SetTargetQuest(Quest quest, QuestInstance questInstance = null)
        {
            m_targetQuest = quest;
            m_targetQuestInstance = questInstance;

            UpdateDescription();
        }

        public void Clear()
        {
            SetTargetQuest(null);
        }

        private string FormatQuestTasks(IEnumerable<QuestTaskProgress> tasks)
        {
            string result = string.Empty;

            foreach (QuestTaskProgress task in tasks)
            {
                if (result.Length > 0)
                {
                    result += "\n";
                }

                result += task.task.GetTitle(task);
            }

            return result;
        }

        private string FormatQuestTasks(IEnumerable<QuestTask> tasks)
        {
            string result = string.Empty;

            foreach (QuestTask task in tasks)
            {
                if (result.Length > 0)
                {
                    result += "\n";
                }

                result += task.GetTitle();
            }

            return result;
        }

        private void UpdateDescription()
        {
            if (m_targetQuest)
            {
                m_description.text = m_targetQuest.description;

                if (m_targetQuestInstance != null)
                {
                    // Active quests
                    m_currentTasks.text = FormatQuestTasks(m_targetQuestInstance.currentTasks);
                    m_completedTasks.text = FormatQuestTasks(m_targetQuestInstance.completedTasks);
                }
                else
                {
                    // Fullfilled quests
                    m_currentTasks.text = StringFormatter.Format("Talk to {0}", m_targetQuest.reportTo.displayName);
                    m_completedTasks.text = FormatQuestTasks(m_targetQuest.tasks);
                }
            }
            else
            {
                m_description.text = string.Empty;
                m_currentTasks.text = string.Empty;
                m_completedTasks.text = string.Empty;
            }
        }

        public void UpdateUI()
        {
            UpdateDescription();
        }
    }
}
