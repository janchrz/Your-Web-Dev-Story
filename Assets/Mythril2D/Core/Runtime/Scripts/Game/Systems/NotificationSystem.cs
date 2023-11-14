using UnityEngine;
using UnityEngine.Events;

namespace Gyvr.Mythril2D
{
    public class NotificationSystem : AGameSystem
    {
        [Header("Gameplay Events")]
        public UnityEvent<MonsterSheet> monsterKilled = new UnityEvent<MonsterSheet>();
        public UnityEvent<CharacterBase, DamageInputDescriptor> damageApplied = new UnityEvent<CharacterBase, DamageInputDescriptor>();
        public UnityEvent<CharacterBase, int> healthRecovered = new UnityEvent<CharacterBase, int>();
        public UnityEvent<CharacterBase, int> manaConsumed = new UnityEvent<CharacterBase, int>();
        public UnityEvent<CharacterBase, int> manaRecovered = new UnityEvent<CharacterBase, int>();
        public UnityEvent<int> experienceGained = new UnityEvent<int>();
        public UnityEvent<int> levelUp = new UnityEvent<int>();
        public UnityEvent<AIController, Transform> targetDetected = new UnityEvent<AIController, Transform>();
        public UnityEvent<int> moneyAdded = new UnityEvent<int>();
        public UnityEvent<int> moneyRemoved = new UnityEvent<int>();
        public UnityEvent<Item, int> itemAdded = new UnityEvent<Item, int>();
        public UnityEvent<Item, int> itemRemoved = new UnityEvent<Item, int>();
        public UnityEvent<Quest> questProgressionUpdated = new UnityEvent<Quest>();
        public UnityEvent<Quest> questStarted = new UnityEvent<Quest>();
        public UnityEvent<Quest> questAvailable = new UnityEvent<Quest>();
        public UnityEvent<Quest> questFullfilled = new UnityEvent<Quest>();
        public UnityEvent<Quest> questCompleted = new UnityEvent<Quest>();
        public UnityEvent<string, bool> gameFlagChanged = new UnityEvent<string, bool>();
        public UnityEvent mapTransitionStarted = new UnityEvent();
        public UnityEvent mapTransitionCompleted = new UnityEvent();
        public UnityEvent mapLoaded = new UnityEvent();
        public UnityEvent mapUnloaded = new UnityEvent();

        public UnityEvent<MapLoadingDelegationParams> mapTransitionDelegationRequested = new UnityEvent<MapLoadingDelegationParams>();

        [Header("User Interface")]
        public UnityEvent<Shop> shopRequested = new UnityEvent<Shop>();
        public UnityEvent gameMenuRequested = new UnityEvent();
        public UnityEvent statsRequested = new UnityEvent();
        public UnityEvent inventoryRequested = new UnityEvent();
        public UnityEvent journalRequested = new UnityEvent();
        public UnityEvent spellBookRequested = new UnityEvent();
        public UnityEvent saveMenuRequested = new UnityEvent();
        public UnityEvent<IUIMenu> menuShowed = new UnityEvent<IUIMenu>();
        public UnityEvent<IUIMenu> menuHid = new UnityEvent<IUIMenu>();
        public UnityEvent<Item> itemDetailsOpened = new UnityEvent<Item>();
        public UnityEvent itemDetailsClosed = new UnityEvent();

        [Header("Audio")]
        public UnityEvent<AudioClipResolver> audioPlaybackRequested = new UnityEvent<AudioClipResolver>();
    }
}
