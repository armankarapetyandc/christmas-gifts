#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Utility.Editor
{
    [CustomEditor(typeof(CanvasGapRaycast))]
    [CanEditMultipleObjects]
    public class CanvasGapRaycastEditor : ImageEditor
    {
        SerializedProperty holeShapeProp;
        SerializedProperty holeRectSizeProp;
        SerializedProperty holeRadiusProp;
        SerializedProperty holeCenterProp;
        SerializedProperty alignToTransformProp;

        protected override void OnEnable()
        {
            base.OnEnable(); // This initializes properties for the Image fields

            // Fetch the HoleRaycastBlocker properties
            holeShapeProp = serializedObject.FindProperty("holeShape");
            holeRectSizeProp = serializedObject.FindProperty("holeRectSize");
            holeRadiusProp = serializedObject.FindProperty("holeRadius");
            holeCenterProp = serializedObject.FindProperty("holeCenter");
            alignToTransformProp = serializedObject.FindProperty("alignToTransform");
        }

        public override void OnInspectorGUI()
        {
            // Draw Image fields (Source Image, Color, etc.)
            base.OnInspectorGUI();

            serializedObject.Update();
            
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Hole Settings", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(holeShapeProp);

            var shape = (CanvasGapRaycast.HoleShape) holeShapeProp.enumValueIndex;
            switch (shape)
            {
                case CanvasGapRaycast.HoleShape.Rectangle:
                    EditorGUILayout.PropertyField(holeRectSizeProp, new GUIContent("Hole Rectangle Size"));
                    break;
                case CanvasGapRaycast.HoleShape.Circle:
                    EditorGUILayout.PropertyField(holeRadiusProp, new GUIContent("Hole Radius"));
                    break;
            }

            EditorGUILayout.PropertyField(holeCenterProp, new GUIContent("Hole Center"));

            EditorGUILayout.PropertyField(alignToTransformProp, new GUIContent("Align To Transform"));

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif