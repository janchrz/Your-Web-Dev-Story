using System;

namespace Gyvr.Mythril2D
{
    [Serializable]
    public class LevelScaledStats : LevelScaledValue<Stats>
    {
        protected override Stats Evalulate(float t)
        {
            return m_initialValue * t;
        }
    }
}
