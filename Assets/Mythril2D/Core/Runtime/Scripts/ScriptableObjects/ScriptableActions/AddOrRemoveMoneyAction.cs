using System;
using Unity.Mathematics;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    [CreateAssetMenu(menuName = AssetMenuIndexer.Mythril2D_ScriptableActions + nameof(AddOrRemoveMoneyAction))]
    public class AddOrRemoveMoneyAction : ScriptableAction
    {
        public override Type[] RequiredArgs => new Type[] { typeof(int) };
        public override Type[] OptionalArgs => new Type[] { };
        public override string[] ArgDescriptions => new string[] { "Amount of money to add or remove (+/-)" };

        public override void Execute(params object[] args)
        {
            int amount = GetRequiredArgAtIndex<int>(0, args);

            if (amount > 0)
            {
                GameManager.InventorySystem.AddMoney(amount);
            }
            else
            {
                GameManager.InventorySystem.RemoveMoney(amount);
            }
        }
    }
}
