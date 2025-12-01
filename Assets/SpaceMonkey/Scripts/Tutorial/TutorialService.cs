using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using R3;
using R3.Triggers;
using SpaceMonkey.Scripts.Tutorial.Steps;
using TMPro;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;
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
                Debug.Log($"[{nameof(TutorialService)}] Processing Step: {lastCompleteStep + 1}");
                await ShowStep(lastCompleteStep);
                Debug.Log($"[{nameof(TutorialService)}] Step Completed: {lastCompleteStep + 1}");
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
        
        public TutorialService ShowFunnySlideOut(string description)
        {
            _tutorialView.gameObject.SetActive(true);
            _tutorialView.ShowFunnySlideOut(description);
            return this;
        } 
        
        public TutorialService HideFunnySlideOut()
        {
            _tutorialView.gameObject.SetActive(true);
            _tutorialView.HideFunnySlideOut();
            return this;
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
        
        public TutorialService HideArrow()
        {
            _tutorialView.gameObject.SetActive(true);
            _tutorialView.HideArrow();
            return this;
        }
        
        public TutorialService HideMask()
        {
            _tutorialView.gameObject.SetActive(true);
            _tutorialView.HideMask();
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
        
        public void AddArrowYOffset(float offset)
        {
            _tutorialView.AddArrowYOffset(offset);
        }

        public async UniTask WaitForWindowOpen<T>() where T : BasePresenter
        {
            await UniTask.WaitUntil(() => _presenterService.GetPresenter<T>() != null);
            var presenter = _presenterService.GetPresenter<T>();
            var compilationSource = new UniTaskCompletionSource();
            var disposable = presenter.OnAfterShow.Subscribe(_ => compilationSource.TrySetResult());
            await compilationSource.Task;
            await UniTask.WaitForEndOfFrame();
            disposable.Dispose();
        }
        
        public async UniTask WaitForButtonPress(Button button)
        {
            await button.OnClickAsync();
        }
        
        public async UniTask WaitForObservable(Observable<Unit> unitObservable)
        {
            var compilationSource = new UniTaskCompletionSource();
            var disposable = unitObservable.Subscribe(_ => compilationSource.TrySetResult());
            await compilationSource.Task;
            await UniTask.WaitForEndOfFrame();
            disposable.Dispose();
        }
        
        public async UniTask<T> WaitForObservable<T>(Observable<T> unitObservable)
        {
            var compilationSource = new UniTaskCompletionSource<T>();
            var disposable = unitObservable.Subscribe(value => compilationSource.TrySetResult(value));
            await compilationSource.Task;
            await UniTask.WaitForEndOfFrame();
            disposable.Dispose();
            return await compilationSource.Task;
        }
        
        public async UniTask WaitForInputFieldSelect(TMP_InputField inputField)
        {
            var compilationSource = new UniTaskCompletionSource();
            var disposable = inputField.onEndEdit.AsObservable().AsUnitObservable().Subscribe(_ =>
            {
                if (inputField.text.Length > 0)
                {
                    compilationSource.TrySetResult();
                }
            });
            await compilationSource.Task;
            await UniTask.WaitForEndOfFrame();
            disposable.Dispose();
        }
        
        public async UniTask WaitForInputFieldSubmit(TMP_InputField inputField)
        {
            var compilationSource = new UniTaskCompletionSource();
            var disposable = inputField.onSubmit.AsObservable().AsUnitObservable().Subscribe(_ =>
            {
                if (inputField.text.Length > 5)
                {
                    compilationSource.TrySetResult();
                }
            });
            await compilationSource.Task;
            await UniTask.WaitForEndOfFrame();
            disposable.Dispose();
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