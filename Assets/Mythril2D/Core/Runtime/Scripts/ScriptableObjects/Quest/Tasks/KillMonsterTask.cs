using UnityEngine;

namespace Gyvr.Mythril2D
{
    public class KillMonsterTaskProgress : QuestTaskProgress
    {
        public int monstersKilled { get; private set; } = 0;

        private KillMonsterTask m_task => (KillMonsterTask)task;

        public KillMonsterTaskProgress(QuestTask task) : base(task)
        {
        }

        public override void OnProgressTrackingStarted()
        {
            GameManager.NotificationSystem.monsterKilled.AddListener(OnMonsterKilled);
        }

        public override void OnProgressTrackingStopped()
        {
            GameManager.NotificationSystem.monsterKilled.RemoveListener(OnMonsterKilled);
        }

        public override bool IsCompleted()
        {
            return monstersKilled >= m_task.monstersToKill;
        }

        private void OnMonsterKilled(MonsterSheet monster)
        {
            if (monster == m_task.monster)
            {
                ++monstersKilled;
                UpdateProgression();
            }
        }
    }

    [CreateAssetMenu(menuName = AssetMenuIndexer.Mythril2D_Quests_Tasks + nameof(KillMonsterTask))]
    public class KillMonsterTask : QuestTask
    {
        public MonsterSheet monster = null;
        public int monstersToKill = 1;

        public KillMonsterTask()
        {
            m_title = "Kill {0} ({1}/{2})";
        }

        public override QuestTaskProgress CreateTaskProgress() => new KillMonsterTaskProgress(this);

        public override string GetTitle()
        {
            return StringFormatter.Format(m_title, monster.displayName, monstersToKill, monstersToKill);
        }

        public override string GetTitle(QuestTaskProgress progress)
        {
            return StringFormatter.Format(m_title, monster.displayName, ((KillMonsterTaskProgress)progress).monstersKilled, monstersToKill);
        }
    }
}
