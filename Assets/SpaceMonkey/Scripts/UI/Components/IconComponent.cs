using System;
using R3;
using SpaceMonkey.Scripts.UI.Asset.Database;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Components
{
    public class IconComponent : MonoBehaviour
    {
        [Flags]
        public enum Requirements
        {
            None = 0,
            Shape = 1 << 0, // 1
            Icon = 1 << 1, // 2
            Color = 1 << 2, // 4
            All = Shape | Icon | Color
        }

        [SerializeField] private RectTransform plusRectTransform;
        [SerializeField] private Image shapeImage;
        [SerializeField] private Image iconImage;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Requirements requirements;
        [SerializeField] private Button button;
        public SpriteVisualAsset SpriteVisualAsset { get; private set; }
        public SpriteVisualAsset ShapeVisualAsset { get; private set; }
        public ColorVisualAsset ColorVisualAsset { get; private set; }

        public Observable<Unit> OnClick => button != null ? button.OnClickAsObservable() : Observable.Empty<Unit>();

        public Observable<bool> Fulfilled =>
            Observable.EveryUpdate()
                .Select(_ =>
                {
                    var current = Requirements.None;

                    if (ShapeVisualAsset != null)
                        current |= Requirements.Shape;

                    if (SpriteVisualAsset != null)
                        current |= Requirements.Icon;

                    if (ColorVisualAsset != null)
                        current |= Requirements.Color;
                    
                    var required = requirements & Requirements.All;
                    return (current & required) == required;
                })
                .DistinctUntilChanged();

        public void SetColor(ColorVisualAsset asset)
        {
            if (asset != null)
            {
                ColorVisualAsset = asset;
                backgroundImage.color = asset.Color;
            }
        }

        public void SetIcon(SpriteVisualAsset asset)
        {
            if (asset != null)
            {
                SpriteVisualAsset = asset;
                iconImage.sprite = asset.Sprite;
            }

            iconImage.gameObject.SetActive(asset != null);
            if (plusRectTransform != null)
            {
                plusRectTransform.gameObject.SetActive(asset == null);
            }
        }

        public void SetShape(SpriteVisualAsset asset)
        {
            if (asset != null)
            {
                ShapeVisualAsset = asset;
                shapeImage.sprite = asset.Sprite;
            }

            shapeImage.gameObject.SetActive(asset != null);
        }
    }
}