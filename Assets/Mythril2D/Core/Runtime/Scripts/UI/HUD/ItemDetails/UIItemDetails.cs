using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gyvr.Mythril2D
{
    public class UIItemDetails : MonoBehaviour
    {
        // Inspector Settings
        [Header("References")]
        [SerializeField] private GameObject m_itemDetailsBox = null;
        [SerializeField] private Image m_itemIcon = null;
        [SerializeField] private TextMeshProUGUI m_itemName = null;
        [SerializeField] private TextMeshProUGUI m_itemDescription = null;

        private void Start()
        {
            m_itemDetailsBox.SetActive(false);

            GameManager.NotificationSystem.itemDetailsOpened.AddListener(OnDetailsOpened);
            GameManager.NotificationSystem.itemDetailsClosed.AddListener(OnDetailsClosed);
        }

        private void OnDetailsOpened(Item item)
        {
            if (item)
            {
                m_itemDetailsBox.SetActive(true);
                m_itemIcon.sprite = item.icon;
                m_itemName.text = item.displayName;
                m_itemDescription.text = item.description;

                if (item is Equipment)
                {
                    Equipment equipment = (Equipment)item;

                    for (int i = 0; i < Stats.StatCount; i++)
                    {
                        EStat stat = (EStat)i;
                        int value = equipment.bonusStats[stat];

                        if (value != 0)
                        {
                            m_itemDescription.text += $" <u>{(value > 0 ? '+' : '-')}{value}\u00A0{GameManager.Config.GetTermDefinition(stat).shortName}</u>";
                        }
                    }
                }
            }
            else
            {
                OnDetailsClosed();
            }
        }

        private void OnDetailsClosed()
        {
            m_itemDetailsBox.SetActive(false);
        }
    }
}
