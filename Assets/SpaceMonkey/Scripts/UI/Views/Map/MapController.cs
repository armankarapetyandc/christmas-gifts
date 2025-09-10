using SpaceMonkey.Scripts.Configs.Map;
using SpaceMonkey.Scripts.UI.Asset.Database;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Map
{
    public class MapController : BasePresenterController
    {
        private readonly MapConfig _mapConfig;
        private readonly VisualAssetDatabase _visualAssetDatabase;

        public MapController(PresenterService presenterService,MapConfig mapConfig,VisualAssetDatabase visualAssetDatabase) : base(presenterService)
        {
            _mapConfig = mapConfig;
            _visualAssetDatabase = visualAssetDatabase;
        }
        
        internal T ResolveVisualAsset<T>(string id) where T : VisualAsset
        {
            return _visualAssetDatabase.GetResourceForAsset<T>(id);
        }

        internal MapPlace[] GetMapPlaces()
        {
            return _mapConfig.Places;
        }
    }
}