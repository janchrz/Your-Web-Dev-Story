using Unity.Mathematics;

namespace Gyvr.Mythril2D
{
    public static class DamageSolver
    {
        public static int CalculateDamageOut(int flatDamages, int scaledDamages, float scale, int stat)
        {
            return flatDamages + (int)math.round((scaledDamages + stat) * scale);
        }

        public static int CalculateDamageIn(int damage, int stat)
        {
            return (int)math.floor(damage * (100.0f / (100.0f + stat)));
        }

        public static int CalculateCriticalDamage(int damage)
        {
            return damage * 2;
        }

        public static int CalculateMissDamage(int damage)
        {
            return 0;
        }

        public static bool EvaluateCritical(int luck)
        {
            return UnityEngine.Random.Range(1, 100) < luck;
        }

        public static bool EvaluateMiss(int attackerAgility, int defenderAgility)
        {
            float missChance = math.clamp((int)math.floor((90.0f + attackerAgility) * (100.0f / (100.0f + defenderAgility))), 0, 100);
            return UnityEngine.Random.Range(1, 100) > missChance;
        }

        public static int GetOffensiveStat(Stats stats, EDamageType type)
        {
            switch (type)
            {
                default:
                case EDamageType.None: return 0;
                case EDamageType.Physical: return stats[EStat.PhysicalAttack];
                case EDamageType.Magical: return stats[EStat.MagicalAttack];
            }
        }

        public static int GetDefensiveStat(Stats stats, EDamageType type)
        {
            switch (type)
            {
                default:
                case EDamageType.None: return 0;
                case EDamageType.Physical: return stats[EStat.PhysicalDefense];
                case EDamageType.Magical: return stats[EStat.MagicalDefense];
            }
        }

        public static DamageOutputDescriptor SolveDamageOutput(CharacterBase attacker, DamageDescriptor input)
        {
            if (attacker)
            {
                EDamageFlag damageFlags = EDamageFlag.None;

                int damage = CalculateDamageOut(
                    input.flatDamages,
                    input.scaledDamages,
                    input.scale,
                    GetOffensiveStat(attacker.stats, input.type));

                if (GameManager.Config.canCriticalHit && EvaluateCritical(attacker.stats[EStat.Luck]))
                {
                    damage = CalculateCriticalDamage(damage);
                    damageFlags |= EDamageFlag.Critical;
                }

                return new DamageOutputDescriptor
                {
                    source = EDamageSource.Character,
                    attacker = attacker,
                    damage = damage,
                    type = input.type,
                    flags = damageFlags
                };
            }
            else
            {
                return new DamageOutputDescriptor
                {
                    source = EDamageSource.Unknown,
                    attacker = null,
                    damage = input.flatDamages,
                    type = input.type,
                    flags = EDamageFlag.None
                };
            }
        }

        public static DamageInputDescriptor SolveDamageInput(CharacterBase defender, DamageOutputDescriptor output)
        {
            if (output.source == EDamageSource.Character)
            {
                CharacterBase attacker = output.attacker as CharacterBase;

                int damage = CalculateDamageIn(output.damage, GetDefensiveStat(defender.stats, output.type));
                bool missed =
                    GameManager.Config.canMissHit ?
                    EvaluateMiss(attacker.stats[EStat.Agility], defender.stats[EStat.Agility]) :
                    false;

                return new DamageInputDescriptor
                {
                    damage = missed ? CalculateMissDamage(damage) : damage,
                    flags = missed ? output.flags | EDamageFlag.Miss : output.flags
                };
            }
            else
            {
                return new DamageInputDescriptor
                {
                    damage = output.damage,
                    flags = output.flags
                };
            }
        }
    }
}
