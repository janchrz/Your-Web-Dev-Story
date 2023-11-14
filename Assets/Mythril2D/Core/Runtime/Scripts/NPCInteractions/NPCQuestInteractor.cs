using UnityEngine;

namespace Gyvr.Mythril2D
{
    public class NPCQuestInteractor : ANPCInteraction
    {
        [Header("References")]
        [SerializeField] private UINPCIcon m_npcIcon = null;

        private void Start()
        {
            GameManager.NotificationSystem.questAvailable.AddListener(OnQuestStatusChanged);
            GameManager.NotificationSystem.questCompleted.AddListener(OnQuestStatusChanged);
            GameManager.NotificationSystem.questFullfilled.AddListener(OnQuestStatusChanged);
            GameManager.NotificationSystem.questProgressionUpdated.AddListener(OnQuestStatusChanged);
            GameManager.NotificationSystem.questStarted.AddListener(OnQuestStatusChanged);

            UpdateIndicator();
        }

        private void OnDestroy()
        {
            GameManager.NotificationSystem.questAvailable.RemoveListener(OnQuestStatusChanged);
            GameManager.NotificationSystem.questCompleted.RemoveListener(OnQuestStatusChanged);
            GameManager.NotificationSystem.questFullfilled.RemoveListener(OnQuestStatusChanged);
            GameManager.NotificationSystem.questProgressionUpdated.RemoveListener(OnQuestStatusChanged);
            GameManager.NotificationSystem.questStarted.RemoveListener(OnQuestStatusChanged);
        }

        private void OnQuestStatusChanged(Quest quest) => UpdateIndicator();

        private void UpdateIndicator()
        {
            if (GameManager.JournalSystem.GetQuestToComplete(m_npc))
            {
                m_npcIcon.SetIconType(UINPCIcon.EIconType.QuestCompleted);
            }
            else if (GameManager.JournalSystem.GetQuestToStart(m_npc))
            {
                m_npcIcon.SetIconType(UINPCIcon.EIconType.QuestAvailable);
            }
            else if (GameManager.JournalSystem.GetNonFullfilledQuestToReportTo(m_npc) != null)
            {
                m_npcIcon.SetIconType(UINPCIcon.EIconType.QuestInProgress);
            }
            else
            {
                m_npcIcon.SetIconType(UINPCIcon.EIconType.None);
            }
        }

        private bool TryCompletingQuest()
        {
            Quest quest = GameManager.JournalSystem.GetQuestToComplete(m_npc);

            if (quest)
            {
                if (quest.questCompletedDialogue != null)
                {
                    Say(quest.questCompletedDialogue, (actionFeed) =>
                    {
                        GameManager.JournalSystem.CompleteQuest(quest);
                    });

                    return true;
                }
                else
                {
                    Debug.LogErrorFormat("No quest completed dialogue provided for [{0}]", quest.title);
                }
            }

            return false;
        }

        private bool TryGivingHint()
        {
            // Try to find a hint for a fullfilled quest (quest with no task, such as "Talk to X")
            Quest quest = GameManager.JournalSystem.GetFullfilledQuest(m_npc);

            if (!quest)
            {
                // Try to find a hint for a started quest
                quest = GameManager.JournalSystem.GetStartedQuest(m_npc);
            }

            if (quest != null && quest.questHintDialogue != null)
            {
                Say(quest.questHintDialogue);
                return true;
            }

            return false;
        }

        private bool TryOfferingQuest()
        {
            Quest quest = GameManager.JournalSystem.GetQuestToStart(m_npc);

            if (quest)
            {
                if (quest.questOfferDialogue != null)
                {
                    Say(quest.questOfferDialogue, (messages) =>
                    {
                        if (messages.Contains(EDialogueMessageType.Accept))
                        {
                            GameManager.JournalSystem.StartQuest(quest);
                        }
                    });

                    return true;
                }
                else
                {
                    Debug.LogErrorFormat("No quest offer dialogue provided for [{0}]", quest.title);
                }
            }

            return false;
        }

        public override bool Interact(CharacterBase sender)
        {
            if (!TryCompletingQuest())
            {
                if (!TryOfferingQuest())
                {
                    if (!TryGivingHint())
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
