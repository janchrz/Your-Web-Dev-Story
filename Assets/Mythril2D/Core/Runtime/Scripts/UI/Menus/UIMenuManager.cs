using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Gyvr.Mythril2D
{
    public class UIMenuManager : MonoBehaviour
    {
        [Header("UI Menus")]
        [SerializeField] private UIGameMenu m_gameMenu = null;
        [SerializeField] private UIShop m_shop = null;
        [SerializeField] private UICharacter m_stats = null;
        [SerializeField] private UIInventory m_inventory = null;
        [SerializeField] private UIJournal m_journal = null;
        [SerializeField] private UIAbilities m_abilities = null;
        [SerializeField] private UISave m_save = null;

        private Stack<IUIMenu> m_menuStack = new Stack<IUIMenu>();

        private void Start()
        {
            GameManager.NotificationSystem.gameMenuRequested.AddListener(OnGameMenuRequested);
            GameManager.NotificationSystem.shopRequested.AddListener(OnShopRequested);
            GameManager.NotificationSystem.statsRequested.AddListener(OnStatsRequested);
            GameManager.NotificationSystem.journalRequested.AddListener(OnJournalRequested);
            GameManager.NotificationSystem.inventoryRequested.AddListener(OnInventoryRequested);
            GameManager.NotificationSystem.spellBookRequested.AddListener(OnAbilitiesRequested);
            GameManager.NotificationSystem.saveMenuRequested.AddListener(OnSaveMenuRequested);

            m_gameMenu.Init();
            m_shop.Init();
            m_stats.Init();
            m_journal.Init();
            m_inventory.Init();
            m_abilities.Init();
            m_save.Init();

            GameManager.InputSystem.ui.cancel.performed += OnCancel;
        }

        private void OnDestroy()
        {
            GameManager.InputSystem.ui.cancel.performed -= OnCancel;
        }

        private void UpdateMenuNavigation(IUIMenu menu)
        {
            GameObject toSelect = menu.FindSomethingToSelect();

            if (!toSelect)
            {
                MonoBehaviour menuMonoBehaviour = (MonoBehaviour)m_menuStack.Peek();
                Selectable firstSelectable = menuMonoBehaviour.gameObject.GetComponentInChildren<Selectable>();

                if (firstSelectable)
                {
                    toSelect = firstSelectable.gameObject;
                }
            }

            GameManager.EventSystem.SetSelectedGameObject(toSelect);
        }

        private void OnCancel(InputAction.CallbackContext context)
        {
            // Prevent exiting a menu if a dialogue is playing
            if (!GameManager.DialogueSystem.Main.IsPlaying())
            {
                PopMenu();
            }
        }

        private void PushMenu(IUIMenu menu, params object[] args)
        {
            if (m_menuStack.Count > 0 && m_menuStack.Peek() != null)
            {
                IUIMenu previousMenu = m_menuStack.Peek();
                previousMenu.EnableInteractions(false);
            }

            m_menuStack.Push(menu);
            menu.OnMenuPushed();
            GameManager.GameStateSystem.AddLayer(EGameState.Menu);
            Show(menu, args);
        }

        private void PopMenu()
        {
            if (m_menuStack.Count > 0)
            {
                IUIMenu menu = m_menuStack.Pop();
                menu.OnMenuPopped();
                Hide(menu);

                if (m_menuStack.Count > 0)
                {
                    Show(m_menuStack.Peek());
                }
            }
        }

        private void Show(IUIMenu menu, params object[] args)
        {
            menu.Show(args);
            menu.EnableInteractions(true);

            GameManager.NotificationSystem.menuShowed.Invoke(menu);

            // Because some menus may use layouts, they may need at least one frame to be setup properly.
            // If we update menu navigation too early, the selection cursor feedback may move during the setup
            StartCoroutine(CoroutineHelpers.ExecuteInXFrames(1, () => UpdateMenuNavigation(menu)));
        }

        private void Hide(IUIMenu menu)
        {
            menu.Hide();
            GameManager.NotificationSystem.menuHid.Invoke(menu);
            GameManager.NotificationSystem.itemDetailsClosed.Invoke();
            GameManager.GameStateSystem.RemoveLayer(EGameState.Menu);
        }

        private void OnGameMenuRequested() => PushMenu(m_gameMenu);

        private void OnShopRequested(Shop shop) => PushMenu(m_shop, shop);

        private void OnStatsRequested() => PushMenu(m_stats);

        private void OnJournalRequested() => PushMenu(m_journal);

        private void OnInventoryRequested() => PushMenu(m_inventory);

        private void OnAbilitiesRequested() => PushMenu(m_abilities);

        private void OnSaveMenuRequested() => PushMenu(m_save);
    }
}
