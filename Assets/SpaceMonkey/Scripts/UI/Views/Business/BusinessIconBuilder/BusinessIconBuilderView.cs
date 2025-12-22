using R3;
using System.Collections.Generic;
using System.Linq;
using AudioPlayer;
using AudioPlayerService.Runtime;
using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Asset.Database;
using SpaceMonkey.Scripts.UI.Components;
using SpaceMonkey.Scripts.Utilities.Validation;
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
        [SerializeField] private TabItem colorTabItem;
        private ReactiveCommand IconSelectedCommand = new ReactiveCommand();
        private ReactiveCommand ColorSelectedCommand = new ReactiveCommand();
        
        public ShapeItem FirstShapeItem => shapeItems.First();
        public IconCollectionComponent IconCollectionComponent => iconCollectionComponent;
        public ColorCollectionComponent ColorCollectionComponent => colorCollectionComponent;
        public Observable<Unit> IconSelectedObservable => IconSelectedCommand;
        public Observable<Unit> ColorSelectedObservable => ColorSelectedCommand;
        public Button SaveButton => saveButton;
        
        public Button BackButton => backButton;
        public TabItem ColorTab => colorTabItem;

        public override UniTask Initialize(IPresenterData data = null)
        {
            var account = Controller.GetAccount();

            backButton.OnClickAsObservable().Subscribe(_ =>
            {
                SfxPlayer.Play(Sounds.Button_Tap);
                Controller.OnBack();
            }).AddTo(this);
            saveButton.OnClickAsObservable().Subscribe(_ =>
            {
                SfxPlayer.Play(Sounds.Button_Tap);
                Controller.OnSave();
            }).AddTo(this);

            shapeItems.Select(item => item.SelectedShapeSprite).Merge().Subscribe(ShapeSelected).AddTo(this);
            iconCollectionComponent
                .Setup(Controller
                    .ResolveVisualAssets<CategorizedSpriteVisualAsset>(asset =>
                        asset.Category.Equals(account.Company.Category))
                    .ToArray<SpriteVisualAsset>()).Subscribe(IconSelected).AddTo(this);
            colorCollectionComponent.Setup(Controller.ResolveVisualAssets<ColorVisualAsset>().ToArray())
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
            var shapeVisualAsset =
                Controller.ResolveVisualAsset<SpriteVisualAsset>(Controller.CompanyLogo.ShapeVisualAssetId);
            var iconVisualAsset =
                Controller.ResolveVisualAsset<SpriteVisualAsset>(Controller.CompanyLogo.IconVisualAssetId);
            var colorVisualAsset =
                Controller.ResolveVisualAsset<ColorVisualAsset>(Controller.CompanyLogo.BackgroundColorVisualAssetId);

            iconComponent.SetShape(shapeVisualAsset);
            // iconComponent.SetIcon(iconVisualAsset);
            // iconComponent.SetColor(colorVisualAsset);
            
            iconCollectionComponent.Select(iconVisualAsset?.Id,true);
            colorCollectionComponent.Select(colorVisualAsset?.Id,true);
        }

        private void ShapeSelected(SpriteVisualAsset visualAsset)
        {
            SfxPlayer.Play(Sounds.Click_Small);
            Debug.Log($"ShapeSelected:  {visualAsset.Id}");
            iconComponent.SetShape(visualAsset);
            Controller.CompanyLogo.ShapeVisualAssetId = visualAsset.Id;
        }

        private void IconSelected(SpriteVisualAsset visualAsset)
        {
            iconComponent.SetIcon(visualAsset);
            Controller.CompanyLogo.IconVisualAssetId = visualAsset.Id;
            IconSelectedCommand.Execute(Unit.Default);
        }

        private void ColorSelected(ColorVisualAsset visualAsset)
        {
            SfxPlayer.Play(Sounds.Click_Small);
            iconComponent.SetColor(visualAsset);
            Controller.CompanyLogo.BackgroundColorVisualAssetId = visualAsset.Id;
            ColorSelectedCommand.Execute(Unit.Default);
        }

        public override void Dispose()
        {
        }
    }
}