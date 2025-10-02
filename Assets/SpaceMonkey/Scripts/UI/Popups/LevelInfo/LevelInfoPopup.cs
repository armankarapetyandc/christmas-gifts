using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.UI.Popups.Core;
using TMPro;
using UIService.Runtime.Core;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Popups.LevelInfo
{
    public class LevelInfoPopup : PopupPresenterWithController<LevelInfoController>
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private Button okButton;
        [SerializeField] private Slider slider;
        [SerializeField] private TextMeshProUGUI fromLevelText;
        [SerializeField] private TextMeshProUGUI toLevelText;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI scoreText;

        public override UniTask Initialize(IPresenterData data = null)
        {
            closeButton.OnClickAsObservable().Subscribe(_ => { Controller.Close(); }).AddTo(this);
            okButton.OnClickAsObservable().Subscribe(_ => { Controller.Close(); }).AddTo(this);
            Configs.LevelInfo nextLevelInfo = Controller.GetLeveInfoData(Controller.Level + 1);
            slider.value = Controller.Score / (float) nextLevelInfo.Score;
            fromLevelText.text = $"{Controller.Level}";
            toLevelText.text = $"{Controller.Level + 1}";
            levelText.text = $"Level {Controller.Level}";
            scoreText.text = $"Score {Controller.Score}";
            return UniTask.CompletedTask;
        }
    }
}