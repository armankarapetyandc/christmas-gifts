using System;
using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Views.Business.BusinessSetup.Panels;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Business.BusinessSetup
{
    public class BusinessSetupView : BasePresenterWithController<BusinessSetupController>
    {
        [SerializeField] private Button backButton;
        [SerializeField] private BusinessDetailsPanel detailsPanel;
        [SerializeField] private IconBuilderPanel iconBuilderPanel;
        [SerializeField] private Button saveButton;

        public override UniTask Initialize(IPresenterData data = null)
        {
            backButton.OnClickAsObservable().Subscribe(_ => Controller.OnBack()).AddTo(this);
            saveButton.OnClickAsObservable().Subscribe(_ => Controller.OnNext()).AddTo(this);
            detailsPanel.OnIconButtonClicked.Subscribe(_ => NavigateToIconBuilderPanel()).AddTo(this);
            iconBuilderPanel.SaveCommand.Subscribe(OnIconSelected).AddTo(this);
            return UniTask.CompletedTask;
        }

        private void NavigateToIconBuilderPanel()
        {
            detailsPanel.gameObject.SetActive(false);
            iconBuilderPanel.gameObject.SetActive(true);
        }

        private void OnIconSelected(IconBuilderPanel.Result result)
        {
            iconBuilderPanel.gameObject.SetActive(false);
            detailsPanel.gameObject.SetActive(true);
            detailsPanel.SetCompanyLogo(result.ShapeSprite, result.IconSprite, result.BackgroundColor);
            Controller.SetCompanyLogoData(result.ShapeSprite, result.IconSprite, result.BackgroundColor);
        }

        public override void Dispose()
        {
        }
    }
}