using UnityEngine;

namespace Gyvr.Mythril2D
{
    [System.Serializable]
    public struct DialogueSequenceOption
    {
        public string name;
        public DialogueSequence sequence;
        public DialogueMessage message;
    }

    [CreateAssetMenu(menuName = AssetMenuIndexer.Mythril2D_Dialogues + nameof(DialogueSequence))]
    public class DialogueSequence : ScriptableObject
    {
        public string[] lines = null;
        public DialogueSequenceOption[] options = null;
        public ActionHandler[] toExecuteOnStart = null;
        public ActionHandler[] toExecuteOnCompletion = null;

        public DialogueTree ToDialogueTree(string speaker, params string[] args)
        {
            return DialogueUtils.CreateDialogueTree(this, speaker, args);
        }
    }
}
