using R3;
using SpaceMonkey.Scripts.UI.Asset.Database;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Components
{
    public class IconComponent : MonoBehaviour
    {
        [SerializeField] private RectTransform plusRectTransform;
        [SerializeField] private Image iconImage;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Button button;
        public SpriteVisualAsset SpriteVisualAsset { get; private set; }
        public ColorVisualAsset ColorVisualAsset { get; private set; }

        public Observable<Unit> OnClick => button != null ? button.OnClickAsObservable() : Observable.Empty<Unit>();

        public void SetColor(ColorVisualAsset asset)
        {
            ColorVisualAsset = asset;
            if (ColorVisualAsset != null)
            {
                backgroundImage.color = asset.Color;
            }
        }

        public void SetIcon(SpriteVisualAsset asset)
        {
            SpriteVisualAsset = asset;
            iconImage.sprite = asset.Sprite;
            iconImage.gameObject.SetActive(asset != null);
            if (plusRectTransform != null)
            {
                plusRectTransform.gameObject.SetActive(asset != null);
            }
        }
    }
}