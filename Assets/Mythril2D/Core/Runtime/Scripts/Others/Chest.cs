using UnityEngine;

namespace Gyvr.Mythril2D
{
    public class Chest : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Animator m_chestAnimator = null;
        [SerializeField] private Animator m_contentAnimator = null;
        [SerializeField] private SpriteRenderer m_contentSpriteRenderer = null;

        [Header("Settings")]
        [SerializeField] private Item m_item = null;
        [SerializeField] private bool m_singleUse = false;
        [SerializeField] private string m_gameFlagID = "chest_00";
        [SerializeField] private string m_openedAnimationParameter = "opened";
        [SerializeField] private string m_contentRevealAnimationParameter = "reveal";
        [SerializeField] private DialogueSequence m_noItemDialogue = null;
        [SerializeField] private DialogueSequence m_hasItemDialogue = null;

        [Header("Audio")]
        [SerializeField] private AudioClipResolver m_openingSound;

        private bool m_hasOpeningAnimation = false;
        private bool m_hasRevealAnimation = false;
        private bool m_opened = false;

        protected void Awake()
        {
            Debug.Assert(m_chestAnimator, ErrorMessages.InspectorMissingComponentReference<Animator>());
            Debug.Assert(m_contentAnimator, ErrorMessages.InspectorMissingComponentReference<Animator>());
            Debug.Assert(m_contentSpriteRenderer, ErrorMessages.InspectorMissingComponentReference<SpriteRenderer>());

            if (m_chestAnimator)
            {
                m_hasOpeningAnimation = AnimationUtils.HasParameter(m_chestAnimator, m_openedAnimationParameter);
            }

            if (m_contentAnimator)
            {
                m_hasRevealAnimation = AnimationUtils.HasParameter(m_contentAnimator, m_contentRevealAnimationParameter);
            }
        }

        private void Start()
        {
            if (m_singleUse && GameManager.GameFlagSystem.Get(m_gameFlagID))
            {
                m_opened = true;
                TryPlayOpeningAnimation(m_opened);
            }
        }

        public bool TryPlayOpeningAnimation(bool open)
        {
            if (m_chestAnimator && m_hasOpeningAnimation)
            {
                m_chestAnimator.SetBool(m_openedAnimationParameter, open);
                return true;
            }

            return false;
        }

        public bool TryPlayContentRevealAnimation()
        {
            if (m_contentSpriteRenderer && m_contentAnimator && m_hasRevealAnimation)
            {
                if (m_item && m_item.icon)
                {
                    m_contentSpriteRenderer.sprite = m_item.icon;
                    m_contentAnimator.SetTrigger(m_contentRevealAnimationParameter);
                    return true;
                }

                return false;
            }

            return false;
        }

        private void OnInteract(CharacterBase sender)
        {
            if (!m_opened)
            {
                TryPlayOpeningAnimation(true);
                TryPlayContentRevealAnimation();

                DialogueTree dialogueTree = null;

                if (m_item != null)
                {
                    GameManager.NotificationSystem.audioPlaybackRequested.Invoke(m_openingSound);
                    GameManager.InventorySystem.AddToBag(m_item);
                    dialogueTree = m_hasItemDialogue.ToDialogueTree(string.Empty, m_item.displayName);
                }
                else
                {
                    dialogueTree = m_noItemDialogue.ToDialogueTree(string.Empty);
                }

                GameManager.DialogueSystem.Main.PlayNow(dialogueTree);

                m_opened = true;

                if (m_singleUse)
                {
                    if (string.IsNullOrWhiteSpace(m_gameFlagID))
                    {
                        Debug.LogError("No ChestID provided while SingleUse is checked. Make sure to provide this chest with an ID");
                    }
                    else
                    {
                        GameManager.GameFlagSystem.Set(m_gameFlagID, true);
                    }
                }
            }
        }
    }
}
