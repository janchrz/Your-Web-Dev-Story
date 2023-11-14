using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Gyvr.Mythril2D
{
    public class UIGameMenuEntry : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        public enum EGameMenuAction
        {
            None,
            OpenInventory,
            OpenJournal,
            OpenSaveMenu,
            OpenAbilities,
            OpenCharacter,
            GoToMainMenu
        }

        [Header("Settings")]
        [SerializeField] private EGameMenuAction m_action = EGameMenuAction.None;

        [Header("References")]
        [SerializeField] private Button m_button = null;
        [SerializeField] private TextMeshProUGUI m_text = null;

        private void Awake()
        {
            m_button.onClick.AddListener(OnButtonBlicked);
            m_text.enabled = false;
        }

        public void OnDeselect(BaseEventData eventData)
        {
            m_text.enabled = false;
        }

        public void OnSelect(BaseEventData eventData)
        {
            m_text.enabled = true;
            SendMessageUpwards("OnGameMenuEntrySelected", m_button);
        }

        private void OnButtonBlicked()
        {
            switch (m_action)
            {
                case EGameMenuAction.OpenJournal:
                    GameManager.NotificationSystem.journalRequested.Invoke();
                    break;

                case EGameMenuAction.OpenCharacter:
                    GameManager.NotificationSystem.statsRequested.Invoke();
                    break;

                case EGameMenuAction.OpenSaveMenu:
                    GameManager.NotificationSystem.saveMenuRequested.Invoke();
                    break;

                case EGameMenuAction.OpenInventory:
                    GameManager.NotificationSystem.inventoryRequested.Invoke();
                    break;

                case EGameMenuAction.OpenAbilities:
                    GameManager.NotificationSystem.spellBookRequested.Invoke();
                    break;

                case EGameMenuAction.GoToMainMenu:
                    SceneManager.LoadScene(GameManager.Config.mainMenuSceneName);
                    break;
            }
        }
    }
}
