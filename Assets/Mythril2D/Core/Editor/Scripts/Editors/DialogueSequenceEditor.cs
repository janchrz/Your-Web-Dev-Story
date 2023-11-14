using UnityEditor;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    [CustomEditor(typeof(DialogueSequence))]
    public class DialogueSequenceEditor : Editor
    {
        // Private Members
        private SerializedProperty m_lines;
        private SerializedProperty m_options;
        private SerializedProperty m_toExecuteOnStart;
        private SerializedProperty m_toExecuteOnCompletion;

        void OnEnable()
        {
            m_lines = serializedObject.FindProperty("lines");
            m_options = serializedObject.FindProperty("options");
            m_toExecuteOnStart = serializedObject.FindProperty("toExecuteOnStart");
            m_toExecuteOnCompletion = serializedObject.FindProperty("toExecuteOnCompletion");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(m_lines);
            EditorGUILayout.Separator();
            m_options.arraySize = EditorGUILayout.IntSlider("Option Count", m_options.arraySize, 0, 3);

            for (int i = 0; i < m_options.arraySize; ++i)
            {
                DisplayOption(i, m_options.arraySize == 1);
            }
            EditorGUILayout.Separator();
            EditorGUILayout.PropertyField(m_toExecuteOnStart);
            EditorGUILayout.PropertyField(m_toExecuteOnCompletion);

            serializedObject.ApplyModifiedProperties();
        }

        private void DisplayOption(int index, bool hideOptionName)
        {
            SerializedProperty property = m_options.GetArrayElementAtIndex(index);

            EditorGUILayout.BeginHorizontal();

            if (!hideOptionName)
            {
                EditorGUILayout.PropertyField(property.FindPropertyRelative("name"), GUIContent.none);
            }

            EditorGUILayout.PropertyField(property.FindPropertyRelative("sequence"), GUIContent.none);
            EditorGUILayout.PropertyField(property.FindPropertyRelative("message"), GUIContent.none);
            EditorGUILayout.EndHorizontal();
        }
    }
}
