using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using SpaceMonkey.Scripts.UI.Popups.Core;
using SpaceMonkey.Scripts.UI.Views.Opportunities.BusinessLocationSign;
using SpaceMonkey.Scripts.Utilities;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Popups.BusinessLocationUnlocked
{
    public class BusinessLocationUnlockedPopupController : BasePresenterController
    {
        private readonly PopupPresenterService _popupPresenterService;
        private readonly NavigationPresenterService _navigationPresenterService;
        public BusinessLocationUnlockedPopupController(PresenterService presenterService,
            PopupPresenterService popupPresenterService,
            NavigationPresenterService navigationPresenterService) : base(presenterService)
        {
            _popupPresenterService = popupPresenterService;
            _navigationPresenterService = navigationPresenterService;
        }

        public new void Close()
        {
            _popupPresenterService.HideLast();
        }
        
        public void RedirectToBusinessLoanStatement(int dataPlaceId)
        {
            _navigationPresenterService.HideAll();
            PresenterService.HidePreviousAndShow<BusinessLocationSignView>(new BusinessLocationSignView.Data
            {
                PlaceId = dataPlaceId
            }).Forget();
        }
    }
}