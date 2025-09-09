using SpaceMonkey.Scripts.Configs.Map;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Map
{
    public class MapController : BasePresenterController
    {
        private readonly MapConfig _mapConfig;

        public MapController(PresenterService presenterService,MapConfig mapConfig) : base(presenterService)
        {
            _mapConfig = mapConfig;
        }

        internal MapPlace[] GetMapPlaces()
        {
            return _mapConfig.Places;
        }
    }
}