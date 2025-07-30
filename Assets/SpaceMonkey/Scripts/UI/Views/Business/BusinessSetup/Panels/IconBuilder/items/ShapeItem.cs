using R3;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Business.BusinessSetup.Panels.IconBuilder.items
{
    public class ShapeItem : MonoBehaviour
    {
        [SerializeField] private Color deselectedColor;
        [SerializeField] private Sprite shapeSprite;
        [SerializeField] private Sprite shapeSelectedSprite;
        [SerializeField] private Image shape;
        [SerializeField] private Toggle toggle;

        public Observable<Sprite> SelectedShapeSprite =>
            toggle.OnValueChangedAsObservable().Where(isOn => isOn).Select(_ => shapeSprite);

        private void Start()
        {
            toggle.OnValueChangedAsObservable().Subscribe(state =>
            {
                shape.sprite = state ? shapeSelectedSprite : shapeSprite;
                shape.color = state ? Color.white : deselectedColor;
            }).AddTo(this);
        }

        internal void Set(Sprite sprite)
        {
            shape.sprite = sprite;
        }
    }
}