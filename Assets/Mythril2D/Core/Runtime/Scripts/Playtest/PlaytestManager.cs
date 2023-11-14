using UnityEngine;
using UnityEngine.SceneManagement;

#if DEBUG
namespace Gyvr.Mythril2D
{
    public class PlaytestManager : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private string m_gameplaySceneLayer = "Gameplay";
        [SerializeField] private PlaytestScenario m_scenario = null;

        private void Awake()
        {
            // The Playtest Manager only works when loading a map directly without a GameManager
            if (!GameManager.Exists() && m_scenario != null)
            {
                string scene = SceneManager.GetActiveScene().name;

                // Load Gameplay as the main scene before adding the current scene as a layer
                AsyncOperation asyncOp = SceneManager.LoadSceneAsync(m_gameplaySceneLayer, LoadSceneMode.Additive);

                asyncOp.completed += (operation) =>
                {
                    GameManager.NotificationSystem.mapLoaded.Invoke();

                    // Update the save file
                    SaveFileData save = GameManager.SaveSystem.DuplicateSaveFile(m_scenario.SaveFile);

                    save.map = string.Empty;
                    save.player.position = m_scenario.transform.position;

                    GameManager.SaveSystem.LoadSaveFile(save);
                    GameManager.MapLoadingSystem.SetActiveMap(scene);
                };
            }
        }
    }
}
#endif
