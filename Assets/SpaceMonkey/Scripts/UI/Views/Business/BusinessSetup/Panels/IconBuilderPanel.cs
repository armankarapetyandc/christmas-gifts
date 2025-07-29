using System.Collections.Generic;
using R3;
using SpaceMonkey.Scripts.UI.Asset.IconBuilder;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Business.BusinessSetup.Panels
{
    public class IconBuilderPanel : MonoBehaviour
    {
        [SerializeField] private Button backButton;
        [SerializeField] private Button saveButton;

        [SerializeField] private Image selectedShape;
        [SerializeField] private Image selectedIcon;
        
        [SerializeField] private List<ShapeItem> shapesSprites;
        [SerializeField] private IconBuilderTab iconBuilderTab;
        // [SerializeField] private List<IconItem> iconItems;
        // [SerializeField] private List<BackgroundItem> backgroundImages;
        
        // [SerializeField] private Sprite selectedButtonBackground;
        // [SerializeField] private Sprite noneSelectedButtonBackground;

        internal void Initialize(IconBuilderConfig iconBuilderConfig)
        {
            if (iconBuilderConfig == null)
            {
                Debug.LogError("IconBuilderConfig is null");
                return;
            }
            
            for (var i = 0; i < shapesSprites.Count; ++i)
            {
                shapesSprites[i].Set(iconBuilderConfig.ShapesSprites[i]);
                shapesSprites[i].SelectedShapeSprite.Subscribe(UpdateSelectedShape);
            }

            foreach (var icon in iconBuilderTab.IconItems)
            {
                icon.OnSelectedSprite.Subscribe(UpdateSelectedIcon);
            }

            foreach (var background in iconBuilderTab.BackgroundItems)
            {
                background.OnSelectedColor.Subscribe(UpdateSelectedBackground);
            }
            
            iconBuilderTab.Initialize(iconBuilderConfig.IconSprites, iconBuilderConfig.BackgroundColors);
        }

        private void UpdateSelectedShape(Sprite shape)
        {
            selectedShape.gameObject.SetActive(true);
            selectedShape.sprite = shape;
        }
        
        private void UpdateSelectedIcon(Sprite shape)
        {
            selectedIcon.gameObject.SetActive(true);
            selectedIcon.sprite = shape;
        }
        
        private void UpdateSelectedBackground(Color color)
        {
            selectedShape.color = color;
        }
    }
}