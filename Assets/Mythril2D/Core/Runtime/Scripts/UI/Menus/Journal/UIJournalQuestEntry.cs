using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Gyvr.Mythril2D
{
    public struct JournalQuestEntry
    {
        public Quest quest;
        public QuestInstance instance;
    }

    public class UIJournalQuestEntry : MonoBehaviour, ISelectHandler
    {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI m_text = null;

        // Private Members
        private Quest m_targetQuest = null;
        private QuestInstance m_targetQuestInstance = null;

        public void SetTargetQuest(Quest quest, QuestInstance questInstance = null)
        {
            m_targetQuest = quest;
            m_targetQuestInstance = questInstance;

            if (quest)
            {
                m_text.text = StringFormatter.Format("[Lvl. {0}] {1}", quest.recommendedLevel, quest.title);
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        public void Hide()
        {
            SetTargetQuest(null);
        }

        public void OnSelect(BaseEventData eventData)
        {
            SendMessageUpwards("UpdateQuestDescription", new JournalQuestEntry
            {
                quest = m_targetQuest,
                instance = m_targetQuestInstance
            }, SendMessageOptions.RequireReceiver);
        }
    }
}
