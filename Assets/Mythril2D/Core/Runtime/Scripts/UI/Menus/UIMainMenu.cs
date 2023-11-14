using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Gyvr.Mythril2D
{
    public class UIMainMenu : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Button m_defaultSelectedButton = null;

        [Header("References")]
        [SerializeField] private UISaveFile[] m_saveFiles = null;
        [SerializeField] private Button[] m_eraseButtons = null;

        public void OnEnable()
        {
            UpdateUI();
            SelectDefaultButton();
        }

        private void UpdateUI(bool skipItemSlots = false)
        {
            for (int i = 0; i < m_saveFiles.Length; ++i)
            {
                UISaveFile saveFile = m_saveFiles[i];
                Button eraseButton = i < m_eraseButtons.Length ? m_eraseButtons[i] : null;
                saveFile.UpdateUI();
                eraseButton.interactable = !saveFile.isEmpty;
            }
        }

        public void StartNewGameFromDefaultSaveFile(SaveFile saveFile)
        {
            SceneManager.LoadSceneAsync(GameManager.Config.gameplayScene).completed += (operation) =>
            {
                GameManager.SaveSystem.LoadDefaultSaveFile(saveFile);
            };
        }

        private void SelectDefaultButton()
        {
            m_defaultSelectedButton.Select();
        }

        public void EraseSaveFile(UISaveFile saveFile)
        {
            SaveSystem.EraseSaveData(saveFile.name);
            UpdateUI();
            SelectDefaultButton();
        }

        private void OnSaveFileClicked(SaveFileActionDesc desc)
        {
            SceneManager.LoadSceneAsync(GameManager.Config.gameplayScene).completed += (operation) =>
            {
                switch (desc.action)
                {
                    case SaveFileActionType.Load:
                        GameManager.SaveSystem.LoadFromFile(desc.filename);
                        break;
                }
            };
        }
    }
}
