using System;
using UnityEditor;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    [CustomPropertyDrawer(typeof(ActionArg))]
    public class ActionArgPropertyDrawer : PropertyDrawer
    {
        private const int fieldOffset = 3;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty type = property.FindPropertyRelative("type");

            var typeRect = new Rect(position.x, position.y, position.width / 3, EditorGUIUtility.singleLineHeight);
            var valueRect = new Rect(typeRect.xMax + fieldOffset, position.y, (position.width / 3) * 2 - fieldOffset, EditorGUIUtility.singleLineHeight);

            EditorGUI.BeginProperty(position, label, property);

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            EditorGUI.LabelField(typeRect, ((EActionArgType)type.enumValueIndex).ToString());

            // type.enumValueIndex = Convert.ToInt32(EditorGUI.EnumPopup(typeRect, GUIContent.none, (EActionArgType)type.enumValueIndex));

            switch ((EActionArgType)type.enumValueIndex)
            {
                default: EditorGUI.LabelField(valueRect, "Null"); break;
                case EActionArgType.Int: DrawProperty(property, valueRect, "int"); break;
                case EActionArgType.Bool: DrawProperty(property, valueRect, "bool"); break;
                case EActionArgType.Float: DrawProperty(property, valueRect, "float"); break;
                case EActionArgType.String: DrawProperty(property, valueRect, "string"); break;
                case EActionArgType.Object: DrawProperty(property, valueRect, "object"); break;
            }

            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }

        private void DrawProperty(SerializedProperty property, Rect position, string type)
        {
            EditorGUI.PropertyField(position, property.FindPropertyRelative(string.Format("{0}_value", type)), GUIContent.none);
        }
    }
}
