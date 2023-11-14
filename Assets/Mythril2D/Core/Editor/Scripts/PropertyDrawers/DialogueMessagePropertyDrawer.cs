using System;
using UnityEditor;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    [CustomPropertyDrawer(typeof(DialogueMessage))]
    public class DialogueMessagePropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty type = property.FindPropertyRelative("type");

            EditorGUI.BeginProperty(position, label, property);

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            EDialogueMessageType selectedType = (EDialogueMessageType)type.enumValueIndex;

            Rect typeRect;

            if (selectedType == EDialogueMessageType.Custom)
            {
                typeRect = new Rect(position.x, position.y, position.width / 2, EditorGUIUtility.singleLineHeight);
                type.enumValueIndex = Convert.ToInt32(EditorGUI.EnumPopup(typeRect, GUIContent.none, (EDialogueMessageType)type.enumValueIndex));
                SerializedProperty customMessage = property.FindPropertyRelative("customMessage");
                var customMessageRect = new Rect(typeRect.xMax, position.y, position.width / 2, EditorGUIUtility.singleLineHeight);
                EditorGUI.PropertyField(customMessageRect, customMessage, GUIContent.none);
            }
            else
            {
                typeRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
                type.enumValueIndex = Convert.ToInt32(EditorGUI.EnumPopup(typeRect, GUIContent.none, (EDialogueMessageType)type.enumValueIndex));
            }

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }
}
