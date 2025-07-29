using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.UI.Asset.BusinessIdeas;
using SpaceMonkey.Scripts.UI.Views.Business.BusinessSetup;
using SpaceMonkey.Scripts.UI.Views.Startup;
using SpaceMonkey.Scripts.Utilities;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Business.CategorySelection
{
    public class CategorySelectionController : BasePresenterController
    {
        private readonly BusinessIdeaAssetDatabase _ideaAssetDatabase;

        public CategorySelectionController(PresenterService presenterService,
            BusinessIdeaAssetDatabase ideaAssetDatabase) : base(presenterService)
        {
            _ideaAssetDatabase = ideaAssetDatabase;
        }

        internal List<IdeaInfo> RetrieveIdeas()
        {
            return _ideaAssetDatabase.Assets.Select(asset =>
                new IdeaInfo(asset.BusinessIdeaName, asset.BusinessIdeaItemAsset)).ToList();
        }

        public void Back()
        {
            PresenterService.HidePreviousAndShow<StartupView>().Forget();
        }

        public void OnNext()
        {
            PresenterService.HidePreviousAndShow<BusinessSetupView>().Forget();
        }
    }
}