using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.UI.Views.Product.NewProduct;
using SpaceMonkey.Scripts.UI.Views.Product.ProductList;
using UIService.Runtime.Presenter;
using UnityEngine;
using Zenject;

namespace SpaceMonkey.Scripts.Tutorial.Steps
{
    public class TutorialStep12 : ITutorialStep
    {
        private LazyInject<TutorialService> _tutorialService;
        private PresenterService _presenterService;
        public int Order => 12;

        [Inject]
        private void Inject(LazyInject<TutorialService> tutorialService, PresenterService presenterService)
        {
            _presenterService = presenterService;
            _tutorialService = tutorialService;
        }
        
        public async UniTask Show()
        {
            await _tutorialService.Value.WaitForWindowOpen<ProductView>();
            var productView = _presenterService.GetPresenter<ProductView>();
            var iconComponent = productView.IconComponent;
            var rectTransform = (RectTransform)iconComponent.transform;
            _tutorialService.Value.ShowFunnySlideOut(FunnySlideOutTexts.Step12);
            _tutorialService.Value.ShowArrow(rectTransform).ShowMask(rectTransform);
            await _tutorialService.Value.WaitForObservable(iconComponent.OnClick);
            _tutorialService.Value.HideArrow().HideMask();
            
        }

        public void Hide()
        {
        }
    }
}