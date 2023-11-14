using System;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    [System.Serializable]
    public struct MonsterSpawn
    {
        public GameObject prefab;
        public int rate;
    }

    [RequireComponent(typeof(Collider2D))]
    public class MonsterSpawner : MonoBehaviour
    {
        // Inspector Settings
        [SerializeField] private MonsterSpawn[] m_monsters = null;
        [SerializeField][Range(Stats.MinLevel, Stats.MaxLevel)] private int m_minLevel = Stats.MinLevel;
        [SerializeField][Range(Stats.MinLevel, Stats.MaxLevel)] private int m_maxLevel = Stats.MaxLevel;
        [SerializeField] private int m_maxMonsterCount = 4;
        [SerializeField] private float m_spawnCooldown = 5.0f;
        [SerializeField] private int m_monstersToPrespawn = 4;

        // Component References
        private Collider2D m_collider = null;

        // Private Members
        private float m_spawnTimer = 0.0f;
        private bool m_valid = false;

        private void Awake()
        {
            m_collider = GetComponent<Collider2D>();

            if (Validate())
            {
                Array.Sort(m_monsters, (a, b) => a.rate.CompareTo(b.rate));

                for (int i = 0; i < m_monstersToPrespawn; ++i)
                {
                    Spawn();
                }
            }
            else
            {
                Debug.LogError("MonsterSpawner validation failed. Make sure the total spawn rate is equal to 100");
            }
        }

        private bool Validate()
        {
            int rateSum = 0;

            foreach (MonsterSpawn monster in m_monsters)
            {
                rateSum += monster.rate;
            }

            return m_valid = rateSum == 100;
        }

        private void Update()
        {
            if (m_valid && transform.childCount < m_maxMonsterCount)
            {
                m_spawnTimer += Time.deltaTime;

                if (m_spawnTimer > m_spawnCooldown)
                {
                    m_spawnTimer = 0.0f;
                    Spawn();
                }
            }
        }

        private Vector2 FindPointInCollider()
        {
            while (true)
            {
                Vector2 point = new Vector2
                {
                    x = UnityEngine.Random.Range(m_collider.bounds.min.x, m_collider.bounds.max.x),
                    y = UnityEngine.Random.Range(m_collider.bounds.min.y, m_collider.bounds.max.y)
                };

                if (m_collider.OverlapPoint(point))
                {
                    return point;
                }
            }
        }

        private GameObject FindMonsterToSpawn()
        {
            int randomNumber = UnityEngine.Random.Range(0, 100);

            foreach (MonsterSpawn monster in m_monsters)
            {
                if (randomNumber <= monster.rate)
                {
                    return monster.prefab;
                }
                else
                {
                    randomNumber -= monster.rate;
                }
            }

            return null;
        }

        private void Spawn()
        {
            Vector2 position = FindPointInCollider();
            GameObject monster = FindMonsterToSpawn();

            if (monster)
            {
                GameObject instance = Instantiate(monster, position, Quaternion.identity, transform);
                Monster monsterComponent = instance.GetComponent<Monster>();

                if (monsterComponent)
                {
                    monsterComponent.SetLevel(UnityEngine.Random.Range(m_minLevel, m_maxLevel));
                }
                else
                {
                    Debug.LogError("No Monster component found on the monster prefab");
                }
            }
            else
            {
                Debug.LogError("Couldn't find a monster to spawn, please check your spawn rates and make sure their sum is 100");
            }
        }
    }
}
