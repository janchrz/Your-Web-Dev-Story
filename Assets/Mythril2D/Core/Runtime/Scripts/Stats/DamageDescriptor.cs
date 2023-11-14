using System;

namespace Gyvr.Mythril2D
{
    public enum EDamageSource
    {
        Character,
        Unknown
    }

    [Flags]
    public enum EDamageFlag
    {
        Default = None,
        None = 0,
        Critical = 1 << 0,
        Miss = 1 << 1,
    }

    public enum EDamageType
    {
        None,
        Physical,
        Magical
    }

    /// <summary>
    /// Damage settings
    /// </summary>
    [System.Serializable]
    public struct DamageDescriptor
    {
        public int flatDamages;
        public int scaledDamages;
        public float scale;
        public EDamageType type;
    }

    /// <summary>
    /// Damages output by the attacker after calculations (attack/critical)
    /// </summary>
    public struct DamageOutputDescriptor
    {
        public EDamageSource source;
        public object attacker;
        public int damage;
        public EDamageType type;
        public EDamageFlag flags;
    }

    /// <summary>
    /// Damages received by the target after mitigation (defense/miss)
    /// </summary>
    public struct DamageInputDescriptor
    {
        public int damage;
        public EDamageFlag flags;
    }
}