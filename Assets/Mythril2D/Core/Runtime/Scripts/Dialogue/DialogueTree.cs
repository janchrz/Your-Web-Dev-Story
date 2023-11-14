using UnityEngine.Events;

namespace Gyvr.Mythril2D
{
    public class DialogueTree
    {
        // Public Members
        public UnityEvent dialogueStarted = new UnityEvent();
        public UnityEvent<DialogueMessageFeed> dialogueEnded = new UnityEvent<DialogueMessageFeed>();

        // Public Getters
        public DialogueNode root => m_root;
        public DialogueMessageFeed messages => m_messages;

        // Private Members
        private DialogueNode m_root = null;
        private DialogueMessageFeed m_messages = new DialogueMessageFeed();

        public DialogueTree(DialogueNode root)
        {
            m_root = root;
        }

        public void OnNodeExecuted(DialogueNode node, int option)
        {
            if (node.options != null && option < node.options.Length)
            {
                DialogueMessage message = node.options[option].message;

                if (!string.IsNullOrWhiteSpace(message.ToString()))
                {
                    m_messages.Add(message);
                }
            }
        }
    }
}
