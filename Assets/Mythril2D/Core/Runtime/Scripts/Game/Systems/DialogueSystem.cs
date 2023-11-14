using UnityEngine;

namespace Gyvr.Mythril2D
{
    public class DialogueSystem : AGameSystem
    {
        [SerializeField] private DialogueChannel m_mainChannel = null;

        public DialogueChannel Main => m_mainChannel;
    }
}
