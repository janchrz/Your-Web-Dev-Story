using System;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    [CreateAssetMenu(menuName = AssetMenuIndexer.Mythril2D_ScriptableActions + nameof(SetGameFlagAction))]
    public class SetGameFlagAction : ScriptableAction
    {
        public override Type[] RequiredArgs => new Type[] { typeof(string) };
        public override Type[] OptionalArgs => new Type[] { typeof(bool) };
        public override string[] ArgDescriptions => new string[] { "FlagID to set", "True or false to set or unset" };

        public override void Execute(params object[] args)
        {
            string flagID = GetRequiredArgAtIndex<string>(0, args);
            bool value = GetOptionalArgAtIndex<bool>(1, true, args);

            GameManager.GameFlagSystem.Set(flagID, value);
        }
    }
}
