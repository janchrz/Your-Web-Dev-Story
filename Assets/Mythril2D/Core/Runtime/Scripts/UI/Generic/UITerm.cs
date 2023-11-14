using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gyvr.Mythril2D
{
    public enum ETermDisplayMode
    {
        FullName,
        ShortName
    }

    public class UITerm : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private string m_termID = null;
        [SerializeField] private string m_textFormat = "{0}";
        [SerializeField] private ETermDisplayMode m_displayMode = ETermDisplayMode.FullName;

        [Header("References")]
        [SerializeField] private Image m_optionalIcon = null;
        [SerializeField] private TextMeshProUGUI m_optionalLabel = null;

        public void Start()
        {
            UpdateUI();
        }

        public void UpdateUI()
        {
            TermDefinition definition = GameManager.Config.GetTermDefinition(m_termID);

            if (m_optionalIcon)
            {
                m_optionalIcon.sprite = definition.icon;
            }

            if (m_optionalLabel)
            {
                m_optionalLabel.text = string.Format(m_textFormat, m_displayMode == ETermDisplayMode.FullName ? definition.fullName : definition.shortName);
            }
        }
    }
}
