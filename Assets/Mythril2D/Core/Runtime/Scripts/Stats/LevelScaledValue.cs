using System;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    public abstract class LevelScaledValue<T>
    {
        [SerializeField] protected T m_initialValue;
        [SerializeField] private AnimationCurve m_evolutionProfile = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
        [SerializeField] private float m_evolutionScale = 1.0f;

        public T this[int level]
        {
            get
            {
                float t = (level - 1) / (float)Stats.LevelCount;

                if (m_evolutionProfile != null)
                {
                    return Evalulate(1.0f + (m_evolutionProfile.Evaluate(t) * m_evolutionScale));
                }
                else
                {
                    Debug.LogWarning("No animation curve set, falling back to the initial value");
                    return m_initialValue;
                }
            }
        }

        protected abstract T Evalulate(float t);
    }
}
