using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.UI.Views.Product.NewProduct;
using SpaceMonkey.Scripts.UI.Views.Product.ProductIconBuilder;
using UIService.Runtime.Presenter;
using UnityEngine;
using Zenject;

namespace SpaceMonkey.Scripts.Tutorial.Steps
{
    public class TutorialStep13 : ITutorialStep
    {
        private LazyInject<TutorialService> _tutorialService;
        private PresenterService _presenterService;
        public int Order => 13;

        [Inject]
        private void Inject(LazyInject<TutorialService> tutorialService, PresenterService presenterService)
        {
            _presenterService = presenterService;
            _tutorialService = tutorialService;
        }
        
        public async UniTask Show()
        {
            await _tutorialService.Value.WaitForWindowOpen<ProductIconBuilderView>();
            var productIconBuilderView = _presenterService.GetPresenter<ProductIconBuilderView>();
            var firstIcon = productIconBuilderView.IconCollectionComponent.FirstItem;
            var rectTransform = (RectTransform)firstIcon.transform;
            _tutorialService.Value.ShowFunnySlideOut(FunnySlideOutTexts.Step13);
            _tutorialService.Value.ShowArrow(rectTransform).ShowMask(rectTransform);
            await _tutorialService.Value.WaitForObservable(firstIcon.OnSelected.AsUnitObservable());
            
            var saveButton = productIconBuilderView.SaveButton;
            rectTransform = (RectTransform)saveButton.transform;
            _tutorialService.Value.ShowArrow(rectTransform).ShowMask(rectTransform);
            await _tutorialService.Value.WaitForObservable(saveButton.OnClickAsObservable());
            _tutorialService.Value.HideArrow().HideMask();
        }

        public void Hide()
        {
        }
    }
}