using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace Gyvr.Mythril2D
{
    public class WelcomeWindow : EditorWindow
    {
        private const string ShowOnImportKey = "Mythril2DShowWelcomeWindowOnImport";

        [InitializeOnLoadMethod]
        private static void InitializeOnLoad()
        {
            if (EditorPrefs.GetBool(ShowOnImportKey, true))
            {
                EditorApplication.update += ShowWelcomeWindowOnNextUpdate;
                EditorPrefs.SetBool(ShowOnImportKey, false);
            }
        }

        [MenuItem("Window/Mythril2D/Welcome")]
        public static void ShowWindow()
        {
            EditorWindow window = GetWindow<WelcomeWindow>();
            window.titleContent = new GUIContent("Welcome");
            window.ShowModalUtility();
        }

        void OnGUI()
        {
            minSize = maxSize = new Vector2(400, 140);

            GUILayout.Space(10);
            GUILayout.Label("Thank you for installing Mythril2D!", new GUIStyle(EditorStyles.label)
            {
                fontSize = 20,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
            });
            GUILayout.Space(10);
            GUILayout.Label("Get started with Mythril2D by checking out our series of tutorials directly embedded in the Unity Editor", new GUIStyle(EditorStyles.label)
            {
                fontSize = 14,
                alignment = TextAnchor.MiddleCenter,
                wordWrap = true
            });
            GUILayout.Space(10);
            if (GUILayout.Button("View Tutorials", new GUIStyle(GUI.skin.button)
            {
                fontSize = 16,
                fontStyle = FontStyle.Bold
            }, GUILayout.Height(40)))
            {
                TutorialsWindow.ShowWindow();
                Close();
            }
        }

        private static void ShowWelcomeWindowOnNextUpdate()
        {
            EditorApplication.update -= ShowWelcomeWindowOnNextUpdate;
            ShowWindow();
        }
    }
}
