using UnityEngine;
using UnityEngine.Events;

namespace Gyvr.Mythril2D
{
    [RequireComponent(typeof(NPC))]
    public abstract class ANPCInteraction : MonoBehaviour
    {
        protected NPC m_npc = null;

        public abstract bool Interact(CharacterBase sender);

        protected void Awake()
        {
            m_npc = GetComponent<NPC>();

            if (!m_npc)
            {
                m_npc = GetComponentInParent<NPC>();
            }
        }

        public void Say(DialogueSequence sequence, UnityAction<DialogueMessageFeed> onDialogueEnded = null, params string[] args)
        {
            m_npc.Say(sequence, onDialogueEnded, args);
        }
    }
}
