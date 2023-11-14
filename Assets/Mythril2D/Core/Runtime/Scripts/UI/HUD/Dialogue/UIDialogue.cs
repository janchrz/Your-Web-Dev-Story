using UnityEngine;
using UnityEngine.InputSystem;

namespace Gyvr.Mythril2D
{
    public class UIDialogue : MonoBehaviour
    {
        // Inspector Settings
        [SerializeField] private UIDialogueMessageBox m_messageBox = null;
        [SerializeField] private UIDialogueChoiceBox m_choiceBox = null;

        private DialogueNode m_currentNode = null;

        private void Start()
        {
            DialogueChannel mainChannel = GameManager.DialogueSystem.Main;

            mainChannel.dialogueStarted.AddListener(OnDialogueStarted);
            mainChannel.dialogueEnded.AddListener(OnDialogueEnded);
            mainChannel.dialogueNodeChanged.AddListener(OnDialogueNodeChanged);

            GameManager.InputSystem.ui.skip.performed += OnSkip;

            m_choiceBox.Hide();
        }

        private void OnDestroy()
        {
            DialogueChannel mainChannel = GameManager.DialogueSystem.Main;

            mainChannel.dialogueStarted.RemoveListener(OnDialogueStarted);
            mainChannel.dialogueEnded.RemoveListener(OnDialogueEnded);
            mainChannel.dialogueNodeChanged.RemoveListener(OnDialogueNodeChanged);

            GameManager.InputSystem.ui.skip.performed -= OnSkip;
        }

        private void OnDialogueStarted(DialogueTree dialogue)
        {
            m_messageBox.Show();
            GameManager.GameStateSystem.AddLayer(EGameState.Dialogue);
        }

        private void OnDialogueEnded(DialogueTree dialogue)
        {
            m_messageBox.Hide();
            GameManager.GameStateSystem.RemoveLayer(EGameState.Dialogue);
        }

        private void OnDialogueNodeChanged(DialogueNode node)
        {
            m_currentNode = node;

            if (node != null)
            {
                m_messageBox.SetText(node.speaker, node.text, node.optionCount == 1);

                if (m_currentNode.optionCount < 2)
                {
                    m_choiceBox.Hide();
                }
            }
            else
            {
                m_choiceBox.Hide();
            }
        }

        private void OnMessageBoxTextAnimationFinished()
        {
            UpdateChoiceBox();
        }

        private void UpdateChoiceBox()
        {
            if (m_currentNode != null)
            {
                if (m_currentNode.optionCount > 1)
                {
                    m_choiceBox.Show(m_currentNode.options);
                }
            }
        }

        public void OnSkip(InputAction.CallbackContext context)
        {
            if (!m_messageBox.IsTextAnimationFinished())
            {
                m_messageBox.SkipTextAnimation();
            }
            else
            {
                GameManager.DialogueSystem.Main.TrySkipping();
            }
        }

        public void OnOptionClicked(int option)
        {
            GameManager.DialogueSystem.Main.Next(option);
        }
    }
}
