using System.Collections.Generic;

namespace Gyvr.Mythril2D
{
    public enum EDialogueMessageType
    {
        None,
        Custom,
        Accept,
        Decline
    }

    [System.Serializable]
    public class DialogueMessageFeed
    {
        public HashSet<DialogueMessage> messages = new HashSet<DialogueMessage>();

        public void Add(DialogueMessage message)
        {
            messages.Add(message);
        }

        public bool Contains(string message)
        {
            return Contains(new DialogueMessage
            {
                type = EDialogueMessageType.Custom,
                customMessage = message
            });
        }

        public bool Contains(EDialogueMessageType type)
        {
            return Contains(new DialogueMessage
            {
                type = type,
                customMessage = string.Empty
            });
        }

        public bool Contains(DialogueMessage message)
        {
            return messages.Contains(message);
        }
    }

    [System.Serializable]
    public struct DialogueMessage
    {
        public EDialogueMessageType type;
        public string customMessage;

        public override string ToString()
        {
            switch (type)
            {
                case EDialogueMessageType.None: return string.Empty;
                case EDialogueMessageType.Custom: return customMessage.ToLower();
            }

            return type.ToString().ToLower();
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return ToString().Equals(obj.ToString());
        }

        public bool Equals(EDialogueMessageType type)
        {
            return Equals(new DialogueMessage { type = type });
        }
    }

    [System.Serializable]
    public struct DialogueNodeOption
    {
        public string name;
        public DialogueNode node;
        public DialogueMessage message;
    }

    public class DialogueNode
    {
        public string text;
        public string speaker;
        public DialogueNodeOption[] options;
        public ActionHandler[] toExecuteOnStart;
        public ActionHandler[] toExecuteOnCompletion;

        public int optionCount => options != null ? options.Length : 0;

        public DialogueNode(string text = "", string speaker = "", DialogueNodeOption[] options = null, ActionHandler[] toExecuteOnStart = null, ActionHandler[] toExecuteOnCompletion = null)
        {
            this.text = text;
            this.speaker = speaker;
            this.options = options;
            this.toExecuteOnStart = toExecuteOnStart;
            this.toExecuteOnCompletion = toExecuteOnCompletion;
        }

        public DialogueNode GetNext(int option)
        {
            if (options != null && option < options.Length)
            {
                return options[option].node;
            }
            else
            {
                return null;
            }
        }
    }
}
