using UnityEngine.Events;

namespace Gyvr.Mythril2D
{
    public class ObservableStats
    {
        public Stats stats => m_stats;
        public UnityEvent<Stats> changed => m_changed;

        private Stats m_stats;
        private UnityEvent<Stats> m_changed = new UnityEvent<Stats>();

        public ObservableStats() : this(new Stats())
        {
        }

        public ObservableStats(Stats stats)
        {
            m_stats = stats;
        }

        public int this[EStat stat]
        {
            get => m_stats[stat];
            set
            {
                Stats previous = new Stats(m_stats);
                m_stats[stat] = value;
                m_changed.Invoke(previous);
            }
        }

        public void Set(Stats stats)
        {
            Stats previous = new Stats(m_stats);
            m_stats = new Stats(stats);
            m_changed.Invoke(previous);
        }
    }
}
