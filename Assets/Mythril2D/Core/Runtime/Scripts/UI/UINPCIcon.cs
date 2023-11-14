using UnityEngine;
using UnityEngine.U2D.Animation;

namespace Gyvr.Mythril2D
{
    public class UINPCIcon : MonoBehaviour
    {
        public enum EIconType
        {
            None,
            QuestAvailable,
            QuestCompleted,
            QuestInProgress,
            Speech,
            Love
        }

        [SerializeField] private SpriteResolver m_spriteResolver = null;

        private void Awake()
        {
            Debug.Assert(m_spriteResolver, ErrorMessages.InspectorMissingComponentReference<SpriteResolver>());
        }

        public void SetIconType(EIconType iconType)
        {
            switch (iconType)
            {
                case EIconType.None:
                    m_spriteResolver.SetCategoryAndLabel("None", "None");
                    break;

                case EIconType.QuestAvailable:
                    m_spriteResolver.SetCategoryAndLabel("Quest", "Available");
                    break;

                case EIconType.QuestCompleted:
                    m_spriteResolver.SetCategoryAndLabel("Quest", "Completed");
                    break;

                case EIconType.QuestInProgress:
                    m_spriteResolver.SetCategoryAndLabel("Quest", "In Progress");
                    break;

                case EIconType.Speech:
                    m_spriteResolver.SetCategoryAndLabel("Expression", "Speech");
                    break;

                case EIconType.Love:
                    m_spriteResolver.SetCategoryAndLabel("Expression", "Heart");
                    break;
            }
        }
    }
}
