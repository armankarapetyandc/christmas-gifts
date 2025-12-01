using Cysharp.Threading.Tasks;
using R3;
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
        public int Order => 14;

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
            var inputField = productView.InputField;
            var rectTransform = (RectTransform) inputField.transform;
            _tutorialService.Value.ShowArrow(rectTransform).ShowMask(rectTransform).ShowFunnySlideOut(FunnySlideOutTexts.Step141);
            await _tutorialService.Value.WaitForInputFieldSubmit(inputField);
            _tutorialService.Value.HideArrow().HideMask().HideFunnySlideOut();

            _tutorialService.Value.ShowArrow(productView.ParametersContainer).ShowMask(productView.ParametersContainer)
                .ShowFunnySlideOut(FunnySlideOutTexts.Step142).AddArrowYOffset(20);
            for (int i = 0; i < 3; i++)
            {
                await _tutorialService.Value.WaitForObservable(productView.ParametersChanged);
            }
            
            var saveButton = productView.SaveButton;
            rectTransform = (RectTransform) saveButton.transform;
            _tutorialService.Value.ShowArrow(rectTransform).ShowMask(rectTransform).ShowFunnySlideOut(FunnySlideOutTexts.Step143);
            await _tutorialService.Value.WaitForObservable(saveButton.OnClickAsObservable());
            _tutorialService.Value.HideArrow().HideMask();
        }

        public void Hide()
        {
        }
    }
}