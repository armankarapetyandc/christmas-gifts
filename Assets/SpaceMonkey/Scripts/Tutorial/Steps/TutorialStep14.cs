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
            var rectTransform = (RectTransform)inputField.transform;
            _tutorialService.Value.ShowArrow(rectTransform).ShowMask(rectTransform);
            await _tutorialService.Value.WaitForInputFieldSelect(inputField);
            _tutorialService.Value.HideArrow().HideMask();
            
            var saveButton = productView.SaveButton;
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