using System;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    public enum EActionArgType
    {
        Null,
        Int,
        Bool,
        Float,
        String,
        Object
    }

    [Serializable]
    public struct ActionArg
    {
        public EActionArgType type;

        public int int_value;
        public bool bool_value;
        public float float_value;
        public string string_value;
        public UnityEngine.Object object_value;
    }

    [Serializable]
    public class ActionHandler
    {
        public ScriptableAction action = null;
        public ActionArg[] args = null;

        private object[] GetFormattedArgs()
        {
            object[] result = new object[args.Length];

            for (int i = 0; i < result.Length; ++i)
            {
                switch (args[i].type)
                {
                    case EActionArgType.Null: result[i] = null; break;
                    case EActionArgType.Int: result[i] = args[i].int_value; break;
                    case EActionArgType.Bool: result[i] = args[i].bool_value; break;
                    case EActionArgType.Float: result[i] = args[i].float_value; break;
                    case EActionArgType.String: result[i] = args[i].string_value; break;
                    case EActionArgType.Object: result[i] = args[i].object_value; break;
                }
            }

            return result;
        }

        public void Execute()
        {
            if (action != null)
            {
                action.Execute(GetFormattedArgs());
            }
            else
            {
                Debug.LogError("No ScriptableAction specified, please provide a reference or switch the ActionHandler.type to EActionType.None");
            }
        }
    }
}
