using System;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    public enum EMessagePropagationMode
    {
        BroadcastMessage,
        SendMessage,
        SendMessageUpwards
    }

    [Serializable]
    public struct MessageData
    {
        public string message;
        public EMessagePropagationMode propagationMode;
        public SendMessageOptions options;

        public bool IsValid() => !string.IsNullOrWhiteSpace(message);
    }

    public class StateMessageDispatcher : StateMachineBehaviour
    {
        public MessageData animationStartMessage;
        public MessageData animationEndMessage;


        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (animationStartMessage.IsValid())
            {
                PropagateMessage(animator, animationStartMessage);
            }
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (animationEndMessage.IsValid())
            {
                PropagateMessage(animator, animationEndMessage);
            }
        }

        private void PropagateMessage(Component source, MessageData messageData)
        {
            switch (messageData.propagationMode)
            {
                case EMessagePropagationMode.BroadcastMessage:
                    source.BroadcastMessage(messageData.message, messageData.options);
                    break;

                case EMessagePropagationMode.SendMessage:
                    source.SendMessage(messageData.message, messageData.options);
                    break;

                case EMessagePropagationMode.SendMessageUpwards:
                    source.SendMessageUpwards(messageData.message, messageData.options);
                    break;
            }
        }
    }
}
