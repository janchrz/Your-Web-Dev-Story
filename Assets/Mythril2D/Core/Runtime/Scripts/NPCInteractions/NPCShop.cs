using UnityEngine;

namespace Gyvr.Mythril2D
{
    public class NPCShop : ANPCInteraction
    {
        [Header("Dialogues")]
        [SerializeField] private DialogueSequence m_dialogue = null;

        [Header("References")]
        [SerializeField] private Shop m_shop = null;

        public override bool Interact(CharacterBase sender)
        {
            if (m_shop != null)
            {
                Say(m_dialogue, (messages) =>
                {
                    if (messages.Contains(EDialogueMessageType.Accept))
                    {
                        GameManager.NotificationSystem.shopRequested.Invoke(m_shop);
                    }
                });

                return true;
            }

            return false;
        }
    }
}
