using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gyvr.Mythril2D
{
    public class UIShop : MonoBehaviour, IUIMenu
    {
        [Header("Audio")]
        [SerializeField] private AudioClipResolver m_buySellAudio;

        [Header("Dialogues")]
        [SerializeField] private DialogueSequence m_cannotBuy = null;
        [SerializeField] private DialogueSequence m_cannotSell = null;

        [Header("References")]
        [SerializeField] private UIInventoryBag m_inventoryBag = null;
        [SerializeField] private GameObject m_shopEntryPrefab = null;
        [SerializeField] private GameObject m_itemSlotsRoot = null;
        [SerializeField] private TextMeshProUGUI m_money = null;

        private UIShopEntry[] m_slots = null;
        private Shop m_shop = null;

        public void Init()
        {
            m_inventoryBag.Init();
        }

        public void Show(params object[] args)
        {
            Debug.Assert(args.Length == 1 && args[0] is Shop, "Shop panel invoked with incorrect arguments");

            m_shop = (Shop)args[0];
            UpdateUI();
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void EnableInteractions(bool enable)
        {
            CanvasGroup canvasGroup = GetComponent<CanvasGroup>();

            if (canvasGroup)
            {
                canvasGroup.interactable = enable;
            }
        }

        public GameObject FindSomethingToSelect()
        {
            if (m_slots.Length > 0)
            {
                return m_slots[0].GetComponentInChildren<Button>().gameObject;
            }

            UINavigationCursorTarget bagNavigationTarget = m_inventoryBag.FindNavigationTarget();

            if (bagNavigationTarget && bagNavigationTarget.gameObject.activeInHierarchy)
            {
                return bagNavigationTarget.gameObject;
            }

            return null;
        }

        // Message called by children using SendMessageUpward when the bag or equipment changed
        private void UpdateUI(bool skipItemSlots = false)
        {
            m_inventoryBag.UpdateSlots();

            if (!skipItemSlots)
            {
                ClearSlots();
                FillSlots();
                RewireNavigation();
            }
        }

        private void Update()
        {
            UpdatePlayerMoneyDisplay();
        }

        private void UpdatePlayerMoneyDisplay()
        {
            int currentMoney = GameManager.InventorySystem.money;
            int selectedItemPrice = 0;
            ETransactionType transactionType = ETransactionType.Buy;

            GameObject selection = GameManager.EventSystem.currentSelectedGameObject;
            UIShopEntry shopItem = selection.GetComponent<UIShopEntry>();

            if (shopItem && shopItem.GetItem())
            {
                transactionType = ETransactionType.Buy;
                selectedItemPrice = m_shop.GetPrice(shopItem.GetItem(), transactionType);
            }
            else
            {
                UIInventoryBagSlot bagItem = selection.GetComponent<UIInventoryBagSlot>();

                if (bagItem && bagItem.GetItem())
                {
                    transactionType = ETransactionType.Sell;
                    selectedItemPrice = m_shop.GetPrice(bagItem.GetItem(), transactionType);
                }
            }

            if (selectedItemPrice == 0)
            {
                m_money.text = GameManager.InventorySystem.money.ToString();
            }
            else
            {
                m_money.text = string.Format("{0}\n({1}{2})",
                    GameManager.InventorySystem.money,
                    transactionType == ETransactionType.Buy ? "-" : "+",
                    selectedItemPrice);
            }
        }

        private void FillSlots()
        {
            int itemCount = m_shop.items.Length;
            m_slots = new UIShopEntry[itemCount];

            for (int i = 0; i < itemCount; ++i)
            {
                Item item = m_shop.items[i];

                GameObject itemSlot = Instantiate(m_shopEntryPrefab, m_itemSlotsRoot.transform);
                UIShopEntry inventoryBagSlot = itemSlot.GetComponent<UIShopEntry>();

                if (inventoryBagSlot)
                {
                    inventoryBagSlot.Initialize(item);
                    m_slots[i] = inventoryBagSlot;
                }
            }
        }

        private void ClearSlots()
        {
            for (int i = 0; i < m_itemSlotsRoot.transform.childCount; ++i)
            {
                Transform child = m_itemSlotsRoot.transform.GetChild(i);
                Destroy(child.gameObject);
            }
        }

        private void OnShopSlotClicked(Item item)
        {
            int itemPrice = m_shop.GetPrice(item, ETransactionType.Buy);

            if (GameManager.InventorySystem.money >= itemPrice)
            {
                GameManager.InventorySystem.RemoveMoney(itemPrice);
                GameManager.InventorySystem.AddToBag(item);
                GameManager.NotificationSystem.audioPlaybackRequested.Invoke(m_buySellAudio);
                UpdateUI(true);
            }
            else
            {
                GameManager.DialogueSystem.Main.PlayNow(DialogueUtils.CreateDialogueTree(m_cannotBuy, null, item.displayName));
            }
        }

        private void OnBagItemClicked(Item item)
        {
            int itemPrice = m_shop.GetPrice(item, ETransactionType.Sell);

            if (itemPrice > 0)
            {
                GameManager.InventorySystem.RemoveFromBag(item);
                GameManager.InventorySystem.AddMoney(itemPrice);
                GameManager.NotificationSystem.audioPlaybackRequested.Invoke(m_buySellAudio);
                UpdateUI();
            }
            else
            {
                GameManager.DialogueSystem.Main.PlayNow(DialogueUtils.CreateDialogueTree(m_cannotSell, null, item.displayName));
            }
        }

        private void RewireNavigation()
        {
            UIInventoryBagSlot firstBagSlot = m_inventoryBag.GetFirstSlot();

            for (int i = 0; i < m_slots.Length; ++i)
            {
                UIShopEntry current = m_slots[i];
                UIShopEntry previous = i > 0 ? m_slots[i - 1] : null;
                UIShopEntry next = i < m_slots.Length - 1 ? m_slots[i + 1] : null;

                current.button.navigation = new Navigation
                {
                    mode = Navigation.Mode.Explicit,
                    selectOnUp = previous?.button,
                    selectOnDown = next?.button,
                    selectOnRight = firstBagSlot?.button
                };
            }
        }
    }
}
