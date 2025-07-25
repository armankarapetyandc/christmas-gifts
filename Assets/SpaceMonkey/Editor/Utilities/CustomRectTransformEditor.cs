using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SpaceMonkey.Editor.Utilities
{
    [CustomEditor(typeof(RectTransform))]
    [CanEditMultipleObjects]
    public class CustomRectTransformEditor : UnityEditor.Editor
    {
        private UnityEditor.Editor defaultEditor;
        private RectTransform rectTransform;

        void OnEnable()
        {
            // Get the internal RectTransformEditor type using reflection
            Type rectTransformEditorType =
                typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.RectTransformEditor");

            // Create the default inspector using the internal type
            defaultEditor = CreateEditor(targets, rectTransformEditorType);
            rectTransform = target as RectTransform;
        }

        void OnDisable()
        {
            // Clean up the default editor
            if (defaultEditor != null)
                DestroyImmediate(defaultEditor);
        }

        public override void OnInspectorGUI()
        {
            // Draw the default RectTransform inspector
            if (defaultEditor != null)
                defaultEditor.OnInspectorGUI();

            // Add spacing
            EditorGUILayout.Space(10);

            // Draw custom fields
            DrawCanvasPositionFields();
        }

        private void DrawCanvasPositionFields()
        {
            if (rectTransform == null) return;

            // Find the Canvas
            Canvas canvas = FindLastCanvas(rectTransform);
            if (canvas == null)
            {
                EditorGUILayout.HelpBox("No Canvas found in parent hierarchy", MessageType.Warning);
                return;
            }

            EditorGUILayout.LabelField("Canvas Position", EditorStyles.boldLabel);

            // Calculate current canvas position
            Vector2 canvasPosition = GetCanvasPosition(rectTransform, canvas);

            EditorGUI.BeginChangeCheck();

            // Create position fields
            Vector2 newCanvasPosition = EditorGUILayout.Vector2Field("Position from Canvas", canvasPosition);

            if (EditorGUI.EndChangeCheck())
            {
                // Record undo
                Undo.RecordObject(rectTransform, "Change Canvas Position");

                // Set the new canvas position
                SetCanvasPosition(rectTransform, canvas, newCanvasPosition);
            }

            // Show some helpful info
            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField($"Canvas: {canvas.name}", EditorStyles.miniLabel);
            EditorGUILayout.LabelField($"Canvas Size: {GetCanvasSize(canvas)}", EditorStyles.miniLabel);
        }

        private Canvas GetCanvasFromRectTransform(RectTransform rt)
        {
            Transform current = rt.transform;
            while (current != null)
            {
                Canvas canvas = current.GetComponent<Canvas>();
                if (canvas != null)
                    return canvas;
                current = current.parent;
            }

            return null;
        }
        
        private Canvas FindLastCanvas(RectTransform rt)
        {
            Transform current = rt.transform;
            Canvas lastCanvas = null;
    
            while (current != null)
            {
                Canvas canvas = current.GetComponent<Canvas>();
                if (canvas != null)
                    lastCanvas = canvas; // Keep updating to get the topmost one
            
                current = current.parent;
            }
    
            return lastCanvas;
        }
        
        private Canvas GetMainCanvasFromRectTransform(RectTransform rt)
        {
            Transform current = rt.transform;
            while (current != null)
            {
                Canvas canvas = current.GetComponent<Canvas>();
                if (canvas != null)
                    return canvas;
                current = current.parent;
            }

            return null;
        }

        private Vector2 GetCanvasPosition(RectTransform rt, Canvas canvas)
        {
            // Get the canvas RectTransform
            RectTransform canvasRT = canvas.GetComponent<RectTransform>();

            // Convert world position to canvas local position
            Vector3 worldPos = rt.position;
            Vector3 canvasLocalPos = canvasRT.InverseTransformPoint(worldPos);

            // Convert from centered coordinates to left-top origin coordinates
            Vector2 canvasSize = canvasRT.sizeDelta;
            float canvasX = canvasLocalPos.x + (canvasSize.x * 0.5f); // Convert from center to left origin
            float canvasY = (canvasSize.y * 0.5f) - canvasLocalPos.y; // Convert from center to top origin (flip Y)

            return new Vector2(canvasX, canvasY);
        }

        private void SetCanvasPosition(RectTransform rt, Canvas canvas, Vector2 canvasPosition)
        {
            RectTransform canvasRT = canvas.GetComponent<RectTransform>();
            Vector2 canvasSize = canvasRT.sizeDelta;

            // Convert from left-top origin coordinates to centered coordinates
            float centeredX = canvasPosition.x - (canvasSize.x * 0.5f); // Convert from left origin to center
            float centeredY = (canvasSize.y * 0.5f) - canvasPosition.y; // Convert from top origin to center (flip Y)

            // Convert canvas local position to world position
            Vector3 worldPos = canvasRT.TransformPoint(new Vector3(centeredX, centeredY, 0));

            // Set the world position (this will automatically update anchored position)
            rt.position = worldPos;
        }

        private Vector2 GetCanvasSize(Canvas canvas)
        {
            RectTransform canvasRT = canvas.GetComponent<RectTransform>();
            return canvasRT.sizeDelta;
        }
    }
}