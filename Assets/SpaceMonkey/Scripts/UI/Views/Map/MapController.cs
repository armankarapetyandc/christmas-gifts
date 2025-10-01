using System.Linq;
using SpaceMonkey.Scripts.Configs.Map;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Asset.Database;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Map
{
    public class MapController : BasePresenterController
    {
        private readonly AccountService _accountService;
        private readonly MapConfig _mapConfig;
        private readonly VisualAssetDatabase _visualAssetDatabase;

        public MapController(PresenterService presenterService,AccountService accountService,MapConfig mapConfig,VisualAssetDatabase visualAssetDatabase) : base(presenterService)
        {
            _accountService = accountService;
            _mapConfig = mapConfig;
            _visualAssetDatabase = visualAssetDatabase;
        }
        
        internal T ResolveVisualAsset<T>(string id) where T : VisualAsset
        {
            return _visualAssetDatabase.GetResourceForAsset<T>(id);
        }

        internal IMapPlace[] GetMapPlaces()
        {
            return _mapConfig.Places.Append(GetDefaultCompanyPlace()).ToArray();
        }

        private IMapPlace GetDefaultCompanyPlace()
        {
            var account = _accountService.Model.Account;
            var place = _mapConfig.DefaultCompanyPlace;
            place.Name = $"{account.Company.CompanyName}\nLevel {account.Level}";
            place.IconVisualAsset = ResolveVisualAsset<SpriteVisualAsset>(account.Company.Logo.IconVisualAssetId);
            return place;
        }
    }
}