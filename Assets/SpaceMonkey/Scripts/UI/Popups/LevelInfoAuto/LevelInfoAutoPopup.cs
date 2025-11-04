using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.UI.Asset.Database;
using SpaceMonkey.Scripts.UI.Components;
using SpaceMonkey.Scripts.UI.Popups.Core;
using TMPro;
using UIService.Runtime.Core;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Popups.LevelInfoAuto
{
    public class LevelInfoAutoPopup : PopupPresenterWithController<LevelInfoAutoController>
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private Button okButton;
        [SerializeField] private Slider slider;
        [SerializeField] private TextMeshProUGUI fromLevelText;
        [SerializeField] private TextMeshProUGUI toLevelText;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private Image sliderFillImage;
        [SerializeField] private UnlockedItem[] unlockedItems;
 
        
        [SerializeField]  private Color startColor;
        [SerializeField]  private Color endColor;
        
        private CancellationTokenSource cts;

        public override UniTask Initialize(IPresenterData data = null)
        {
            cts = new CancellationTokenSource();
            closeButton.OnClickAsObservable().Subscribe(_ => { Controller.Close(); }).AddTo(this);
            okButton.OnClickAsObservable().Subscribe(_ => { Controller.Close(); }).AddTo(this);
            Configs.LevelInfo nextLevelInfo = Controller.GetLeveInfoData(Controller.Level + 1);
            slider.value = Controller.Score / (float) nextLevelInfo.Score;
            fromLevelText.text = $"{Controller.Level-1}";
            toLevelText.text = $"{Controller.Level}";
            levelText.text = $"Level {Controller.Level-1}";
            scoreText.text = $"Score {Controller.Score}";
            foreach (var unlockedItem in unlockedItems)
            {
                unlockedItem.gameObject.SetActive(false);
            }
            StartFill(cts.Token).Forget();
            return UniTask.CompletedTask;
        }
        
        private async UniTaskVoid StartFill(CancellationToken token)
        {
            slider.value = 0;
            float elapsed = 0f;
            float duration = 2;
            while (elapsed < duration)
            {
                await UniTask.Yield(PlayerLoopTiming.Update, token);
                elapsed += Time.deltaTime;
                float progress = Mathf.Clamp01(elapsed / duration);
                slider.value = Mathf.Lerp(0, slider.maxValue, progress);
                sliderFillImage.color = Color.Lerp(startColor, endColor, progress);
            }

            slider.value = slider.maxValue;
            await UniTask.Delay(TimeSpan.FromSeconds(0.1), cancellationToken: token);
            OnFillComplete();

        }
        
        private void OnFillComplete()
        {
            levelText.text = $"Level {Controller.Level}";
            fromLevelText.text = $"{Controller.Level}";
            toLevelText.text = $"{Controller.Level + 1}";
            slider.value = 0.1f;
            sliderFillImage.color = startColor;
            Configs.LevelInfo nextLevelInfo = Controller.GetLeveInfoData(Controller.Level);
            for (int i = 0; i < nextLevelInfo.UnlockInfo.Count; i++)
            {
                unlockedItems[i].Initialize(nextLevelInfo.UnlockInfo[i]);
            }
        }
    }
}