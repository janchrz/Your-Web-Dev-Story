using System;
using Unity.Mathematics;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    [CreateAssetMenu(menuName = AssetMenuIndexer.Mythril2D_ScriptableActions + nameof(PlayDialogueSequenceAction))]
    public class PlayDialogueSequenceAction : ScriptableAction
    {
        public override Type[] RequiredArgs => new Type[] { typeof(DialogueSequence) };
        public override Type[] OptionalArgs => new Type[] { typeof(string) };
        public override string[] ArgDescriptions => new string[] { "Dialogue sequence to play", "Name of the speaker" };

        public override void Execute(params object[] args)
        {
            DialogueSequence dialogueSequence = GetRequiredArgAtIndex<DialogueSequence>(0, args);
            string speaker = GetOptionalArgAtIndex<string>(1, string.Empty, args);

            GameManager.DialogueSystem.Main.PlayNow(dialogueSequence.ToDialogueTree(speaker));
        }
    }
}
