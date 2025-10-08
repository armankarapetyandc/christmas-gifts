using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using TMPro;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.UpgradeCapacity
{
    public class WeekEndRewardView : BasePresenterWithController<WeekEndRewardController>
    {
        [SerializeField] private TextMeshProUGUI pointsText;
        [SerializeField] private Slider slider;
        [SerializeField] private TextMeshProUGUI fromLevelText;
        [SerializeField] private TextMeshProUGUI toLevelText;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private Button nextButton;
        
        
        private CancellationTokenSource cts;
        
        public override UniTask Initialize(IPresenterData data = null)
        {
            cts = new CancellationTokenSource();
            Configs.LevelInfo nextLevelInfo = Controller.GetLeveInfoData(Controller.Level + 1);
            slider.value = Controller.Score / (float) nextLevelInfo.Score;
            fromLevelText.text = $"{Controller.Level}";
            toLevelText.text = $"{Controller.Level + 1}";
            levelText.text = $"Level {Controller.Level}";
            scoreText.text = $"Score {Controller.Score}";
            pointsText.text = $"+ {Controller.SellScore}";
            StartFill(cts.Token).Forget();
            nextButton.OnClickAsObservable().Subscribe(_ => Controller.OnNext()).AddTo(this);
            return UniTask.CompletedTask;
        }
        
        private async UniTaskVoid StartFill(CancellationToken token)
        {
            float nextLevelScore = Controller.GetLevelInfoByLevel(Controller.Level+1).Score;
            slider.value = (Controller.Score-Controller.SellScore)/nextLevelScore;
            float elapsed = 0f;
            float duration = 2;
            while (elapsed < duration)
            {
                await UniTask.Yield(PlayerLoopTiming.Update, token);
                elapsed += Time.deltaTime;
                float progress = Mathf.Clamp01(elapsed / duration);
                slider.value = Mathf.Lerp((Controller.Score-Controller.SellScore)/nextLevelScore, Controller.Score/nextLevelScore, progress);
            }

            await UniTask.Delay(TimeSpan.FromSeconds(0.1), cancellationToken: token);
            OnFillComplete();

        }
        
        private void OnFillComplete()
        {
            var beforeLevel = Controller.GetLevel(Controller.Score - Controller.SellScore);
            var afterLevel = Controller.GetLevel(Controller.Score);
            if (beforeLevel< afterLevel)
            {
                levelText.text = $"Level {afterLevel}";
                fromLevelText.text = $"{afterLevel}";
                toLevelText.text = $"{afterLevel + 1}";
            }
        }

        public override void Dispose()
        {
        }
    }
}