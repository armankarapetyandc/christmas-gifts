using System;
using System.Linq;
using AudioPlayer;
using AudioPlayerService.Runtime;
using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.Simulation;
using SpaceMonkey.Scripts.Simulation.Fire;
using SpaceMonkey.Scripts.UI.Asset.Database;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using SpaceMonkey.Scripts.UI.Popups.Competition;
using SpaceMonkey.Scripts.UI.Popups.Core;
using SpaceMonkey.Scripts.UI.Popups.LevelInfo;
using SpaceMonkey.Scripts.UI.Popups.LevelInfoAuto;
using SpaceMonkey.Scripts.UI.Utility;
using SpaceMonkey.Scripts.UI.Utility.Locker;
using SpaceMonkey.Scripts.UI.Views.BusinessExamples;
using SpaceMonkey.Scripts.UI.Views.Marketing;
using SpaceMonkey.Scripts.UI.Views.Orders;
using SpaceMonkey.Scripts.UI.Views.Product.NewProduct;
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
        private readonly FireSimulator fireSimulator;
        private GameConfig _gameConfig;

        public Observable<int> OnLevelChanged => _accountService.OnLevelChanged;
        public Observable<float> OnMoneyChanged => _accountService.Model.Account.OnMoneyChanged;
        public int Level => _accountService.Model.Account.Level;
        public float Money => _accountService.Model.Account.Money;


        public readonly ReactiveCommand<LockBy> OnUnlockBy = new();

        public BusinessHubController(PresenterService presenterService,FireSimulator fireSimulator,
            AccountService accountService, VisualAssetDatabase visualAssetDatabase,
            NavigationPresenterService navigationPresenterService,
            WeekSimulationContext weekSimulationContext,
            PopupPresenterService popupPresenterService, GameConfig gameConfig) : base(presenterService)
        {
            this.fireSimulator = fireSimulator;
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

        public LevelInfo GetLeveInfoData(int level)
        {
            return _gameConfig.GetLeveInfoData(level);
        }
        
        internal Account GetAccount()
        {
            return _accountService.Model.Account;
        }

        internal void ShowProductView()
        {
            _navigationPresenterService.HideAll();

            if (_accountService.Model.Account.Products.Count==0)
            {
                PresenterService.HidePreviousAndShow<ProductView>(new ProductView.Data
                {
                    SelectedProduct = null
                }).Forget();
                return;
            }
            
            PresenterService.HidePreviousAndShow<ProductListView>().Forget();
        }
        internal void CheckAndShowFirePopup()
        {
            if (fireSimulator.HasActiveFire == false)
                return;
            
            
            _popupPresenterService.Show<Popups.Fire.FirePopup>().Forget();
        }
        
        
        internal void CheckForUnlockByMoney(LockByLevel[] lockedByLevels, int level)
        {
            var levelInfo = _gameConfig.LevelInfos.FirstOrDefault(info => info.Level == level);
            foreach (var lockedByLevel in lockedByLevels)
            {

                var unlockedInfo =
                    levelInfo?.UnlockInfo.FirstOrDefault(unlockedInfo => unlockedInfo.Key == lockedByLevel.Key);
                if (unlockedInfo != null || PlayerPrefs.GetInt(lockedByLevel.Key) == 1)
                {
                    OnUnlockBy.Execute(lockedByLevel);
                }
            }
        }

        
        internal void CheckForUnlockByReview(LockByReview[] lockByReview,int week)
        {
            foreach (var byReview in lockByReview)
            {
                if (byReview.Count <= week || PlayerPrefs.GetInt(byReview.Key) == 1)
                {
                    OnUnlockBy.Execute(byReview);
                }
            }
        }

        internal void CheckForUnlockByWeek(LockByWeek[] lockByWeek,int week)
        {
            foreach (var byWeek in lockByWeek)
            {
                if (byWeek.Count <= week || PlayerPrefs.GetInt(byWeek.Key) == 1)
                {
                    OnUnlockBy.Execute(byWeek);
                }
            }
        }

        internal void CheckForUnlockByMoney(LockByMoney[] lockedByMoney, float money)
        {
            foreach (var byMoney in lockedByMoney)
            {
                if (byMoney.Value <= money || PlayerPrefs.GetInt(byMoney.Key) == 1)
                {
                    OnUnlockBy.Execute(byMoney);
                }
            }
        }

        internal LevelInfo GetLevelUnlockConfig(string key)
        {
            foreach (var gameConfigLevelInfo in _gameConfig.LevelInfos)
            {
                var contains = gameConfigLevelInfo.UnlockInfo.Any(info => info.Key == key);
                if (contains)
                {
                    return gameConfigLevelInfo;
                }
            }

            return null;
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
            if (_accountService.Model.Account.WeeksV2.Count <= 0)
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