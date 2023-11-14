using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    [Serializable]
    public struct QuestInstanceDataBlock
    {
        public Quest quest;
        [SerializeReference] public QuestTaskProgress[] completedTasks;
        [SerializeReference] public QuestTaskProgress[] currentTasks;
        public QuestTask[] nextTasks;
    }

    [Serializable]
    public class QuestInstance : IDataBlockHandler<QuestInstanceDataBlock>
    {
        // Public Getters
        public List<QuestTaskProgress> completedTasks => m_completedTasks;
        public List<QuestTaskProgress> currentTasks => m_currentTasks;
        public Quest quest => m_quest;

        // Private Members
        private Quest m_quest = null;
        private List<QuestTaskProgress> m_completedTasks = new List<QuestTaskProgress>();
        private List<QuestTaskProgress> m_currentTasks = new List<QuestTaskProgress>();
        private Queue<QuestTask> m_nextTasks = new Queue<QuestTask>();
        private Action<QuestInstance> m_fullfilledCallback;

        public QuestInstance(Quest quest, Action<QuestInstance> fullfilledCallback)
        {
            m_quest = quest;
            m_fullfilledCallback = fullfilledCallback;

            foreach (QuestTask task in quest.tasks)
            {
                m_nextTasks.Enqueue(task);
            }

            UpdateCurrentTasks();
        }

        public QuestInstance(QuestInstanceDataBlock block, Action<QuestInstance> fullfilledCallback)
        {
            m_fullfilledCallback = fullfilledCallback;
            LoadDataBlock(block);
        }

        public void LoadDataBlock(QuestInstanceDataBlock block)
        {
            m_quest = block.quest;
            m_completedTasks = new List<QuestTaskProgress>(block.completedTasks);
            m_currentTasks = new List<QuestTaskProgress>(block.currentTasks);
            m_nextTasks = new Queue<QuestTask>(block.nextTasks);

            foreach (QuestTaskProgress task in m_currentTasks)
            {
                task.Initialize(OnTaskCompleted);
            }
        }

        public QuestInstanceDataBlock CreateDataBlock()
        {
            return new QuestInstanceDataBlock
            {
                quest = m_quest,
                completedTasks = m_completedTasks.ToArray(),
                currentTasks = m_currentTasks.ToArray(),
                nextTasks = m_nextTasks.ToArray()
            };
        }

        public void CheckFullfillment()
        {
            if (m_currentTasks.Count == 0 && m_nextTasks.Count == 0)
            {
                m_fullfilledCallback(this);
            }
        }

        public void UpdateCurrentTasks()
        {
            while (m_nextTasks.Count > 0)
            {
                QuestTask task = m_nextTasks.Dequeue();
                QuestTaskProgress taskProgress = task.CreateTaskProgress();
                m_currentTasks.Add(taskProgress);
                taskProgress.Initialize(OnTaskCompleted);

                // If the next task requires previous tasks to be completed to become available, stop dequeuing tasks
                if (m_nextTasks.Count > 0 && m_nextTasks.Peek().requirePreviousTaskCompletion)
                {
                    return;
                }
            }
        }

        private void OnTaskCompleted(QuestTaskProgress taskProgress)
        {
            m_currentTasks.Remove(taskProgress);
            m_completedTasks.Add(taskProgress);
            GameManager.NotificationSystem.questProgressionUpdated.Invoke(quest);

            if (m_currentTasks.Count == 0)
            {
                if (m_nextTasks.Count > 0)
                {
                    UpdateCurrentTasks();
                }
                else
                {
                    m_fullfilledCallback(this);
                }
            }
        }

        public void CompleteTask(QuestTask task)
        {
            foreach (QuestTaskProgress taskProgress in m_currentTasks)
            {
                if (taskProgress.task == task)
                {
                    OnTaskCompleted(taskProgress);
                }
            }
        }
    }
}
