using UnityEngine;
using UnityEngine.UI;

namespace Gyvr.Mythril2D
{
    public class UIGameMenu : MonoBehaviour, IUIMenu
    {
        [Header("References")]
        [SerializeField] private UIGameMenuEntry[] m_menus;
        [SerializeField] private GameObject[] m_disableWhileOpened = null;

        [Header("Audio")]
        [SerializeField] private AudioClipResolver m_pauseSound;
        [SerializeField] private AudioClipResolver m_resumeSound;

        private Selectable m_selected = null;

        public GameObject FindSomethingToSelect()
        {
            if (m_selected)
            {
                return m_selected.gameObject;
            }

            return null;
        }

        public void OnMenuPushed()
        {
            GameManager.NotificationSystem.audioPlaybackRequested.Invoke(m_pauseSound);
        }

        public void OnMenuPopped()
        {
            GameManager.NotificationSystem.audioPlaybackRequested.Invoke(m_resumeSound);
        }

        public void Show(params object[] args)
        {
            gameObject.SetActive(true);

            foreach (GameObject gameObject in m_disableWhileOpened)
            {
                gameObject.SetActive(false);
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);

            foreach (GameObject gameObject in m_disableWhileOpened)
            {
                gameObject.SetActive(true);
            }
        }

        public void EnableInteractions(bool enable)
        {
            CanvasGroup canvasGroup = GetComponent<CanvasGroup>();

            if (canvasGroup)
            {
                canvasGroup.interactable = enable;
            }
        }

        public void Init()
        {
            m_selected?.Select();
        }

        private void OnGameMenuEntrySelected(Selectable selected)
        {
            m_selected = selected;
        }
    }
}
