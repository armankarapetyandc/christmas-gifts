using Cysharp.Threading.Tasks;
using R3;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.BusinessLocationSplash
{
    public class BusinessLocationSplashView : BasePresenterWithController<BusinessLocationSplashViewController>
    {
        [SerializeField] private Button nextButton;
        private Data _data;
        public override UniTask Initialize(IPresenterData data = null)
        {
            _data = data as Data;
            nextButton.OnClickAsObservable().Subscribe(_ => Controller.OnNext(_data.PlaceId).Forget()).AddTo(this);
            return UniTask.CompletedTask;
        }

        public override void Dispose()
        {
            
        }
        
        public class Data : IPresenterData
        {
            public int PlaceId;
        }
    }
}