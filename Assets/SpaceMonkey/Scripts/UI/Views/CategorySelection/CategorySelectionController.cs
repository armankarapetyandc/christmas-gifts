using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.UI.Asset.BusinessIdeas;
using SpaceMonkey.Scripts.UI.Views.CategorySelection.BusinessIdeaItems;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Views.CategorySelection
{
    public class CategorySelectionController : BasePresenterController
    {
        private PresenterService _presenterService;
        private BusinessIdeaAssetDatabase _ideaAssetDatabase;
        
        
        public CategorySelectionController(PresenterService presenterService, BusinessIdeaAssetDatabase businessIdea) : base(presenterService)
        {
            _presenterService = presenterService;
            _ideaAssetDatabase = businessIdea;
        }

        public List<BusinessItemData> GetBusinessItemsData()
        {
            return _ideaAssetDatabase.Assets
                .Select(asset => new BusinessItemData(asset.BusinessIdeaName, asset.BusinessIdeaItemAsset)).ToList();
        }
        

        public void OnInfoButtonClicked()
        {
            Debug.LogError("Opened Info Popup");
        }

        public void OnBackButtonClicked()
        {
            Debug.LogError("Back Button Clicked");
        }
    }
}