using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Gyvr.Mythril2D
{
    public class DialogueChannel : MonoBehaviour
    {
        // Public Events
        public UnityEvent<DialogueTree> dialogueStarted = new UnityEvent<DialogueTree>();
        public UnityEvent<DialogueTree> dialogueEnded = new UnityEvent<DialogueTree>();
        public UnityEvent<DialogueNode> dialogueNodeChanged = new UnityEvent<DialogueNode>();

        // Private Members
        private DialogueTree m_dialogueTree = null;
        private DialogueNode m_currentNode = null;
        private Queue<DialogueTree> m_dialogueQueue = new Queue<DialogueTree>();

        public void AddToQueue(DialogueTree dialogue)
        {
            m_dialogueQueue.Enqueue(dialogue);
        }

        public void ClearQueue()
        {
            m_dialogueQueue.Clear();
        }

        public void PlayNow(string line, params object[] args)
        {
            AddToQueue(new DialogueTree(new DialogueNode(StringFormatter.Format(line, args))));
            PlayQueue();
        }

        public void PlayNow(DialogueTree dialogue)
        {
            AddToQueue(dialogue);
            PlayQueue();
        }

        public void PlayQueue()
        {
            if (m_dialogueTree == null)
            {
                if (m_dialogueQueue.Count > 0)
                {
                    Play(m_dialogueQueue.Dequeue());
                }
            }
        }

        public bool TrySkipping()
        {
            if (m_currentNode != null && m_currentNode.optionCount < 2)
            {
                Next();
                return true;
            }

            return false;
        }

        public void Next(int option = 0)
        {
            m_dialogueTree.OnNodeExecuted(m_currentNode, option);

            if (m_currentNode.toExecuteOnCompletion != null)
            {
                foreach (ActionHandler actionHandler in m_currentNode.toExecuteOnCompletion)
                {
                    actionHandler.Execute();
                }
            }

            SetCurrentNode(m_currentNode.GetNext(option));
        }

        public bool IsPlaying() => m_dialogueTree != null;

        private void Play(DialogueTree dialogue)
        {
            if (dialogue.root != null)
            {
                m_dialogueTree = dialogue;
                dialogueStarted.Invoke(m_dialogueTree);
                m_dialogueTree.dialogueStarted.Invoke();
                SetCurrentNode(dialogue.root);
            }
            else
            {
                Debug.LogError("Cannot start a dialogue with a null entry point node.");
            }
        }

        private void SetCurrentNode(DialogueNode node)
        {
            m_currentNode = node;
            dialogueNodeChanged.Invoke(m_currentNode);

            if (m_currentNode == null)
            {
                OnLastNodeReached();
            }
            else
            {
                if (m_currentNode.toExecuteOnStart != null)
                {
                    foreach (ActionHandler actionHandler in m_currentNode.toExecuteOnStart)
                    {
                        actionHandler.Execute();
                    }
                }
            }
        }

        private void OnLastNodeReached()
        {
            DialogueTree tree = m_dialogueTree;

            if (tree != null)
            {
                dialogueEnded.Invoke(tree);
                tree.dialogueEnded.Invoke(m_dialogueTree.messages);
                m_dialogueTree = null;
                PlayQueue();
            }
        }
    }
}
