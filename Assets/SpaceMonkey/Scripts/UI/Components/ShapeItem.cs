using R3;
using SpaceMonkey.Scripts.UI.Asset.Database;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Components
{
    public class ShapeItem : MonoBehaviour
    {
        [SerializeField] private Color deselectedColor;
        [SerializeField] private SpriteVisualAsset shapeVisualAsset;
        [SerializeField] private Sprite shapeSelectedSprite;
        [SerializeField] private Image shape;
        [SerializeField] private Toggle toggle;

        public Observable<SpriteVisualAsset> SelectedShapeSprite =>
            toggle.OnValueChangedAsObservable().Where(isOn => isOn).Select(_ => shapeVisualAsset);

        private void Start()
        {
            toggle.OnValueChangedAsObservable().Subscribe(state =>
            {
                shape.sprite = state ? shapeSelectedSprite : shapeVisualAsset.Sprite;
                shape.color = state ? Color.white : deselectedColor;
            }).AddTo(this);
        }
    }
}