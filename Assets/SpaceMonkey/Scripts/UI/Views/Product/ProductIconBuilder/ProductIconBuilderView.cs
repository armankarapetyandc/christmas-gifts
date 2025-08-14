using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.UI.Asset.Database;
using SpaceMonkey.Scripts.UI.Components;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;
using R3;

namespace SpaceMonkey.Scripts.UI.Views.Product.ProductIconBuilder
{
    public class ProductIconBuilderView : BasePresenterWithController<ProductIconBuilderController>
    {
        public class Data : IPresenterData
        {
            public ColorVisualAsset Color { get; set; }
            public SpriteVisualAsset Icon { get; set; }
        }

        [SerializeField] private Button saveButton;
        [SerializeField] private IconComponent iconComponent;
        [SerializeField] private IconCollectionComponent iconCollectionComponent;
        [SerializeField] private ColorCollectionComponent colorCollectionComponent;
        [SerializeField] private ColorVisualAsset defaultColorVisualAsset;

        public override UniTask Initialize(IPresenterData data = null)
        {
            var presenterData = data as Data;
            iconComponent.SetColor(presenterData?.Color ?? defaultColorVisualAsset);
            iconComponent.SetIcon(presenterData?.Icon);
            // iconCollectionComponent.Setup().Subscribe(OnIconSpriteAssetSelected).AddTo(this);
            // colorCollectionComponent.Setup().Subscribe(OnColorSpriteAssetSelected).AddTo(this);
            return UniTask.CompletedTask;
        }

        private void OnIconSpriteAssetSelected(SpriteVisualAsset asset)
        {
            iconComponent.SetIcon(asset);
        }

        private void OnColorSpriteAssetSelected(ColorVisualAsset asset)
        {
            iconComponent.SetColor(asset);
        }

        public override void Dispose()
        {
        }
    }
}