using System;
using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Views.Business.BusinessSetup.Panels;
using SpaceMonkey.Scripts.UI.Views.Business.BusinessSetup.Panels.BusinessDetails;
using SpaceMonkey.Scripts.UI.Views.Business.BusinessSetup.Panels.HashTag;
using SpaceMonkey.Scripts.UI.Views.Business.BusinessSetup.Panels.IconBuilder;
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
        [SerializeField] private HashTagPanel hashTagPanel;
        [SerializeField] private Button saveButton;

        public override UniTask Initialize(IPresenterData data = null)
        {
            backButton.OnClickAsObservable().Subscribe(_ => Controller.OnBack()).AddTo(this);
            saveButton.OnClickAsObservable().Subscribe(_ => Controller.OnNext()).AddTo(this);
            detailsPanel.OnIconButtonClicked.Subscribe(_ => NavigateToIconBuilderPanel()).AddTo(this);
            detailsPanel.OnHashtagButtonClicked.Subscribe(_ => NavigateToHashTagsPanel()).AddTo(this);
            iconBuilderPanel.SaveCommand.Subscribe(OnIconSelected).AddTo(this);
            hashTagPanel.SaveCommand.Subscribe(OnHashTagsSelected).AddTo(this);
            return UniTask.CompletedTask;
        }

        private void NavigateToHashTagsPanel()
        {
            detailsPanel.gameObject.SetActive(false);
            hashTagPanel.gameObject.SetActive(true);
        }

        private void NavigateToIconBuilderPanel()
        {
            detailsPanel.gameObject.SetActive(false);
            iconBuilderPanel.gameObject.SetActive(true);
        }

        private void OnIconSelected(IconBuilderPanel.Result result)
        {
            Controller.SetCompanyLogoData(result.ShapeSprite, result.IconSprite, result.BackgroundColor);
            iconBuilderPanel.gameObject.SetActive(false);
            detailsPanel.gameObject.SetActive(true);
            detailsPanel.SetCompanyLogo(result.ShapeSprite, result.IconSprite, result.BackgroundColor);
        }

        private void OnHashTagsSelected(HashTagPanel.Result result)
        {
            Controller.SetCompanyHashTags(result.Tags);
            hashTagPanel.gameObject.SetActive(false);
            detailsPanel.gameObject.SetActive(true);
            detailsPanel.SetHashTags(result.Tags);
        }

        public override void Dispose()
        {
        }
    }
}