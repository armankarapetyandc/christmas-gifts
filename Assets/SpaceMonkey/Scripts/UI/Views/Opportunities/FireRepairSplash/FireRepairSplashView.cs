using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.Profile;
using TMPro;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.FireRepairSplash
{
    public class FireRepairSplashView : BasePresenterWithController<FireRepairSplashViewController>
    {
        [SerializeField] private Button nextButton;
        [SerializeField] private TextMeshProUGUI levelText;
        private Data _data;
        public override UniTask Initialize(IPresenterData data = null)
        {
            _data = (Data)data;
            levelText.text = $"{_data.Level.LevelNumber+1}";
            nextButton.OnClickAsObservable().Subscribe(_ => Controller.OnNext()).AddTo(this);
            return UniTask.CompletedTask;
        }

        public override void Dispose()
        {
        }
        
        public class Data : IPresenterData
        {
            public LevelProdCap Level { get; set; }
        }
    }
}
