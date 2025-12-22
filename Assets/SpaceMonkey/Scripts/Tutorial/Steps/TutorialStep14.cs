using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Asset.Database;
using SpaceMonkey.Scripts.UI.Navigation.Bottom;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using SpaceMonkey.Scripts.UI.Views.Product.NewProduct;
using UIService.Runtime.Presenter;
using UnityEngine;
using Zenject;

namespace SpaceMonkey.Scripts.Tutorial.Steps
{
    public class TutorialStep14 : ITutorialStep
    {
        private LazyInject<TutorialService> _tutorialService;
        private PresenterService _presenterService;
        private AccountService _accountService;
        private VisualAssetDatabase _visualAssetDatabase;
        private NavigationPresenterService _navigationPresenterService;
        public int Order => 14;

        private CompositeDisposable _disposable;

        [Inject]
        private void Inject(LazyInject<TutorialService> tutorialService, PresenterService presenterService,AccountService accountService,VisualAssetDatabase visualAssetDatabase,NavigationPresenterService navigationPresenterService)
        {
            _accountService = accountService;
            _presenterService = presenterService;
            _tutorialService = tutorialService;
            _visualAssetDatabase = visualAssetDatabase;
            _navigationPresenterService = navigationPresenterService;
        }

        public async UniTask Show()
        {
            _disposable?.Dispose();
            _disposable = new CompositeDisposable();
            await _tutorialService.Value.WaitForWindowOpen<ProductView>();
            var productView = _presenterService.GetPresenter<ProductView>();
            var inputField = productView.InputField;
            var rectTransform = (RectTransform) inputField.transform;
            _tutorialService.Value.ShowArrow(rectTransform).ShowMask(rectTransform).ShowFunnySlideOut(FunnySlideOutTexts.Step141);
            await _tutorialService.Value.WaitForInputFieldSubmit(inputField);
            _tutorialService.Value.HideArrow().HideMask().HideFunnySlideOut();
            await UniTask.WaitForEndOfFrame();

            _tutorialService.Value.ShowArrow(productView.ParametersContainer).ShowMask(productView.ParametersContainer)
                .ShowFunnySlideOut(FunnySlideOutTexts.Step142).AddArrowYOffset(560);

            // await UniTask.Delay(TimeSpan.FromSeconds(5));
            await _tutorialService.Value.WaitForObservable(productView.ParametersChanged);
            
            _tutorialService.Value.HideArrow().HideMask().HideFunnySlideOut();
            await UniTask.WaitForEndOfFrame();
            
            var saveButton = productView.SaveButton;
            rectTransform = (RectTransform) saveButton.transform;
            _tutorialService.Value.ShowMask(productView.PropsHolder)
                .ShowFunnySlideOut(FunnySlideOutTexts.Step143);
            var transform = rectTransform;
            productView.AllParametersEdited.Subscribe(_ => _tutorialService.Value.ShowArrow(transform)).AddTo(_disposable);
            await _tutorialService.Value.WaitForObservable(saveButton.OnClickAsObservable());
            _tutorialService.Value.HideArrow().HideMask().HideFunnySlideOut();

            rectTransform = productView.HotItemSubView.NextButtonRect;
            var firstProduct = _accountService.Model.Account.Products.FirstOrDefault();
            var visualIcon = ResolveVisualAsset<SpriteVisualAsset>(firstProduct.IconVisualAssetId);
            var color = ResolveVisualAsset<ColorVisualAsset>(firstProduct.BackgroundColorVisualAssetId);
            productView.HotItemSubView.Initialize(firstProduct.Name, visualIcon, color);
            productView.HotItemSubView.Show();
            productView.HideBaseHolder();
            _tutorialService.Value.ShowArrow(rectTransform).ShowMask(rectTransform);
            await _tutorialService.Value.WaitForObservable(productView.HotItemSubView.NextButton.OnClickAsObservable());
            _tutorialService.Value.HideArrow().HideMask().HideFunnySlideOut();
            await UniTask.WaitForEndOfFrame();
            _navigationPresenterService.Show<MainNavigation>(new MainNavigation.Data
            {
                Type = MainNavigationType.BusinessHub
            }).Forget();
            await UniTask.WaitForEndOfFrame();
            await _accountService.SaveAsync();
        }
        
        private T ResolveVisualAsset<T>(string id) where T : VisualAsset
        {
            return _visualAssetDatabase.GetResourceForAsset<T>(id);
        }

        public void Hide()
        {
            _disposable?.Dispose();
            _disposable = null;
        }
    }
}