using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.UI.Navigation.Bottom;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;
using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Staff
{
    public class StaffController : BasePresenterController
    {
        private readonly NavigationPresenterService _navigationPresenterService;
        [Inject] private GameConfig _gameConfig;
        
        public StaffController(PresenterService presenterService, NavigationPresenterService navigationPresenterService) : base(presenterService)
        {
            _navigationPresenterService = navigationPresenterService;
        }

        public Configs.Staff[] GetStaffs()
        {
            return _gameConfig.Staffs;
        }

        internal void OnBack()
        {
            _navigationPresenterService.Show<MainNavigation>(new MainNavigation.Data
            {
                Type = MainNavigationType.BusinessHub
            }).Forget();
        }
    }
}