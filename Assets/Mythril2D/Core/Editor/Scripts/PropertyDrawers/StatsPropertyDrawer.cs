using UnityEditor;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    [CustomPropertyDrawer(typeof(Stats))]
    public class StatsPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = EditorGUIUtility.singleLineHeight;

            if (property.isExpanded)
            {
                height += Stats.StatCount * (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);
            }

            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty values = property.FindPropertyRelative("m_values");

            if (values.arraySize != Stats.StatCount)
            {
                values.ClearArray();
                values.arraySize = Stats.StatCount;
            }

            EditorGUI.BeginProperty(position, label, property);

            var indent = EditorGUI.indentLevel;

            position.height = EditorGUIUtility.singleLineHeight;

            property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label, true);

            if (property.isExpanded)
            {
                ++EditorGUI.indentLevel;
                Rect statRect = new Rect(position.x, position.y + position.height + EditorGUIUtility.standardVerticalSpacing, position.width, EditorGUIUtility.singleLineHeight);

                for (int i = 0; i < values.arraySize; ++i)
                {
                    SerializedProperty statProperty = values.GetArrayElementAtIndex(i);
                    EditorGUI.PropertyField(statRect, statProperty, new GUIContent(((EStat)i).ToString()));
                    statRect.y += statRect.height + EditorGUIUtility.standardVerticalSpacing;
                }

            }

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }
}
