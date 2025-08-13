using SpaceMonkey.Scripts.UI.Asset;
using SpaceMonkey.Scripts.UI.Asset.Database;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Product.ProductIconBuilder
{
    public class ProductIconBuilderController : BasePresenterController
    {
        private readonly VisualAssetDatabase _visualAssetDatabase;

        public ProductIconBuilderController(PresenterService presenterService,VisualAssetDatabase visualAssetDatabase) : base(presenterService)
        {
            _visualAssetDatabase = visualAssetDatabase;
        }

        internal VisualAsset ResolveVisualAsset(string id)
        {
            return _visualAssetDatabase.GetResource(id);
        }
    }
}