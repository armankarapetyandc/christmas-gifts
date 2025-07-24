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
            backButton.gameObject.SetActive(false);
            infoButton.OnClickAsObservable().Subscribe(_ => Controller.OnInfoButtonClicked());
            backButton.OnClickAsObservable().Subscribe(_ => OnBackButtonClicked());
            nextButton.OnClickAsObservable().Subscribe(_ => Controller.OPenNextView());
            LoadBusinessIdeas();

            return UniTask.CompletedTask;
        }

        private void LoadBusinessIdeas()
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
            backButton.gameObject.SetActive(true);
            selectedIdea.Initialize(businessIdea);
        }
        
        private void OnBackButtonClicked()
        {
            categorySelection.SetActive(true);
            categoryApprove.SetActive(false);
            backButton.gameObject.SetActive(false);
        }

        public override void Dispose()
        {
        }
    }
}