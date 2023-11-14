using System;
using Unity.Mathematics;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    public enum EStat
    {
        Health = 0,
        Mana = 1,
        PhysicalAttack = 2,
        MagicalAttack = 3,
        PhysicalDefense = 4,
        MagicalDefense = 5,
        Agility = 6,
        Luck = 7
    }

    [Serializable]
    public class Stats
    {
        public const int MinLevel = 1;
        public const int MaxLevel = 20;
        public const int LevelCount = MaxLevel - MinLevel + 1;
        public const int StatCount = 8;

        [SerializeField] private int[] m_values = new int[StatCount] { 0, 0, 0, 0, 0, 0, 0, 0 };

        public Stats() : this(new int[StatCount] { 0, 0, 0, 0, 0, 0, 0, 0 })
        {
        }

        public Stats(Stats copy)
        {
            Array.Copy(copy.m_values, m_values, StatCount);
        }

        public Stats(int[] values)
        {
            m_values = values;
        }

        public void Reset()
        {
            for (int i = 0; i < m_values.Length; ++i)
            {
                m_values[i] = 0;
            }
        }

        public int GetTotal()
        {
            int total = 0;

            for (int i = 0; i < StatCount; ++i)
            {
                total += m_values[i];
            }

            return total;
        }

        private int this[int i]
        {
            get => m_values[i];
            set => m_values[i] = math.max(value, 0);
        }

        public int this[EStat stat]
        {
            get => this[(int)stat];
            set => this[(int)stat] = value;
        }

        public static Stats operator +(Stats a, Stats b)
        {
            Stats output = new Stats(new int[StatCount]);

            for (int i = 0; i < StatCount; ++i)
            {
                output[i] = a[i] + b[i];
            }

            return output;
        }

        public static Stats operator -(Stats a, Stats b)
        {
            Stats output = new Stats(new int[StatCount]);

            for (int i = 0; i < StatCount; ++i)
            {
                output[i] = a[i] - b[i];
            }

            return output;
        }

        public static Stats operator *(Stats a, float scale)
        {
            Stats output = new Stats(new int[StatCount]);

            for (int i = 0; i < StatCount; ++i)
            {
                output[i] = (int)math.floor(a[i] * scale);
            }

            return output;
        }

        public static Stats Lerp(Stats a, Stats b, float t)
        {
            Stats output = new Stats(new int[StatCount]);

            for (int i = 0; i < StatCount; ++i)
            {
                output[i] = a[i] + b[i];
                output[i] = (int)math.floor(math.lerp(a[i], b[i], t));
            }

            return output;
        }
    }

    [Serializable]
    public struct StatsEvolutionProfile
    {
        public Stats min;
        public Stats max;

        public static int Mix(int min, int max, float a) => (int)math.floor(math.lerp(min, max, a));

        public Stats GetStatsAtLevel(int level)
        {
            float t = (level - 1) / (float)(Stats.MaxLevel - 1);
            return Stats.Lerp(min, max, t);
        }
    }
}
