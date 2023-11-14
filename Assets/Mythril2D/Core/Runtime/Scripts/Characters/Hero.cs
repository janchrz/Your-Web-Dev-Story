using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Gyvr.Mythril2D
{
    public class Hero : Character<HeroSheet>
    {
        [Header("Audio")]
        [SerializeField] private AudioClipResolver m_levelUpSound;

        [Header("Hero")]
        [SerializeField] private bool m_restoreHealthOnLevelUp = true;
        [SerializeField] private bool m_restoreManaOnLevelUp = true;

        public int experience => m_experience;
        public int nextLevelExperience => GetTotalExpRequirement(m_level + 1);
        public int availablePoints => m_sheet.pointsPerLevel * (m_level - Stats.MinLevel) - m_usedPoints;
        public SerializableDictionary<EEquipmentType, Equipment> equipments => m_equipments;
        public AbilitySheet[] equippedAbilities => m_equippedAbilities;
        public UnityEvent<AbilitySheet[]> equippedAbilitiesChanged => m_equippedAbilitiesChanged;
        public Stats customStats => m_customStats;
        public Stats missingCurrentStats => m_missingCurrentStats;
        public int usedPoints => m_usedPoints;

        public const int MaxEquipedAbilityCount = 4;

        private Stats m_customStats = new Stats();
        private Stats m_missingCurrentStats = new Stats();
        private int m_usedPoints = 0;
        private int m_experience = 0;
        private SerializableDictionary<EEquipmentType, Equipment> m_equipments = new SerializableDictionary<EEquipmentType, Equipment>();
        private AbilitySheet[] m_equippedAbilities = new AbilitySheet[MaxEquipedAbilityCount];
        private UnityEvent<AbilitySheet[]> m_equippedAbilitiesChanged = new UnityEvent<AbilitySheet[]>();

        public int GetTotalExpRequirement(int level)
        {
            int total = 0;

            for (int i = 1; i < level; i++)
            {
                total += m_sheet.experience[i];
            }

            return total;
        }

        public void AddExperience(int experience, bool silentMode = false)
        {
            Debug.Assert(experience > 0, "Cannot add a negative amount of experience.");

            GameManager.NotificationSystem.experienceGained.Invoke(experience);

            m_experience += experience;

            while (m_experience >= GetTotalExpRequirement(m_level + 1))
            {
                OnLevelUp(silentMode);
            }
        }

        public void AddCustomStats(Stats customStats)
        {
            m_customStats += customStats;
            UpdateStats();
        }

        public void LogUsedPoints(int points)
        {
            m_usedPoints += points;
        }

        private void Start()
        {
            UpdateStats();
        }

        private void ApplyMissingCurrentStats()
        {
            m_currentStats.Set(m_currentStats.stats - m_missingCurrentStats);
            m_missingCurrentStats.Reset();
        }

        public Equipment Equip(Equipment equipment)
        {
            Equipment previousEquipment = Unequip(equipment.type);

            m_equipments[equipment.type] = equipment;

            UpdateStats();

            return previousEquipment;
        }

        public Equipment Unequip(EEquipmentType type)
        {
            m_equipments.TryGetValue(type, out Equipment toUnequip);

            if (toUnequip)
            {
                m_equipments.Remove(type);

                UpdateStats();
            }

            return toUnequip;
        }

        public void Equip(AbilitySheet ability, int index)
        {
            m_equippedAbilities[index] = ability;
            m_equippedAbilitiesChanged.Invoke(m_equippedAbilities);
        }

        public void Unequip(int index)
        {
            m_equippedAbilities[index] = null;
            m_equippedAbilitiesChanged.Invoke(m_equippedAbilities);
        }

        private Stats CalculateEquipmentStats()
        {
            Stats equipmentStats = new Stats();

            foreach (Equipment piece in m_equipments.Values)
            {
                if (piece)
                {
                    equipmentStats += piece.bonusStats;
                }
            }

            return equipmentStats;
        }

        private void UpdateStats()
        {
            Stats equipmentStats = CalculateEquipmentStats();
            Stats totalStats = m_sheet.baseStats + m_customStats + equipmentStats;

            m_stats.Set(totalStats);

            ApplyMissingCurrentStats();
        }

        private void OnLevelUp(bool silentMode = false)
        {
            ++m_level;

            if (!silentMode)
            {
                if (m_restoreHealthOnLevelUp)
                {
                    Heal(m_stats[EStat.Health] - m_currentStats[EStat.Health]);
                }

                if (m_restoreManaOnLevelUp)
                {
                    RecoverMana(m_stats[EStat.Mana] - m_currentStats[EStat.Mana]);
                }

                GameManager.NotificationSystem.levelUp.Invoke(m_level);
                GameManager.NotificationSystem.audioPlaybackRequested.Invoke(m_levelUpSound);
            }

            foreach (AbilitySheet ability in m_sheet.GetAbilitiesUnlockedAtLevel(m_level))
            {
                AddAbility(ability);
            }
        }

        public void Initialize(PlayerDataBlock block)
        {
            m_usedPoints = block.usedPoints;

            if (block.experience > 0)
            {
                AddExperience(block.experience, true);
            }

            m_equipments = new SerializableDictionary<EEquipmentType, Equipment>();

            foreach (Equipment piece in block.equipments)
            {
                m_equipments[piece.type] = piece;
            }

            m_customStats = block.customStats;

            // Copy missing current stats so block data doesn't get altered
            m_missingCurrentStats = new Stats(block.missingCurrentStats);

            for (int i = 0; i < block.equipedAbilities.Length; ++i)
            {
                Equip(block.equipedAbilities[i], i);
            }

            transform.position = block.position;
        }

        protected override void OnDeath()
        {
            base.OnDeath();
            SceneManager.LoadScene(GameManager.Config.mainMenuSceneName);
        }
    }
}
