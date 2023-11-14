using System;
using System.Linq;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    [Serializable]
    public struct PlayerDataBlock
    {
        public GameObject prefab;
        public int usedPoints;
        public int experience;
        public Stats missingCurrentStats;
        public Stats customStats;
        public Equipment[] equipments;
        public AbilitySheet[] equipedAbilities;
        public Vector3 position;
    }

    public class PlayerSystem : AGameSystem, IDataBlockHandler<PlayerDataBlock>
    {
        [Header("Settings")]
        [SerializeField] private GameObject m_dummyPlayerPrefab = null;

        public Hero PlayerInstance => m_playerInstance;
        public GameObject PlayerPrefab => m_playerPrefab;

        private GameObject m_playerPrefab = null;
        private Hero m_playerInstance = null;

        public override void OnSystemStart()
        {
            m_playerInstance = InstantiatePlayer(m_dummyPlayerPrefab);
        }

        private Hero InstantiatePlayer(GameObject prefab)
        {
            GameObject playerInstance = Instantiate(prefab, transform);
            Hero hero = playerInstance.GetComponent<Hero>();
            Debug.Assert(hero != null, "The player instance specified doesn't have a Hero component");
            m_playerPrefab = prefab;
            return hero;
        }

        public void LoadDataBlock(PlayerDataBlock block)
        {
            if (m_playerInstance)
            {
                Destroy(m_playerInstance.gameObject);
            }

            m_playerInstance = InstantiatePlayer(block.prefab);
            m_playerInstance.Initialize(block);
        }

        public PlayerDataBlock CreateDataBlock()
        {
            return new PlayerDataBlock
            {
                prefab = m_playerPrefab,
                usedPoints = m_playerInstance.usedPoints,
                experience = m_playerInstance.experience,
                equipments = m_playerInstance.equipments.Values.ToArray(),
                missingCurrentStats = m_playerInstance.stats - m_playerInstance.currentStats,
                customStats = m_playerInstance.customStats,
                equipedAbilities = m_playerInstance.equippedAbilities,
                position = m_playerInstance.transform.position
            };
        }
    }
}
