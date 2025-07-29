using System;
using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.UI.Views.Business.BusinessSetup.Panels;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Business.BusinessSetup
{
    public class BusinessSetupView : BasePresenterWithController<BusinessSetupController>
    {
        public class Data : IPresenterData
        {
            public Action BackHandler { get; set; }
        }


        [SerializeField] private Button backButton;
        [SerializeField] private BusinessDetailsPanel detailsPanel;
        [SerializeField] private IconBuilderPanel iconBuilderPanel;

        private Data _data;

        public override UniTask Initialize(IPresenterData data = null)
        {
            _data = data as Data;
            detailsPanel.gameObject.SetActive(true);
            backButton.OnClickAsObservable().Subscribe(_ => OnBackButtonClicked()).AddTo(this);
            detailsPanel.OnIconButtonClicked.Subscribe(_ => OnIconBuilderClicked()).AddTo(this);
            return UniTask.CompletedTask;
        }

        private void OnIconBuilderClicked()
        {
            detailsPanel.gameObject.SetActive(false);
            iconBuilderPanel.gameObject.SetActive(true);
            iconBuilderPanel.Initialize(Controller.IconBuilderConfig);
        }

        private void OnBackButtonClicked()
        {
            _data.BackHandler?.Invoke();
        }

        public override void Dispose()
        {
        }
    }
}