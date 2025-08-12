using UnityEditor;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceMonkey.Editor.Utilities
{
    [Overlay(typeof(SceneView), "", true)]
    public class CustomToolbar : Overlay
    {
        public override VisualElement CreatePanelContent()
        {
            var root = new VisualElement();

            // Get Canvas icon
            GUIContent canvasIcon = EditorGUIUtility.IconContent("Canvas Icon");

            // Create the button
            var button = new Button(() =>
            {
                var prefab = Resources.Load<GameObject>("UI/ViewPresenterCanvas");
                Object.Instantiate(prefab);
            })
            {
                tooltip = "Instantiate ViewPresenter"
            };

            // Create the image with proper styling
            var iconImage = new Image
            {
                image = canvasIcon.image,
                scaleMode = ScaleMode.ScaleToFit,
            };

            // Set the button size to match Unity’s toolbar buttons
            button.style.width = 26;
            button.style.height = 22;
            button.style.paddingLeft = 0;
            button.style.paddingRight = 0;
            button.style.paddingTop = 0;
            button.style.paddingBottom = 0;
            button.style.marginBottom = 0;

            // Center the icon inside the button
            iconImage.style.flexGrow = 1;
            iconImage.style.alignSelf = Align.Center;

            button.Add(iconImage);
            root.Add(button);

            return root;
        }
    }
}