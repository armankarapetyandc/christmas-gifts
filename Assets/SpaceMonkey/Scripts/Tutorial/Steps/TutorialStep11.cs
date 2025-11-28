using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.UI.Views.BusinessHub;
using SpaceMonkey.Scripts.UI.Views.Product.ProductList;
using UIService.Runtime.Presenter;
using UnityEngine;
using Zenject;

namespace SpaceMonkey.Scripts.Tutorial.Steps
{
    public class TutorialStep11 : ITutorialStep
    {
        private LazyInject<TutorialService> _tutorialService;
        private PresenterService _presenterService;
        public int Order => 11;

        [Inject]
        private void Inject(LazyInject<TutorialService> tutorialService, PresenterService presenterService)
        {
            _presenterService = presenterService;
            _tutorialService = tutorialService;
        }

        public async UniTask Show()
        {
            await _tutorialService.Value.WaitForWindowOpen<ProductListView>();
            var productListView = _presenterService.GetPresenter<ProductListView>();
            var newProductButton = productListView.NewProductButton;
            var rectTransform = (RectTransform) newProductButton.transform;
            _tutorialService.Value.ShowArrow(rectTransform).ShowMask(rectTransform)
                .ShowFunnySlideOut(FunnySlideOutTexts.Step11);
            await _tutorialService.Value.WaitForButtonPress(newProductButton);
            _tutorialService.Value.HideArrow().HideMask();
        }

        public void Hide()
        {
        }
    }
}