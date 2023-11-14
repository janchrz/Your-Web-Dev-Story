using UnityEngine;

namespace Gyvr.Mythril2D
{
    [CreateAssetMenu(menuName = AssetMenuIndexer.Mythril2D_Save + nameof(SaveFile))]
    public class SaveFile : ScriptableObject
    {
        [SerializeField] private SaveFileData m_content;

        public SaveFileData content => m_content;
    }
}
