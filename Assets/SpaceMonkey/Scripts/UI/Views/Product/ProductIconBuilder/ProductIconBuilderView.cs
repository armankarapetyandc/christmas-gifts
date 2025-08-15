using System.Linq;
using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.UI.Asset.Database;
using SpaceMonkey.Scripts.UI.Components;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;
using R3;
using SpaceMonkey.Scripts.Utilities.Validation;

namespace SpaceMonkey.Scripts.UI.Views.Product.ProductIconBuilder
{
    public class ProductIconBuilderView : BasePresenterWithController<ProductIconBuilderController>
    {
        public class Data : IPresenterData
        {
            public Profile.Product Product;
        }

        [SerializeField] private Button saveButton;
        [SerializeField] private Button backButton;
        [SerializeField] private IconComponent iconComponent;
        [SerializeField] private IconCollectionComponent iconCollectionComponent;
        [SerializeField] private ColorCollectionComponent colorCollectionComponent;
        [SerializeField] private ColorVisualAsset defaultColorVisualAsset;

        private Data _data;

        public override UniTask Initialize(IPresenterData data = null)
        {
            _data = data as Data;
            var account = Controller.GetAccount();

            backButton.OnClickAsObservable().Subscribe(_ => Controller.ReturnProductView(_data!.Product))
                .AddTo(this);
            saveButton.OnClickAsObservable().Subscribe(_ => Controller.ReturnProductView(_data!.Product)).AddTo(this);

            iconCollectionComponent
                .Setup(Controller
                    .ResolveVisualAssets<CategorizedSpriteVisualAsset>(asset =>
                        asset.Category.Equals(account.Company.Category) && asset.Type.HasFlag(VisualAssetType.Product))
                    .ToArray<SpriteVisualAsset>()).Subscribe(IconSelected).AddTo(this);
            colorCollectionComponent.Setup(Controller
                    .ResolveVisualAssets<ColorVisualAsset>(asset => asset.Type.HasFlag(VisualAssetType.Product))
                    .ToArray())
                .Subscribe(ColorSelected).AddTo(this);

            SetupDefaults();

            Validator
                .Validate(iconComponent.Fulfilled)
                .BindButton(saveButton)
                .AddTo(this);

            return UniTask.CompletedTask;
        }

        private void SetupDefaults()
        {
            var iconVisualAsset =
                Controller.ResolveVisualAsset<SpriteVisualAsset>(_data.Product.IconVisualAssetId);
            var colorVisualAsset =
                Controller.ResolveVisualAsset<ColorVisualAsset>(_data.Product.BackgroundColorVisualAssetId) ??
                defaultColorVisualAsset;

            iconComponent.SetIcon(iconVisualAsset);
            iconComponent.SetColor(colorVisualAsset);

            iconCollectionComponent.Select(iconVisualAsset?.Id);
            colorCollectionComponent.Select(colorVisualAsset?.Id);
        }

        private void IconSelected(SpriteVisualAsset visualAsset)
        {
            iconComponent.SetIcon(visualAsset);
            _data.Product.IconVisualAssetId = visualAsset.Id;
        }

        private void ColorSelected(ColorVisualAsset visualAsset)
        {
            iconComponent.SetColor(visualAsset);
            _data.Product.BackgroundColorVisualAssetId = visualAsset.Id;
        }

        public override void Dispose()
        {
        }
    }
}