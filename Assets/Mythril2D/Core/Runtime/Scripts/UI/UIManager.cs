using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Gyvr.Mythril2D
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private CanvasGroup[] m_lockedCanvasGroupsOnDialogue = null;

        private void Start()
        {
            GameManager.DialogueSystem.Main.dialogueStarted.AddListener(OnDialogueStarted);
            GameManager.DialogueSystem.Main.dialogueEnded.AddListener(OnDialogueEnded);
            GameManager.InputSystem.ui.navigate.performed += OnNavigate;
        }

        private void OnDestroy()
        {
            GameManager.DialogueSystem.Main.dialogueStarted.RemoveListener(OnDialogueStarted);
            GameManager.DialogueSystem.Main.dialogueEnded.RemoveListener(OnDialogueEnded);
            GameManager.InputSystem.ui.navigate.performed -= OnNavigate;
        }

        private void SetCanvasGroupsInteractionState(bool enabled)
        {
            foreach (CanvasGroup group in m_lockedCanvasGroupsOnDialogue)
            {
                group.interactable = enabled;
            }
        }

        private void OnDialogueStarted(DialogueTree dialogue)
        {
            SetCanvasGroupsInteractionState(false);
        }

        private void OnDialogueEnded(DialogueTree dialogue)
        {
            // Wait one frame to prevent the input which triggered the dialogue end to be processed by the menu
            StartCoroutine(CoroutineHelpers.ExecuteInXFrames(1, () => SetCanvasGroupsInteractionState(true)));
        }

        private void OnNavigate(InputAction.CallbackContext context)
        {
            if (GameManager.EventSystem.currentSelectedGameObject == null)
            {
                Selectable somethingToSelect = FindObjectOfType<Selectable>();
                if (somethingToSelect)
                {
                    GameManager.EventSystem.SetSelectedGameObject(somethingToSelect.gameObject);
                }
            }
        }
    }
}
