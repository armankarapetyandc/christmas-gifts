using System.Linq;
using AudioPlayer;
using AudioPlayerService.Runtime;
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
        [SerializeField] private RectTransform tabsContainer;
        [SerializeField] private IconCollectionComponent iconCollectionComponent;
        [SerializeField] private ColorCollectionComponent colorCollectionComponent;

        private Data _data;
        public IconCollectionComponent IconCollectionComponent => iconCollectionComponent;
        public Button SaveButton => saveButton;

        public RectTransform TabsContainer => tabsContainer;

        private readonly Subject<Unit> _anyIconSelectedSubject = new Subject<Unit>();

        public Observable<Unit> AnyIconSelectedObservable => _anyIconSelectedSubject;

        public override UniTask Initialize(IPresenterData data = null)
        {
            _data = data as Data;
            var account = Controller.GetAccount();

            backButton.OnClickAsObservable().Subscribe(_ =>
                {
                    SfxPlayer.Play(Sounds.sfx_ClickSmall);
                    Controller.ReturnProductView(_data!.Product);
                })
                .AddTo(this);
            saveButton.OnClickAsObservable().Subscribe(_ =>
                {
                    SfxPlayer.Play(Sounds.Button_Tap);
                    Controller.ReturnProductView(Controller.Product);
                })
                .AddTo(this);

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
            Controller.Product = _data.Product;

            var iconVisualAsset =
                Controller.ResolveVisualAsset<SpriteVisualAsset>(Controller.Product.IconVisualAssetId);
            var colorVisualAsset =
                Controller.ResolveVisualAsset<ColorVisualAsset>(Controller.Product.BackgroundColorVisualAssetId) ??
                Controller.GetDefaultColorVisualAssetInSequence();

            iconCollectionComponent.Select(iconVisualAsset?.Id, true);
            colorCollectionComponent.Select(colorVisualAsset?.Id, true);
        }

        private void IconSelected(SpriteVisualAsset visualAsset)
        {
            SfxPlayer.Play(Sounds.Click_Small);
            iconComponent.SetIcon(visualAsset);
            Controller.Product.IconVisualAssetId = visualAsset.Id;
            _anyIconSelectedSubject?.OnNext(Unit.Default);
        }

        private void ColorSelected(ColorVisualAsset visualAsset)
        {
            SfxPlayer.Play(Sounds.Click_Small);
            iconComponent.SetColor(visualAsset);
            Controller.Product.BackgroundColorVisualAssetId = visualAsset.Id;
        }

        public override void Dispose()
        {
        }
    }
}