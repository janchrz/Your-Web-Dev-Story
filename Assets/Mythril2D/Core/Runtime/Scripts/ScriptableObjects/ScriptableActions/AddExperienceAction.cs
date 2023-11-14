using System;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    [CreateAssetMenu(menuName = AssetMenuIndexer.Mythril2D_ScriptableActions + nameof(AddExperienceAction))]
    public class AddExperienceAction : ScriptableAction
    {
        public override Type[] RequiredArgs => new Type[] { typeof(int) };
        public override Type[] OptionalArgs => new Type[] { };
        public override string[] ArgDescriptions => new string[] { "Amount of experience to add" };

        public override void Execute(params object[] args)
        {
            int experience = GetRequiredArgAtIndex<int>(0, args);

            GameManager.Player.AddExperience(experience);
        }
    }
}
