using UnityEngine;

namespace Gyvr.Mythril2D
{
    public class UIJournal : MonoBehaviour, IUIMenu
    {
        [SerializeField] private UIJournalQuestDescription m_questDescription = null;
        [SerializeField] private Transform m_questListRoot = null;
        [SerializeField] private GameObject m_questEntryPrefab = null;
        [SerializeField] private int m_questEntryPoolSize = 100;

        private UIJournalQuestEntry[] m_quests = null;

        public void Init()
        {
            m_quests = new UIJournalQuestEntry[m_questEntryPoolSize];

            for (int i = 0; i < m_questEntryPoolSize; ++i)
            {
                GameObject instance = Instantiate(m_questEntryPrefab, m_questListRoot);
                m_quests[i] = instance.GetComponent<UIJournalQuestEntry>();
            }
        }

        public void Show(params object[] args)
        {
            UpdateUI();
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void EnableInteractions(bool enable)
        {
            CanvasGroup canvasGroup = GetComponent<CanvasGroup>();

            if (canvasGroup)
            {
                canvasGroup.interactable = enable;
            }
        }

        public GameObject FindSomethingToSelect()
        {
            return m_quests.Length > 0 ? m_quests[0].gameObject : null;
        }

        private void UpdateUI()
        {
            int entryCount = m_quests.Length;
            int usedQuestEntries = 0;

            foreach (Quest quest in GameManager.JournalSystem.fullfilledQuests)
            {
                if (usedQuestEntries < entryCount)
                {
                    UIJournalQuestEntry availableText = m_quests[usedQuestEntries++];
                    availableText.SetTargetQuest(quest);
                }
                else
                {
                    Debug.LogError("Not enough quest texts allocated");
                }
            }

            foreach (QuestInstance quest in GameManager.JournalSystem.activeQuests)
            {
                if (usedQuestEntries < entryCount)
                {
                    UIJournalQuestEntry availableText = m_quests[usedQuestEntries++];
                    availableText.SetTargetQuest(quest.quest, quest);
                }
                else
                {
                    Debug.LogError("Not enough quest texts allocated");
                }
            }

            // Disable remaining texts
            for (int i = usedQuestEntries; i < entryCount; ++i)
            {
                m_quests[i].Hide();
            }

            m_questDescription.UpdateUI();
        }

        private void UpdateQuestDescription(JournalQuestEntry entry)
        {
            m_questDescription.SetTargetQuest(entry.quest, entry.instance);
            UpdateUI();
        }
    }
}
