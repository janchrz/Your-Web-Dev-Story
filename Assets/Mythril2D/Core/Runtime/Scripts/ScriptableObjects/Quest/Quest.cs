using UnityEngine;

namespace Gyvr.Mythril2D
{
    [CreateAssetMenu(menuName = AssetMenuIndexer.Mythril2D_Quests + nameof(Quest))]
    public class Quest : ScriptableObject
    {
        [Header("Details")]
        [SerializeField] private string m_title = string.Empty;
        [SerializeField][TextArea] private string m_description = string.Empty;
        [SerializeField][Range(Stats.MinLevel, Stats.MaxLevel)] private int m_recommendedLevel = 1;
        [SerializeField] private bool m_repeatable = false;
        [SerializeField] private QuestTask[] m_tasks = null;

        [Header("Completion")]
        [SerializeField] private ActionHandler[] m_toExecuteOnQuestCompletion = null;

        [Header("Related NPCs")]
        [SerializeField] private NPCSheet m_offeredBy = null;
        [SerializeField] private NPCSheet m_reportTo = null;

        [Header("Dialogues")]
        [SerializeField] private DialogueSequence m_questOfferDialogue = null;
        [SerializeField] private DialogueSequence m_questHintDialogue = null;
        [SerializeField] private DialogueSequence m_questCompletedDialogue = null;

        public QuestTask[] tasks => m_tasks;
        public string title => m_title;
        public string description => m_description;
        public int recommendedLevel => m_recommendedLevel;
        public bool repeatable => m_repeatable;
        public NPCSheet offeredBy => m_offeredBy;
        public NPCSheet reportTo => m_reportTo;
        public DialogueSequence questOfferDialogue => m_questOfferDialogue;
        public DialogueSequence questHintDialogue => m_questHintDialogue;
        public DialogueSequence questCompletedDialogue => m_questCompletedDialogue;
        public ActionHandler[] toExecuteOnQuestCompletion => m_toExecuteOnQuestCompletion;
    }
}
