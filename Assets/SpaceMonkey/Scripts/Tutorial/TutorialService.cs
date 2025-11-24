using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Tutorial.Steps;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using Zenject;

namespace SpaceMonkey.Scripts.Tutorial
{
    public class TutorialService : IInitializable
    {
        private List<ITutorialStep> _steps;
        private TutorialView _tutorialView;
        private PresenterService _presenterService;

        public TutorialView tutorialView => _tutorialView;

        [Inject]
        private void Inject(List<ITutorialStep> steps, TutorialView tutorialView, PresenterService presenterService)
        {
            _presenterService = presenterService;
            _tutorialView = tutorialView;
            _steps = steps.OrderBy(s => s.Order).ToList();
        }
        
        public void Initialize()
        {
            ProcessStepsTick().Forget();
        }
        
        public async UniTask ProcessStepsTick()
        {
            var lastCompleteStep = GetLastCompleteStep();
            var lastStep = _steps.Last().Order;
            HideTutorialView();
            
            while (lastCompleteStep < lastStep)
            {
                await ShowStep(lastCompleteStep);
                HideStep(lastCompleteStep);
                SetLastCompleteStep(lastCompleteStep);
                lastCompleteStep++;
            }
        }

        private async UniTask<bool> ShowStep(int lastCompleteStep)
        {
            var step = _steps.FirstOrDefault(s => s.Order == lastCompleteStep + 1);
            if (step == null)
            {
                return false;
            }
            await step.Show();
            return true;
        }
        
        private void HideStep(int lastCompleteStep)
        {
            var step = _steps.FirstOrDefault(s => s.Order == lastCompleteStep + 1);
            step?.Hide();
            HideTutorialView();
        }

        public TutorialService ShowArrow(RectTransform rectTransform)
        {
            _tutorialView.gameObject.SetActive(true);
            _tutorialView.ShowArrow(rectTransform);
            return this;
        }
        
        public TutorialService ShowMask(RectTransform rectTransform)
        {
            _tutorialView.gameObject.SetActive(true);
            _tutorialView.ShowMask(rectTransform);
            return this;
        }
        
        public TutorialService ShowTutorialView()
        {
            _tutorialView.gameObject.SetActive(true);
            return this;
        }
        
        public void HideTutorialView()
        {
            _tutorialView.Reset();
            _tutorialView.gameObject.SetActive(false);
        }

        public async UniTask WaitForWindowOpen<T>() where T : BasePresenter
        {
            await UniTask.WaitUntil(() => _presenterService.GetPresenter<T>() != null);
        }
        
        public async UniTask WaitForWindowClose<T>() where T : BasePresenter
        {
            await UniTask.WaitUntil(() => _presenterService.GetPresenter<T>() == null);
        }
        
        private void SetLastCompleteStep(int step)
        {
            PlayerPrefs.SetInt("lastCompleteStep", step);
            PlayerPrefs.Save();
        }

        private int GetLastCompleteStep()
        {
            return PlayerPrefs.GetInt("lastCompleteStep", 0);
        }
    }
}