using System;
using Unity.Mathematics;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    [CreateAssetMenu(menuName = AssetMenuIndexer.Mythril2D_ScriptableActions + nameof(HealOrDamagePlayerAction))]
    public class HealOrDamagePlayerAction : ScriptableAction
    {
        public override Type[] RequiredArgs => new Type[] { typeof(int) };
        public override Type[] OptionalArgs => new Type[] { };
        public override string[] ArgDescriptions => new string[] { "Amount of health to remove or add (+/-)" };

        public override void Execute(params object[] args)
        {
            int amount = GetRequiredArgAtIndex<int>(0, args);

            if (amount > 0)
            {
                GameManager.Player.Heal(amount);
            }
            else
            {
                GameManager.Player.Damage(new DamageOutputDescriptor
                {
                    attacker = null,
                    damage = math.abs(amount),
                    flags = EDamageFlag.Default,
                    type = EDamageType.None,
                    source = EDamageSource.Unknown
                });
            }
        }
    }
}
