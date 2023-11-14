using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    [CustomPropertyDrawer(typeof(ActionHandler))]
    public class ActionHandlerPropertyDrawer : PropertyDrawer
    {
        private const int fieldOffset = 20;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty args = property.FindPropertyRelative("args");

            if (args.isExpanded)
            {
                int lineCount = math.max(args.arraySize, 1) + 3;
                return (EditorGUIUtility.singleLineHeight + 2) * lineCount;
            }
            else
            {
                return EditorGUIUtility.singleLineHeight;
            }
        }

        public bool CreateObjectSelectionPopup<T>(Rect rect, SerializedProperty property) where T : UnityEngine.Object
        {
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);
            T[] instances = new T[guids.Length];
            List<string> names = new List<string>(guids.Length);
            GUIContent[] options = new GUIContent[guids.Length];
            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                instances[i] = AssetDatabase.LoadAssetAtPath<T>(path);
                string name = instances[i].name;
                names.Add(name);
                options[i] = new GUIContent(name);
            }

            int previousSelected = property.objectReferenceValue ? names.IndexOf((property.objectReferenceValue as ScriptableAction).name.ToString()) : -1;
            int currentSelected = EditorGUI.Popup(rect, previousSelected, options);

            if (previousSelected != currentSelected)
            {
                property.objectReferenceValue = instances[currentSelected];
                return true;
            }

            return false;
        }

        private EActionArgType TypeToActionArgType(Type type)
        {
            if (type == typeof(int)) return EActionArgType.Int;
            if (type == typeof(bool)) return EActionArgType.Bool;
            if (type == typeof(float)) return EActionArgType.Float;
            if (type == typeof(string)) return EActionArgType.String;
            if (type.IsSubclassOf(typeof(UnityEngine.Object))) return EActionArgType.Object;
            
            return EActionArgType.Null;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty action = property.FindPropertyRelative("action");
            SerializedProperty args = property.FindPropertyRelative("args");

            Rect actionRect = new Rect(position.x, position.y, position.width / 3, EditorGUIUtility.singleLineHeight);
            Rect argsRect = new Rect(actionRect.xMax + fieldOffset, position.y, (position.width / 3) * 2 - fieldOffset, EditorGUIUtility.singleLineHeight);

            EditorGUI.BeginProperty(position, label, property);

            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            CreateObjectSelectionPopup<ScriptableAction>(actionRect, action);
            ScriptableAction actionInstance = (action.objectReferenceValue as ScriptableAction);
            if (actionInstance)
            {
                EditorGUI.PropertyField(argsRect, args);

                args.arraySize = math.clamp(args.arraySize, actionInstance.RequiredArgs.Length, actionInstance.RequiredArgs.Length + actionInstance.OptionalArgs.Length);

                // Forces args types based on the provided ScriptableAction Type requirements
                for (int i = 0; i < args.arraySize; i++)
                {
                    SerializedProperty elementProperty = args.GetArrayElementAtIndex(i);
                    SerializedProperty typeProperty = elementProperty.FindPropertyRelative("type");
                    Type expectedArgType = actionInstance.GetArgTypeAtIndex(i);
                    typeProperty.enumValueIndex = (int)TypeToActionArgType(expectedArgType);
                }
            }

            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}
