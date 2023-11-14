using System;
using UnityEditor;

namespace Gyvr.Mythril2D
{
    // TODO: Remove, not necessary anymore
    [CustomEditor(typeof(AbilitySheet), true)]
    public class AbilitySheetEditor : Editor
    {
        private void ShowMessage(MessageType type, string message, params object[] args)
        {
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox(StringFormatter.Format(message, args), type);
        }

        private void ShowError(string message, params object[] args) => ShowMessage(MessageType.Error, message, args);
        private void ShowWarning(string message, params object[] args) => ShowMessage(MessageType.Warning, message, args);

        static Type FindGenericBaseTemplateParameter(Type type, Type genericBase)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == genericBase.GetGenericTypeDefinition())
            {
                return type.GetGenericArguments()[0];
            }
            else if (type.BaseType != null)
            {
                return FindGenericBaseTemplateParameter(type.BaseType, genericBase);
            }
            else
            {
                return null;
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            AbilitySheet abilitySheet = target as AbilitySheet;

            if (abilitySheet.prefab)
            {
                var abilityBase = abilitySheet.prefab.GetComponent<AbilityBase>();

                if (abilityBase == null)
                {
                    ShowError("The provided ability prefab must have a component of type {0} attached to its root!", nameof(AbilityBase));
                }
                else
                {
                    Type abilitySheetType = abilitySheet.GetType();
                    Type abilityExpectedSheetType = FindGenericBaseTemplateParameter(abilityBase.GetType(), typeof(Ability<>));

                    if (abilitySheetType != abilityExpectedSheetType)
                    {
                        if (abilitySheetType.IsSubclassOf(abilityExpectedSheetType))
                        {
                            ShowWarning("{0} is overkill for \"{1}\". Some settings from this sheet may not be used. Consider using a sheet of type {2} instead.",
                                abilitySheetType.Name,
                                abilitySheet.prefab.name,
                                abilityExpectedSheetType.Name);
                        }
                        else
                        {
                            ShowError("\"{0}\" is incompatible with {1}. A ScriptableObject of type {2} is required to work with this prefab.",
                                abilitySheet.prefab.name,
                                abilitySheetType.Name,
                                abilityExpectedSheetType.Name);
                        }
                    }
                }
            }
        }
    }
}
