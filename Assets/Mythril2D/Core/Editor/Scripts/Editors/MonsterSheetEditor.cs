using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    [CustomEditor(typeof(MonsterSheet))]
    public class MonsterSheeEditor : Editor
    {
        private int m_previewLevel = 1;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            MonsterSheet sheet = target as MonsterSheet;

            EditorGUILayout.Separator();

            EditorGUILayout.LabelField("Evolution Preview", EditorStyles.boldLabel);

            m_previewLevel = EditorGUILayout.IntSlider("Level", m_previewLevel, 1, Stats.MaxLevel);

            Stats previewStats = sheet.stats[m_previewLevel];
            int experience = sheet.experience[m_previewLevel];
            int money = sheet.money[m_previewLevel];

            int total = previewStats.GetTotal();
            int average = (int)math.round(total / 5.0f);

            GUI.enabled = false;
            EditorGUILayout.IntField("Health", previewStats[EStat.Health]);
            EditorGUILayout.IntField("Mana", previewStats[EStat.Mana]);
            EditorGUILayout.IntField("Physical Attack", previewStats[EStat.PhysicalAttack]);
            EditorGUILayout.IntField("Magical Attack", previewStats[EStat.MagicalAttack]);
            EditorGUILayout.IntField("Physical Defense", previewStats[EStat.PhysicalDefense]);
            EditorGUILayout.IntField("Magical Defense", previewStats[EStat.MagicalDefense]);
            EditorGUILayout.IntField("Agility", previewStats[EStat.Agility]);
            EditorGUILayout.IntField("Luck", previewStats[EStat.Luck]);
            EditorGUILayout.Space();
            EditorGUILayout.IntField("Total", total);
            EditorGUILayout.IntField("Average", average);
            EditorGUILayout.Space();
            EditorGUILayout.IntField("Experience", experience);
            EditorGUILayout.IntField("Money", money);
            EditorGUILayout.Space();
            GUI.enabled = true;
        }
    }
}
