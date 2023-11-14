using UnityEngine;

#if DEBUG
namespace Gyvr.Mythril2D
{
    public class PlaytestScenario : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private SaveFile m_saveFile;

        public SaveFileData SaveFile => m_saveFile.content;
    }
}
#endif
