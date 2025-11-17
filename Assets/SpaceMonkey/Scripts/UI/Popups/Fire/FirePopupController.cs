using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.Simulation.Fire;
using SpaceMonkey.Scripts.UI.Navigation.Bottom;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using SpaceMonkey.Scripts.UI.Views.ProductionCapacity;
using SpaceMonkey.Scripts.Utilities;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Popups.Fire
{
    public class FirePopupController : BasePresenterController
    {
        private readonly NavigationPresenterService _navigationPresenterService;
        private readonly FireSimulator _fireSimulator;
        private readonly AccountService _accountService;
        
        public FirePopupController(
            PresenterService presenterService,
            NavigationPresenterService navigationPresenterService,
            FireSimulator fireSimulator,
            AccountService accountService) : base(presenterService)
        {
            _navigationPresenterService = navigationPresenterService;
            _fireSimulator = fireSimulator;
            _accountService = accountService;
        }
        
        public void NavigateToProductionCapacity()
        {
            _navigationPresenterService.HideAll();
            PresenterService.HidePreviousAndShow<ProductionCapacityView>(new ProductionCapacityView.Data()).Forget();
        }
        
        public async UniTask RepairFire()
        {
            float repairCost = _fireSimulator.GetRepairCost();
            
            if (!_accountService.Model.Account.CanAfford(repairCost))
            {
                // TODO: Show insufficient funds message
                return;
            }
            
            await _fireSimulator.RepairFire(repairCost);
        }
    }
}
