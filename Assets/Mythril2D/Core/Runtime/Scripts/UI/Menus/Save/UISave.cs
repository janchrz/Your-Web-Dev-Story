using UnityEngine;

namespace Gyvr.Mythril2D
{
    public class UISave : MonoBehaviour, IUIMenu
    {
        [Header("References")]
        [SerializeField] private UISaveFile[] m_saveFiles = null;

        public void Init() { }

        public void Show(params object[] args)
        {
            Debug.Assert(args.Length == 0, "SaveMenu panel invoked with incorrect arguments");
            gameObject.SetActive(true);
            UpdateUI();
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
            return null;
        }

        private void UpdateUI(bool skipItemSlots = false)
        {
            foreach (UISaveFile saveFile in m_saveFiles)
            {
                saveFile.UpdateUI();
            }
        }

        private void OnSaveFileClicked(SaveFileActionDesc desc)
        {
            GameManager.SaveSystem.SaveToFile(desc.filename);
            UpdateUI();
        }
    }
}
