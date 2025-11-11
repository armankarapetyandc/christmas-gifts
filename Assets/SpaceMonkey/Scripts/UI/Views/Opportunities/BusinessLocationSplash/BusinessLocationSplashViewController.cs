using System;
using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.UI.Navigation.Bottom;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using SpaceMonkey.Scripts.UI.Views.Map;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.BusinessLocationSplash
{
    public class BusinessLocationSplashViewController : BasePresenterController
    {
        private readonly NavigationPresenterService _navigationPresenterService;

        public BusinessLocationSplashViewController(PresenterService presenterService,
            NavigationPresenterService navigationPresenterService) : base(presenterService)
        {
            _navigationPresenterService = navigationPresenterService;
        }

        public async UniTaskVoid OnNext(int dataPlaceId)
        {
            await PresenterService.Hide();
            await _navigationPresenterService.Show<MainNavigation>(new MainNavigation.Data
            {
                Type = MainNavigationType.Map
            });
            var mapView = PresenterService.GetPresenter<MapView>();
            await UniTask.WaitWhile(() =>
            {
                mapView = PresenterService.GetPresenter<MapView>();
                return mapView == null;
            });
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            mapView?.FocusOnPlace(dataPlaceId);
        }
    }
}