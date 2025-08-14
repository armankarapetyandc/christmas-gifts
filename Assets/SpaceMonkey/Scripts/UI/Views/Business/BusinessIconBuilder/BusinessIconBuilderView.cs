using R3;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Asset.Database;
using SpaceMonkey.Scripts.UI.Components;
using SpaceMonkey.Scripts.UI.Views.Business.BusinessSetup.Panels.IconBuilder.items;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Business.BusinessIconBuilder
{
    public class BusinessIconBuilderView : BasePresenterWithController<BusinessIconBuilderViewController>
    {
        [SerializeField] private Button backButton;
        [SerializeField] private Button saveButton;
        [SerializeField] private IconComponent iconComponent;
        [SerializeField] private List<ShapeItem> shapeItems;
        [SerializeField] private IconCollectionComponent iconCollectionComponent;
        [SerializeField] private ColorCollectionComponent colorCollectionComponent;

        public override UniTask Initialize(IPresenterData data = null)
        {
            var account = Controller.GetAccount();

            backButton.OnClickAsObservable().Subscribe(_ => Controller.OnBack()).AddTo(this);
            saveButton.OnClickAsObservable().Subscribe(_ => Controller.OnSave()).AddTo(this);
            
            shapeItems.Select(item => item.SelectedShapeSprite).Merge().Subscribe(ShapeSelected).AddTo(this);
            iconCollectionComponent
                .Setup(Controller
                    .ResolveVisualAssets<CategorizedSpriteVisualAsset>(asset =>
                        asset.Category.Equals(account.Company.Category))
                    .ToArray<SpriteVisualAsset>()).Subscribe(IconSelected).AddTo(this);
            colorCollectionComponent.Setup(Controller.ResolveVisualAssets<ColorVisualAsset>().ToArray())
                .Subscribe(ColorSelected).AddTo(this);

            SetupDefaults();

            return UniTask.CompletedTask;
        }

        private void SetupDefaults()
        {
            var account = Controller.GetAccount();

            var shapeVisualAsset =
                Controller.ResolveVisualAsset<SpriteVisualAsset>(account.Company.Logo.ShapeVisualAssetId);
            var iconVisualAsset =
                Controller.ResolveVisualAsset<SpriteVisualAsset>(account.Company.Logo.IconVisualAssetId);
            var colorVisualAsset =
                Controller.ResolveVisualAsset<ColorVisualAsset>(account.Company.Logo.BackgroundColorVisualAssetId);

            iconComponent.SetShape(shapeVisualAsset);
            iconComponent.SetIcon(iconVisualAsset);
            iconComponent.SetColor(colorVisualAsset);
        }

        private void ShapeSelected(SpriteVisualAsset visualAsset)
        {
            Debug.Log($"ShapeSelected:  {visualAsset.Id}");
            iconComponent.SetShape(visualAsset);
            Controller.CompanyLogo.ShapeVisualAssetId = visualAsset.Id;
        }

        private void IconSelected(SpriteVisualAsset visualAsset)
        {
            iconComponent.SetIcon(visualAsset);
            Controller.CompanyLogo.ShapeVisualAssetId = visualAsset.Id;
        }

        private void ColorSelected(ColorVisualAsset visualAsset)
        {
            iconComponent.SetColor(visualAsset);
            Controller.CompanyLogo.ShapeVisualAssetId = visualAsset.Id;
        }

        public override void Dispose()
        {
        }
    }
}