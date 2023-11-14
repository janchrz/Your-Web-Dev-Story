using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Gyvr.Mythril2D
{
    public class GameManager : MonoBehaviour
    {
        // Inspector Settings
        [Header("Global Settings")]
        [SerializeField] private GameConfig m_config = null;

        // Public Static Members
        public static Hero Player => PlayerSystem.PlayerInstance;
        public static EventSystem EventSystem => EventSystem.current;
        public static GameConfig Config => _instance.m_config;
        public static GameManager Instance => _instance;

        // System Access Shortcuts
        public static AudioSystem AudioSystem => GetSystem<AudioSystem>();
        public static DialogueSystem DialogueSystem => GetSystem<DialogueSystem>();
        public static GameFlagSystem GameFlagSystem => GetSystem<GameFlagSystem>();
        public static GameStateSystem GameStateSystem => GetSystem<GameStateSystem>();
        public static InputSystem InputSystem => GetSystem<InputSystem>();
        public static InventorySystem InventorySystem => GetSystem<InventorySystem>();
        public static JournalSystem JournalSystem => GetSystem<JournalSystem>();
        public static NotificationSystem NotificationSystem => GetSystem<NotificationSystem>();
        public static SaveSystem SaveSystem => GetSystem<SaveSystem>();
        public static MapLoadingSystem MapLoadingSystem => GetSystem<MapLoadingSystem>();
        public static PlayerSystem PlayerSystem => GetSystem<PlayerSystem>();
        public static PhysicsSystem PhysicsSystem => GetSystem<PhysicsSystem>();

        // Private Static Members
        private static GameManager _instance = null;
        private Dictionary<Type, AGameSystem> m_systems = null;

        private void Awake()
        {
            _instance = this;

            FindSystems();
            InitializeSystems();
        }

        private void OnEnable()
        {
            if (m_systems.ContainsKey(typeof(NotificationSystem)))
            {
                NotificationSystem.mapLoaded.AddListener(OnMapLoaded);
                NotificationSystem.mapUnloaded.AddListener(OnMapUnloaded);
            }

            StartSystems();
        }

        private void OnDisable()
        {
            StopSystems();

            if (m_systems.ContainsKey(typeof(NotificationSystem)))
            {
                NotificationSystem.mapLoaded.RemoveListener(OnMapLoaded);
                NotificationSystem.mapUnloaded.RemoveListener(OnMapUnloaded);
            }
        }

        private void InitializeSystems()
        {
            foreach (AGameSystem system in m_systems.Values)
            {
                system.OnSystemInit();
            }
        }

        private void StartSystems()
        {
            foreach (AGameSystem system in m_systems.Values)
            {
                system.OnSystemStart();
            }
        }

        private void StopSystems()
        {
            foreach (AGameSystem system in m_systems.Values)
            {
                system.OnSystemStop();
            }
        }

        private void OnMapLoaded()
        {
            foreach (AGameSystem system in m_systems.Values)
            {
                system.OnMapLoaded();
            }
        }

        private void OnMapUnloaded()
        {
            foreach (AGameSystem system in m_systems.Values)
            {
                system.OnMapUnloaded();
            }
        }

        private void FindSystems()
        {
            AGameSystem[] systems = FindObjectsOfType<AGameSystem>();

            m_systems = new Dictionary<Type, AGameSystem>();

            foreach (AGameSystem system in systems)
            {
                Type type = system.GetType();
                Debug.Assert(!m_systems.ContainsKey(type), $"Game System {type.Name} already registered");
                m_systems[type] = system;
            }
        }

        public static T GetSystem<T>() where T : AGameSystem
        {
            AGameSystem system = null;
            bool systemFound = _instance.m_systems.TryGetValue(typeof(T), out system);
            Debug.Assert(systemFound, $"Game System {typeof(T).Name} could not be found");
            return (T)system;
        }

        public static bool Exists() => _instance;
    }
}
