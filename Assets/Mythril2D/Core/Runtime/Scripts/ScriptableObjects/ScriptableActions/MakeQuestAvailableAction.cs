using System;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    [CreateAssetMenu(menuName = AssetMenuIndexer.Mythril2D_ScriptableActions + nameof(MakeQuestAvailableAction))]
    public class MakeQuestAvailableAction : ScriptableAction
    {
        public override Type[] RequiredArgs => new Type[] { typeof(Quest) };
        public override Type[] OptionalArgs => new Type[] { };
        public override string[] ArgDescriptions => new string[] { "Quest to make available" };

        public override void Execute(params object[] args)
        {
            Quest quest = GetRequiredArgAtIndex<Quest>(0, args);

            GameManager.JournalSystem.MakeQuestAvailable(quest);
        }
    }
}
