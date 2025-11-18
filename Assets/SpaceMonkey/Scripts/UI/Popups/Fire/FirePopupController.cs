using Cysharp.Threading.Tasks;
using Org.BouncyCastle.Bcpg.OpenPgp;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.Simulation.Fire;
using SpaceMonkey.Scripts.UI.Navigation.Bottom;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using SpaceMonkey.Scripts.UI.Popups.Core;
using SpaceMonkey.Scripts.UI.Views.ProductionCapacity;
using SpaceMonkey.Scripts.Utilities;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Popups.Fire
{
    public class FirePopupController : BasePresenterController
    {
        private readonly NavigationPresenterService _navigationPresenterService;
        private readonly PopupPresenterService _popupPresenterService;

        public FirePopupController(
            PresenterService presenterService,
            NavigationPresenterService navigationPresenterService,
            PopupPresenterService popupPresenterService) : base(presenterService)
        {
            _navigationPresenterService = navigationPresenterService;
            _popupPresenterService = popupPresenterService;
        }
        
        public void NavigateToProductionCapacity()
        {
            _popupPresenterService.HideLast();
            _navigationPresenterService.HideAll();
            PresenterService.HidePreviousAndShow<ProductionCapacityView>(new ProductionCapacityView.Data()).Forget();
        }

        public void ClosePopUp()
        {
            _popupPresenterService.HideLast();
        }
    }
}
