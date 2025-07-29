using R3;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Business.BusinessSetup.Panels
{
    public class ShapeItem : MonoBehaviour
    {
        [SerializeField] private Image shape;
        [SerializeField] private Button selected;
        
        public Observable<Sprite> SelectedShapeSprite => selected.OnClickAsObservable().Select(_ => shape.sprite);

        internal void Set(Sprite sprite)
        {
            shape.sprite = sprite;
        }

    }
}