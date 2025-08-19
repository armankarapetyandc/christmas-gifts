using SpaceMonkey.Scripts.UI.Components;
using UnityEditor;
using UnityEditor.UI;

namespace SpaceMonkey.Editor.Utilities
{
    [CustomEditor(typeof(EnhancedButton))]
    public class EnhancedButtonEditor : ButtonEditor
    {
        SerializedProperty enableTextColorTransitionProp;
        SerializedProperty textColorsProp;
        SerializedProperty targetTMPProp;

        protected override void OnEnable()
        {
            base.OnEnable();
            enableTextColorTransitionProp = serializedObject.FindProperty("enableTextColorTransition");
            textColorsProp = serializedObject.FindProperty("textColors");
            targetTMPProp = serializedObject.FindProperty("targetTMP");
        }

        public override void OnInspectorGUI()
        {
            // Draw the original Button inspector
            base.OnInspectorGUI();

            // Draw a separator for clarity
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Enhanced Button Settings", EditorStyles.boldLabel);

            serializedObject.Update();

            EditorGUILayout.PropertyField(enableTextColorTransitionProp);
            if (enableTextColorTransitionProp.boolValue)
            {
                EditorGUILayout.PropertyField(textColorsProp);
            }

            EditorGUILayout.PropertyField(targetTMPProp);

            serializedObject.ApplyModifiedProperties();
        }
    }
}