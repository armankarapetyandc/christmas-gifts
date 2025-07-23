using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.UI.Views.CategorySelection.BusinessIdeaItems;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.CategorySelection
{
    public class CategorySelectionView : BasePresenterWithController<CategorySelectionController>
    {
        [SerializeField] private GameObject categorySelection;
        [SerializeField] private GameObject categoryApprove;
        [SerializeField] private Button infoButton;
        [SerializeField] private Button backButton;
        [SerializeField] private Button nextButton;
        [SerializeField] private BusinessIdeaItem selectedIdea;

        [SerializeField] private BusinessIdeaItem businessIdeaItemPrefab;

        [SerializeField] private RectTransform businessIdeaContainer;
        
        [SerializeField] private List<BusinessIdeaItem> businessItemsList;

        public override UniTask Initialize(IPresenterData data = null)
        {
            categoryApprove.SetActive(false);
            categorySelection.SetActive(true);
            backButton.interactable = false;
            infoButton.onClick.AddListener(Controller.OnInfoButtonClicked);
            backButton.onClick.AddListener(Controller.OnBackButtonClicked);
            LoadBusinessIdeas().Forget();

            return UniTask.CompletedTask;
        }

        private async UniTask LoadBusinessIdeas()
        {
            var businessData = Controller.GetBusinessItemsData();
            if (businessData != null)
            {
                for (var i = 0; i < businessItemsList.Count; ++i)
                {
                    var item = businessItemsList[i];
                    item.Initialize(businessData[i]);
                    item.OnClickObservable.Subscribe(ShowApprove).AddTo(item);
                }
            }
        }

        private void ShowApprove(BusinessItemData businessIdea)
        {
            categorySelection.SetActive(false);
            categoryApprove.SetActive(true);
            backButton.interactable = true;
            selectedIdea.Initialize(businessIdea);
        }

        public override void Dispose()
        {
        }
    }
}