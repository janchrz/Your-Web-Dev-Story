using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Gyvr.Mythril2D
{
    public class UIShopEntry : MonoBehaviour, IItemSlotHandler, IPointerEnterHandler, ISelectHandler, IDeselectHandler
    {
        [Header("References")]
        [SerializeField] private Image m_image = null;
        [SerializeField] private TextMeshProUGUI m_name = null;
        [SerializeField] private TextMeshProUGUI m_price = null;
        [SerializeField] private Button m_button = null;

        public Button button => m_button;

        private Item m_target = null;

        private void Awake()
        {
            m_button.onClick.AddListener(OnSlotClicked);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            m_button.Select();
        }

        public void OnSelect(BaseEventData eventData)
        {
            GameManager.NotificationSystem.itemDetailsOpened.Invoke(m_target);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            GameManager.NotificationSystem.itemDetailsClosed.Invoke();
        }

        public void OnSlotClicked()
        {
            if (m_target != null)
            {
                SendMessageUpwards("OnShopSlotClicked", m_target, SendMessageOptions.RequireReceiver);
            }
        }

        public void Initialize(Item item)
        {
            m_target = item;
            m_name.text = item.displayName;
            m_price.text = item.price.ToString();
            m_image.sprite = item.icon;
        }

        public Item GetItem()
        {
            return m_target;
        }
    }
}
