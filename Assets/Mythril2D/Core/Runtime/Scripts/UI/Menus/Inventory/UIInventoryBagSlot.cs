using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Gyvr.Mythril2D
{
    public class UIInventoryBagSlot : MonoBehaviour, IItemSlotHandler, IPointerEnterHandler, ISelectHandler, IDeselectHandler
    {
        [SerializeField] private Image m_image = null;
        [SerializeField] private TextMeshProUGUI m_quantity = null;
        [SerializeField] private Button m_button = null;

        public Button button => m_button;

        private Item m_item = null;
        private bool m_selected = false;

        public void Clear() => SetItem(null, 0);

        public Item GetItem()
        {
            return m_item;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            m_button.Select();
        }

        public void OnSelect(BaseEventData eventData)
        {
            m_selected = true;
            GameManager.NotificationSystem.itemDetailsOpened.Invoke(m_item);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            m_selected = false;
            GameManager.NotificationSystem.itemDetailsClosed.Invoke();
        }

        public void SetItem(Item item, int quantity)
        {
            if (item != null)
            {
                m_item = item;
                m_quantity.text = quantity.ToString();
                m_image.enabled = true;
                m_image.sprite = item.icon;
            }
            else
            {
                m_image.enabled = false;
                m_quantity.text = string.Empty;
                m_item = null;
            }

            if (m_selected)
            {
                GameManager.NotificationSystem.itemDetailsOpened.Invoke(m_item);
            }
        }

        private void Awake()
        {
            m_button.onClick.AddListener(OnSlotClicked);
        }

        private void OnSlotClicked()
        {
            if (m_item != null)
            {
                SendMessageUpwards("OnBagItemClicked", m_item, SendMessageOptions.RequireReceiver);
            }
        }
    }
}
