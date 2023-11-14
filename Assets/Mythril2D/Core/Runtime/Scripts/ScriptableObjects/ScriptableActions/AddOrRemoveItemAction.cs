using System;
using Unity.Mathematics;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    [CreateAssetMenu(menuName = AssetMenuIndexer.Mythril2D_ScriptableActions + nameof(AddOrRemoveItemAction))]
    public class AddOrRemoveItemAction : ScriptableAction
    {
        public override Type[] RequiredArgs => new Type[] { typeof(Item) };
        public override Type[] OptionalArgs => new Type[] { typeof(int) };
        public override string[] ArgDescriptions => new string[] { "Item to add or remove", "Quantity (+/-)" };

        public override void Execute(params object[] args)
        {
            Item item = GetRequiredArgAtIndex<Item>(0, args);
            int count = GetOptionalArgAtIndex<int>(1, 1, args);

            if (count > 0)
            {
                GameManager.InventorySystem.AddToBag(item, count);
            }
            else
            {
                GameManager.InventorySystem.RemoveFromBag(item, math.abs(count));
            }
        }
    }
}
