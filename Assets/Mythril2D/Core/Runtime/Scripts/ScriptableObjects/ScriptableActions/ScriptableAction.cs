using System;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    public abstract class ScriptableAction : ScriptableObject
    {
        public abstract void Execute(params object[] args);
        public abstract Type[] RequiredArgs { get; }
        public abstract Type[] OptionalArgs { get; }
        public abstract string[] ArgDescriptions { get; }

        public Type GetArgTypeAtIndex(int index)
        {
            if (index < RequiredArgs.Length)
            {
                return RequiredArgs[index];
            }
            else
            {
                return OptionalArgs[index - RequiredArgs.Length];
            }
        }

        public T GetRequiredArgAtIndex<T>(int index, params object[] args)
        {
            if (index < args.Length)
            {
                if (args[index] != null && args[index] is T)
                {
                    return (T)args[index];
                }

                Debug.LogError($"ScriptableAction args[{index}] doesn't match expected type: {typeof(T).Name}");
                return default;
            }

            Debug.LogError($"ScriptableAction args[{index}] is outside of args.Length ({args.Length})");
            return default;
        }

        public T GetOptionalArgAtIndex<T>(int index, T defaultValue, params object[] args)
        {
            if (index < args.Length)
            {
                if (args[index] != null && args[index] is T)
                {
                    return (T)args[index];
                }

                return defaultValue;
            }

            return defaultValue;
        }
    }
}
