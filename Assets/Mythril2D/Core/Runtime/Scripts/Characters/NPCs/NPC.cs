using UnityEngine;
using UnityEngine.Events;

namespace Gyvr.Mythril2D
{
    public class NPC : Character<NPCSheet>
    {
        [SerializeField] private ANPCInteraction[] m_interactions = null;

        public void Say(DialogueSequence sequence, UnityAction<DialogueMessageFeed> onDialogueEnded = null, params string[] args)
        {
            DialogueTree dialogueTree = sequence.ToDialogueTree(characterSheet.displayName, args);

            if (onDialogueEnded != null)
            {
                dialogueTree.dialogueEnded.AddListener(onDialogueEnded);
            }

            GameManager.DialogueSystem.Main.PlayNow(dialogueTree);
        }

        private void OnInteract(CharacterBase sender)
        {
            SetLookAtDirection(sender.transform);

            if (m_interactions != null)
            {
                foreach (ANPCInteraction interaction in m_interactions)
                {
                    if (interaction.Interact(this))
                    {
                        break;
                    }
                }
            }
        }
    }
}
