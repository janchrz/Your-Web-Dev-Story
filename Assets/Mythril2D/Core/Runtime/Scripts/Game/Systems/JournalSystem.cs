using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    [Serializable]
    public struct JournalDataBlock
    {
        public List<Quest> availableQuests;
        public List<QuestInstanceDataBlock> activeQuests;
        public List<Quest> fullfilledQuests;
        public List<Quest> completedQuests;
    }

    public class JournalSystem : AGameSystem, IDataBlockHandler<JournalDataBlock>
    {
        [Header("Audio")]
        [SerializeField] private AudioClipResolver m_questStartedSound;
        [SerializeField] private AudioClipResolver m_questCompletedSound;

        public List<Quest> availableQuests => m_availableQuests;
        public List<QuestInstance> activeQuests => m_activeQuests;
        public List<Quest> fullfilledQuests => m_fullfilledQuests;
        public List<Quest> completedQuests => m_completedQuests;

        public bool IsQuestAvailable(Quest quest) => availableQuests.Contains(quest);
        public bool IsQuestActive(Quest quest) => m_activeQuests.Find((QuestInstance questInstance) => questInstance.quest == quest) != null;
        public bool HasFullfilledQuest(Quest quest) => fullfilledQuests.Contains(quest);
        public bool HasCompletedQuest(Quest quest) => completedQuests.Contains(quest);

        public bool IsTaskActive(QuestTask task)
        {
            foreach (QuestInstance questInstance in activeQuests)
            {
                if (questInstance.currentTasks.Find((taskProgress) => taskProgress.task == task) != null)
                {
                    return true;
                }
            }

            return false;
        }

        private List<Quest> m_availableQuests = new List<Quest>();
        private List<QuestInstance> m_activeQuests = new List<QuestInstance>();
        private List<Quest> m_fullfilledQuests = new List<Quest>();
        private List<Quest> m_completedQuests = new List<Quest>();

        public void StartQuest(Quest quest)
        {
            QuestInstance instance = new QuestInstance(quest, OnQuestFullfilled);
            availableQuests.Remove(quest);
            activeQuests.Add(instance);
            GameManager.NotificationSystem.questStarted.Invoke(quest);
            GameManager.NotificationSystem.audioPlaybackRequested.Invoke(m_questStartedSound);

            // Some quests have no tasks, and thus should be considered as fullfilled right away
            instance.CheckFullfillment();
        }

        public void OnQuestFullfilled(QuestInstance instance)
        {
            fullfilledQuests.Add(instance.quest);
            activeQuests.Remove(instance);
            GameManager.NotificationSystem.questFullfilled.Invoke(instance.quest);
        }

        public void CompleteQuest(Quest quest)
        {
            fullfilledQuests.Remove(quest);
            completedQuests.Add(quest);

            GameManager.NotificationSystem.questCompleted.Invoke(quest);
            GameManager.NotificationSystem.audioPlaybackRequested.Invoke(m_questCompletedSound);

            if (quest.repeatable)
            {
                MakeQuestAvailable(quest);
            }

            foreach (ActionHandler action in quest.toExecuteOnQuestCompletion)
            {
                action.Execute();
            }
        }

        public void MakeQuestAvailable(Quest quest)
        {
            availableQuests.Add(quest);
            GameManager.NotificationSystem.questAvailable.Invoke(quest);
        }

        public QuestInstance GetNonFullfilledQuestToReportTo(NPC npc)
        {
            return activeQuests.Find((quest) => quest.quest.reportTo == npc.characterSheet);
        }

        public Quest GetQuestToComplete(NPC npc)
        {
            return fullfilledQuests.Find((quest) => quest.reportTo == npc.characterSheet);
        }

        public Quest GetQuestToStart(NPC npc)
        {
            return availableQuests.Find((quest) => quest.offeredBy == npc.characterSheet);
        }

        public Quest GetStartedQuest(NPC npc)
        {
            List<QuestInstance> results = activeQuests.FindAll((quest) => quest.quest.offeredBy == npc.characterSheet);
            return results != null && results.Count > 0 ? results[0].quest : null;
        }

        public Quest GetFullfilledQuest(NPC npc)
        {
            List<Quest> results = fullfilledQuests.FindAll((quest) => quest.offeredBy == npc.characterSheet);
            return results != null && results.Count > 0 ? results[0] : null;
        }

        public void LoadDataBlock(JournalDataBlock block)
        {
            m_availableQuests = block.availableQuests;
            m_activeQuests = new List<QuestInstance>(block.activeQuests.Count);
            m_fullfilledQuests = block.fullfilledQuests;
            m_completedQuests = block.completedQuests;

            foreach (QuestInstanceDataBlock qidb in block.activeQuests)
            {
                QuestInstance questInstance = new QuestInstance(qidb, OnQuestFullfilled);
                m_activeQuests.Add(questInstance);
                questInstance.CheckFullfillment();
            }
        }

        public JournalDataBlock CreateDataBlock()
        {
            return new JournalDataBlock
            {
                availableQuests = m_availableQuests,
                activeQuests = new List<QuestInstanceDataBlock>(m_activeQuests.Select(qi => qi.CreateDataBlock())),
                fullfilledQuests = m_fullfilledQuests,
                completedQuests = m_completedQuests
            };
        }
    }
}