using System;
using System.Linq;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    [CreateAssetMenu(menuName = AssetMenuIndexer.Mythril2D_ScriptableActions + nameof(FullfillTaskAction))]
    public class FullfillTaskAction : ScriptableAction
    {
        public override Type[] RequiredArgs => new Type[] { typeof(QuestTask) };
        public override Type[] OptionalArgs => new Type[] { };
        public override string[] ArgDescriptions => new string[] { "Task to fullfill" };

        public override void Execute(params object[] args)
        {
            QuestTask task = GetRequiredArgAtIndex<QuestTask>(0, args);

            foreach (QuestInstance questInstance in GameManager.JournalSystem.activeQuests)
            {
                questInstance.CompleteTask(task);
            }
        }
    }
}
