using System;
using System.Collections.Generic;

namespace Gyvr.Mythril2D
{
    [Serializable]
    public struct GameFlagsDataBlock
    {
        public string[] flags;
    }

    public class GameFlagSystem : AGameSystem, IDataBlockHandler<GameFlagsDataBlock>
    {
        private HashSet<string> m_flags = new HashSet<string>();

        public bool Get(string variableName)
        {
            return m_flags.Contains(variableName);
        }

        public void Set(string variableName, bool value)
        {
            if (value)
            {
                m_flags.Add(variableName);
            }
            else
            {
                m_flags.Remove(variableName);
            }

            GameManager.NotificationSystem.gameFlagChanged.Invoke(variableName, value);
        }

        public void LoadDataBlock(GameFlagsDataBlock block)
        {
            m_flags = new HashSet<string>(block.flags);
        }

        public GameFlagsDataBlock CreateDataBlock()
        {
            string[] flagsArray = new string[m_flags.Count];
            m_flags.CopyTo(flagsArray);

            return new GameFlagsDataBlock
            {
                flags = flagsArray
            };
        }
    }
}
