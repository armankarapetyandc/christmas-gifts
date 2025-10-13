using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.Simulation;
using SpaceMonkey.Scripts.UI.Asset.Database;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using SpaceMonkey.Scripts.UI.Popups.Core;
using SpaceMonkey.Scripts.UI.Popups.LevelInfo;
using SpaceMonkey.Scripts.UI.Popups.LevelInfoAuto;
using SpaceMonkey.Scripts.UI.Utility;
using SpaceMonkey.Scripts.UI.Views.BusinessExamples;
using SpaceMonkey.Scripts.UI.Views.Marketing;
using SpaceMonkey.Scripts.UI.Views.Orders;
using SpaceMonkey.Scripts.UI.Views.Product.ProductList;
using SpaceMonkey.Scripts.UI.Views.ProductionCapacity;
using SpaceMonkey.Scripts.UI.Views.ProfitAndLoss;
using SpaceMonkey.Scripts.UI.Views.Staff;
using SpaceMonkey.Scripts.Utilities;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Views.BusinessHub
{
    public class BusinessHubController : BasePresenterController
    {
        private readonly AccountService _accountService;
        private readonly VisualAssetDatabase _visualAssetDatabase;
        private readonly NavigationPresenterService _navigationPresenterService;
        private readonly WeekSimulationContext _weekSimulationContext;
        private PopupPresenterService _popupPresenterService;
        private GameConfig _gameConfig;

        public Observable<int> OnLevelChanged => _accountService.OnLevelChanged;
        public Observable<float> OnMoneyChanged => _accountService.Model.Account.OnMoneyChanged;
        public int Level => _accountService.Model.Account.Level;
        public float Money => _accountService.Model.Account.Money;


        public readonly ReactiveCommand<LockByLevel> OnUnlockByLevel = new ReactiveCommand<LockByLevel>();
        public readonly ReactiveCommand<LockByMoney> OnUnlockByMoney = new ReactiveCommand<LockByMoney>();

        public BusinessHubController(PresenterService presenterService,
            AccountService accountService, VisualAssetDatabase visualAssetDatabase,
            NavigationPresenterService navigationPresenterService,
            WeekSimulationContext weekSimulationContext,
            PopupPresenterService popupPresenterService, GameConfig gameConfig) : base(presenterService)
        {
            _gameConfig = gameConfig;
            _popupPresenterService = popupPresenterService;
            _accountService = accountService;
            _visualAssetDatabase = visualAssetDatabase;
            _navigationPresenterService = navigationPresenterService;
            _weekSimulationContext = weekSimulationContext;
        }


        internal T ResolveVisualAsset<T>(string id) where T : VisualAsset
        {
            return _visualAssetDatabase.GetResourceForAsset<T>(id);
        }


        internal Account GetAccount()
        {
            return _accountService.Model.Account;
        }

        internal void ShowProductView()
        {
            _navigationPresenterService.HideAll();
            PresenterService.HidePreviousAndShow<ProductListView>().Forget();
        }


        internal void CheckForUnlockByMoney(LockByMoney[] lockedByMoney, float money)
        {
            foreach (var byMoney in lockedByMoney)
            {
                if (byMoney.Value <= money)
                {
                    OnUnlockByMoney.Execute(byMoney);
                }
            }
        }

        internal void CheckForUnlockByMoney(LockByLevel[] lockedByLevels, int level)
        {
            foreach (var lockedByLevel in lockedByLevels)
            {
                var levelInfo = _gameConfig.LevelInfos.FirstOrDefault(info => info.Level == level);
                var unlockedInfo =
                    levelInfo?.UnlockInfo.FirstOrDefault(unlockedInfo => unlockedInfo.Key == lockedByLevel.Key);
                if (unlockedInfo != null)
                {
                    OnUnlockByLevel.Execute(lockedByLevel);
                }
            }
        }

        internal void StartWeek()
        {
            _weekSimulationContext.Run();
        }

        internal void ShowProductionView()
        {
            _navigationPresenterService.HideAll();
            PresenterService.HidePreviousAndShow<ProductionCapacityView>().Forget();
        }

        internal void ShowAllReviewView()
        {
            _navigationPresenterService.HideAll();
            PresenterService.HidePreviousAndShow<AllReviewView.AllReviewView>().Forget();
        }

        internal void ShowLevelInfoPopup()
        {
            _popupPresenterService.Show<LevelInfoPopup>().Forget();
        }

        public void ShowMarketingView()
        {
            _navigationPresenterService.HideAll();
            PresenterService.HidePreviousAndShow<MarketingView>().Forget();
        }

        public void ShowStaffView()
        {
            _navigationPresenterService.HideAll();
            PresenterService.HidePreviousAndShow<StaffView>(new StaffView.Data()
            {
                Profession = EmployeeProfession.All
            }).Forget();
        }

        public void ShowPlmView()
        {
            if (_accountService.Model.Account.Weeks.Count <= 0)
            {
                return;
            }

            _navigationPresenterService.HideAll();
            PresenterService.Show<ProfitView>(new ProfitView.Data
            {
                EnableBackButton = true
            }).Forget();
        }
    }
}