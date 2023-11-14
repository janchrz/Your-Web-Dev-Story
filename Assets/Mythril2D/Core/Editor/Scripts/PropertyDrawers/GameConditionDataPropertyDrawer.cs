using System;
using UnityEditor;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    [CustomPropertyDrawer(typeof(GameConditionData))]
    public class GameConditionDataPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty type = property.FindPropertyRelative("type");
            SerializedProperty state = property.FindPropertyRelative("state");

            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, label);

            EGameConditionType conditionType = (EGameConditionType)type.enumValueIndex;

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            if (conditionType != EGameConditionType.None)
            {
                var stateRect = new Rect(position.x, position.y, position.width * 0.15f, EditorGUIUtility.singleLineHeight);
                var typeRect = new Rect(stateRect.xMax, position.y, position.width * 0.4f, EditorGUIUtility.singleLineHeight);
                var valueRect = new Rect(typeRect.xMax, position.y, position.width * 0.45f, EditorGUIUtility.singleLineHeight);

                type.enumValueIndex = Convert.ToInt32(EditorGUI.EnumPopup(typeRect, GUIContent.none, conditionType));

                EditorGUI.PropertyField(stateRect, state, GUIContent.none);

                switch (conditionType)
                {
                    default: EditorGUI.LabelField(valueRect, "Select a type"); break;
                    case EGameConditionType.QuestAvailable:
                    case EGameConditionType.QuestFullfilled:
                    case EGameConditionType.QuestCompleted:
                    case EGameConditionType.QuestActive: DrawProperty(property, valueRect, "quest"); break;
                    case EGameConditionType.TaskActive: DrawProperty(property, valueRect, "task"); break;
                    case EGameConditionType.GameFlagSet: DrawProperty(property, valueRect, "flagID"); break;
                }
            }
            else
            {
                var typeRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
                type.enumValueIndex = Convert.ToInt32(EditorGUI.EnumPopup(typeRect, GUIContent.none, conditionType));
            }

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }

        private void DrawProperty(SerializedProperty property, Rect position, string propertyName)
        {
            EditorGUI.PropertyField(position, property.FindPropertyRelative(propertyName), GUIContent.none);
        }
    }
}
