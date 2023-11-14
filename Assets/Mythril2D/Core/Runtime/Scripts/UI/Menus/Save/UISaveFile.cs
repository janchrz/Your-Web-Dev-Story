using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Gyvr.Mythril2D
{
    enum SaveFileActionType
    {
        Save,
        Load
    }

    struct SaveFileActionDesc
    {
        public SaveFileActionType action;
        public string filename;
    }

    public class UISaveFile : MonoBehaviour, IPointerEnterHandler
    {
        [Header("Settings")]
        [SerializeField] private SaveFileActionType m_action = SaveFileActionType.Load;
        [SerializeField] private string m_saveFileName = null;

        [Header("References")]
        [SerializeField] private TextMeshProUGUI m_details = null;
        [SerializeField] private Button m_button = null;

        public string saveFileName => m_saveFileName;
        public Button button => m_button;
        public bool isEmpty => m_isEmpty;

        private bool m_isEmpty;

        private void Awake()
        {
            m_button.onClick.AddListener(OnClick);
        }

        public void UpdateUI()
        {
            SaveFileData saveFile;

            if (SaveSystem.TryExtractingSaveData(m_saveFileName, out saveFile))
            {
                m_details.text = saveFile.header;
                m_isEmpty = false;
            }
            else
            {
                m_details.text = "Empty";
                m_isEmpty = true;

                if (m_action == SaveFileActionType.Load)
                {
                    m_button.interactable = false;
                }
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            m_button.Select();
        }

        public void OnClick()
        {
            SendMessageUpwards("OnSaveFileClicked", new SaveFileActionDesc
            {
                action = m_action,
                filename = m_saveFileName

            }, SendMessageOptions.RequireReceiver);
        }
    }
}
