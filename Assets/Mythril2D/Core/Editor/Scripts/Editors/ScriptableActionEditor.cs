using System;
using UnityEditor;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    [CustomEditor(typeof(ScriptableAction), true)]
    public class ScriptableActionEditor : Editor
    {
        private int m_argIndex = 0;
        public string GetArgList(Type[] argTypes, string[] argDescriptions)
        {
            string list = string.Empty;

            if (argTypes != null && argTypes.Length > 0)
            {
                foreach (Type argType in argTypes)
                {
                    list += $"[{m_argIndex}] > <b>{argType.Name}</b> <i>({argDescriptions[m_argIndex]})</i>\n";
                    ++m_argIndex;
                }
            }

            return list;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.HelpBox("ScriptableAction are generic objects that can be executed multiple times with different arguments. The provided arguments must match the format explicited below.", MessageType.Info);

            m_argIndex = 0;

            ScriptableAction scriptableAction = target as ScriptableAction;

            string requiredArgList = GetArgList(scriptableAction.RequiredArgs, scriptableAction.ArgDescriptions);
            string optionalArgList = GetArgList(scriptableAction.OptionalArgs, scriptableAction.ArgDescriptions);

            GUIStyle listTitleStyle = new GUIStyle(EditorStyles.label)
            {
                fontSize = 14,
                fontStyle = FontStyle.Bold,
                richText = true,
                wordWrap = true
            };

            GUIStyle listContentStyle = new GUIStyle(EditorStyles.label)
            {
                richText = true,
                wordWrap = true
            };

            if (requiredArgList != string.Empty)
            {
                GUILayout.Label("Required Arguments", listTitleStyle);
                GUILayout.Label(requiredArgList, listContentStyle);
            }

            if (optionalArgList != string.Empty)
            {
                GUILayout.Label("Optional Arguments", listTitleStyle);
                GUILayout.Label(optionalArgList, listContentStyle);
            }
        }
    }
}
