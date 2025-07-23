using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.UI.Views.CategorySelection.BusinessIdeaItems;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.Serialization;
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

        [SerializeField] private BusinessIdeaItem businessIdeaItemPrefab;

        [SerializeField] private RectTransform businessIdeaContainer;

        public override UniTask Initialize(IPresenterData data = null)
        {
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
                foreach (var businessIdea in businessData)
                {
                    BusinessIdeaItem businessIdeaItem = Instantiate(businessIdeaItemPrefab, businessIdeaContainer);
                    businessIdeaItem.Initialize(businessIdea);
                    //businessIdeaItem.OnClickObservable.Subscribe(ShowApprove).AddTo(businessIdea);
                }
            }
        }

        private void ShowApprove(BusinessIdeaItem businessIdea)
        {
        }

        public override void Dispose()
        {
        }
    }
}