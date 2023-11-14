using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    public class DatabaseWindow : EditorWindow
    {
        private static readonly Type[] Tabs = {
            typeof(HeroSheet),
            typeof(MonsterSheet),
            typeof(NPCSheet),
            typeof(AbilitySheet),
            typeof(Item),
            typeof(Shop),
            typeof(Inn),
            typeof(Quest),
            typeof(DialogueSequence),
            typeof(ScriptableAction),
            typeof(AudioClipResolver),
            typeof(SaveFile),
            typeof(NavigationCursorStyle),
            typeof(GameConfig)
        };

        private static int _selectedTab = 0;
        private static int _selectedIndex = -1;
        private static Vector2 _scrollPos;
        private static ScriptableObject[] _scriptableObjects;
        private static string _searchString = string.Empty;

        [MenuItem("Window/Mythril2D/Database")]
        public static void ShowWindow()
        {
            EditorWindow window = GetWindow<DatabaseWindow>();
            window.titleContent = new GUIContent("Database");
            window.Show();
        }

        public static T[] FindAllInstances<T>() where T : ScriptableObject
        {
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);
            T[] instances = new T[guids.Length];

            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                instances[i] = AssetDatabase.LoadAssetAtPath<T>(path);
            }

            return instances;
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical(GUILayout.Width(100));
            var previousSelectedTab = _selectedTab;
            _selectedTab = GUILayout.SelectionGrid(_selectedTab, Tabs.Select(t => t.Name).ToArray(), 1);

            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical();
            _scriptableObjects = FindAllInstances<ScriptableObject>();
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

            // Search field to filter elements
            var previousSearchString = _searchString;
            _searchString = GUILayout.TextField(_searchString, EditorStyles.toolbarSearchField);

            if (previousSearchString != _searchString || _selectedTab != previousSelectedTab)
            {
                _selectedIndex = -1;
            }

            // Create an array of scriptable object names that match the selected tab
            var visibleScriptableObjects = _scriptableObjects
                .Where(so => Tabs[_selectedTab].IsAssignableFrom(so.GetType()) && so.name.Contains(_searchString))
                .OrderBy(so => so.name);

            var names = visibleScriptableObjects.Select(so => so.name).ToArray();

            // Use a SelectionGrid to display the names and get the selected index
            var previouslySelectedIndex = _selectedIndex;
            _selectedIndex = GUILayout.SelectionGrid(_selectedIndex, names, 1, EditorStyles.objectField);

            // If an index is selected, set the active object to the corresponding scriptable object
            if (_selectedIndex >= 0 && previouslySelectedIndex != _selectedIndex)
            {
                Selection.activeObject = _scriptableObjects.First(so => so.name == names[_selectedIndex]);
            }

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }
    }
}