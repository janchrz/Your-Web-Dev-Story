using UnityEngine;

namespace Gyvr.Mythril2D
{
    [System.Serializable]
    public struct UIEventSettings
    {
        public bool enabled;
        public string text;
        public Color color;
    }

    public class UIEventLog : MonoBehaviour
    {
        // Inspector Settings
        [Header("Global Parameters")]
        [SerializeField] private float m_logDuration = 3.0f;
        [SerializeField] private float m_characterTypingDuration = 0.025f;
        [SerializeField] private int m_linePoolSize = 5;
        [SerializeField] private GameObject m_linePrefab = null;

        [Header("Event Settings")]
        [SerializeField] private UIEventSettings m_experienceAdded;
        [SerializeField] private UIEventSettings m_levelUp;
        [SerializeField] private UIEventSettings m_moneyAdded;
        [SerializeField] private UIEventSettings m_moneyRemoved;
        [SerializeField] private UIEventSettings m_itemAdded;
        [SerializeField] private UIEventSettings m_itemRemoved;
        [SerializeField] private UIEventSettings m_questStarted;
        [SerializeField] private UIEventSettings m_questUpdated;
        [SerializeField] private UIEventSettings m_questCompleted;

        private UIEventLogLine[] m_lines = null;

        private void Awake()
        {
            m_lines = new UIEventLogLine[m_linePoolSize];

            for (int i = 0; i < m_linePoolSize; ++i)
            {
                GameObject instance = Instantiate(m_linePrefab, transform);
                m_lines[i] = instance.GetComponent<UIEventLogLine>();

                if (!m_lines[i])
                {
                    Debug.LogErrorFormat("The provided Line Prefab has no {0} component attached.", typeof(UIEventLogLine).Name);
                }
            }
        }

        private void Start()
        {
            GameManager.NotificationSystem.experienceGained.AddListener(OnExperienceGained);
            GameManager.NotificationSystem.levelUp.AddListener(OnLevelUp);
            GameManager.NotificationSystem.moneyAdded.AddListener(OnMoneyAdded);
            GameManager.NotificationSystem.moneyRemoved.AddListener(OnMoneyRemoved);
            GameManager.NotificationSystem.itemAdded.AddListener(OnItemAdded);
            GameManager.NotificationSystem.itemRemoved.AddListener(OnItemRemoved);
            GameManager.NotificationSystem.questStarted.AddListener(OnQuestStarted);
            GameManager.NotificationSystem.questProgressionUpdated.AddListener(OnQuestUpdated);
            GameManager.NotificationSystem.questCompleted.AddListener(OnQuestCompleted);
        }

        private void OnExperienceGained(int experience) => Log(m_experienceAdded, experience);
        private void OnLevelUp(int level) => Log(m_levelUp, level);
        private void OnMoneyAdded(int money) => Log(m_moneyAdded, money);
        private void OnMoneyRemoved(int money) => Log(m_moneyRemoved, money);
        private void OnItemAdded(Item item, int count) => Log(m_itemAdded, item.displayName, count);
        private void OnItemRemoved(Item item, int count) => Log(m_itemRemoved, item.displayName, count);
        private void OnQuestStarted(Quest quest) => Log(m_questStarted, quest.title);
        private void OnQuestUpdated(Quest quest) => Log(m_questUpdated, quest.title);
        private void OnQuestCompleted(Quest quest) => Log(m_questCompleted, quest.title);

        private void Log(UIEventSettings settings, params object[] args)
        {
            if (settings.enabled)
            {
                UIEventLogLine line = FindAvailableLine();

                if (line)
                {
                    line.Show(settings.color, StringFormatter.Format(settings.text, args), m_characterTypingDuration, m_logDuration);
                }
                else
                {
                    Debug.LogError("No available line, consider expanding the pool");
                }
            }
        }

        private UIEventLogLine FindAvailableLine()
        {
            foreach (UIEventLogLine line in m_lines)
            {
                if (!line.gameObject.activeSelf)
                {
                    return line;
                }
            }

            // If no inactive line has been found, return the first one in the hierarchy
            return transform.GetChild(0)?.GetComponent<UIEventLogLine>();
        }
    }
}
