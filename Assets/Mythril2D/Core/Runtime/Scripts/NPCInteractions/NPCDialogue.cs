using UnityEngine;

namespace Gyvr.Mythril2D
{
    [System.Serializable]
    public struct ConditionalDialogueSequence
    {
        public DialogueSequence sequence;
        public GameConditionSet conditions;
    }

    public class NPCDialogue : ANPCInteraction
    {
        [SerializeField] private ConditionalDialogueSequence[] m_sequences;

        public override bool Interact(CharacterBase sender)
        {
            foreach (ConditionalDialogueSequence conditionalSequence in m_sequences)
            {
                if (conditionalSequence.sequence != null && GameConditionChecker.IsConditionSetMet(conditionalSequence.conditions))
                {
                    Say(conditionalSequence.sequence);
                    return true;
                }
            }

            return false;
        }
    }
}
