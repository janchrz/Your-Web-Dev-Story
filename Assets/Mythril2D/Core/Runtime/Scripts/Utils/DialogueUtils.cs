namespace Gyvr.Mythril2D
{
    public static class DialogueUtils
    {
        public static DialogueTree CreateDialogueTree(DialogueSequence sequence, string speaker, params string[] args)
        {
            return new DialogueTree(CreateDialogueNodeRecursive(sequence, speaker, args));
        }

        private static DialogueNode CreateDialogueNodeRecursive(DialogueSequence sequence, string speaker, params string[] args)
        {
            DialogueNode root = null;
            DialogueNode previous = null;

            for (int i = 0; i < sequence.lines.Length; ++i)
            {
                DialogueNode current = new DialogueNode();

                current.text = StringFormatter.Format(sequence.lines[i], args);
                current.speaker = speaker;

                if (i == sequence.lines.Length - 1)
                {
                    current.options = new DialogueNodeOption[sequence.options.Length];

                    for (int j = 0; j < current.options.Length; ++j)
                    {
                        current.options[j] = new DialogueNodeOption
                        {
                            name = StringFormatter.Format(sequence.options[j].name),
                            node = sequence.options[j].sequence ? CreateDialogueNodeRecursive(sequence.options[j].sequence, speaker, args) : null,
                            message = sequence.options[j].message
                        };
                    }

                    current.toExecuteOnStart = sequence.toExecuteOnStart;
                    current.toExecuteOnCompletion = sequence.toExecuteOnCompletion;
                }

                if (root == null)
                {
                    root = current;
                }

                if (previous != null)
                {
                    previous.options = new DialogueNodeOption[]
                    {
                    new DialogueNodeOption { node = current }
                    };
                }

                previous = current;
            }

            return root;
        }
    }
}