using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Configs.Map;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.Simulation.BusinessLoan;
using SpaceMonkey.Scripts.Simulation.CreditCard;
using SpaceMonkey.Scripts.Simulation.Fire;
using SpaceMonkey.Scripts.Simulation.Investment;
using SpaceMonkey.Scripts.UI.Asset.Database;
using SpaceMonkey.Scripts.UI.Navigation.Bottom;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using SpaceMonkey.Scripts.UI.Popups.BigOrder;
using SpaceMonkey.Scripts.UI.Popups.Core;
using SpaceMonkey.Scripts.UI.Popups.CreditCard;
using SpaceMonkey.Scripts.UI.Popups.BusinessLoan;
using SpaceMonkey.Scripts.UI.Popups.BusinessLocationUnlocked;
using SpaceMonkey.Scripts.UI.Popups.Competition;
using SpaceMonkey.Scripts.UI.Views.Opportunities.BigOrder;
using SpaceMonkey.Scripts.UI.Views.Opportunities.BigOrderDetails;
using SpaceMonkey.Scripts.UI.Views.Opportunities.BusinessLoanStatement;
using SpaceMonkey.Scripts.Utilities;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Views.Map
{
    public class MapController : BasePresenterController
    {
        private readonly PopupPresenterService _popupPresenterService;
        private readonly NavigationPresenterService _navigationPresenterService;
        private readonly AccountService _accountService;
        private readonly CreditSimulator _creditSimulator;
        private readonly BusinessLoanSimulator _businessLoanSimulator;
        private readonly FireSimulator _fireSimulator;
        private readonly MapConfig _mapConfig;
        private readonly VisualAssetDatabase _visualAssetDatabase;
        private readonly InvestmentSimulator _investmentSimulator;
        private readonly GameConfig _gameConfig;

        public int CurrentWeek => _accountService.Model.Account.Week;
        public int CurrentLevel => _accountService.Model.Account.Level;
        public int ReviewsCount => _accountService.Model.Account.WeeksV2.Count > 0 ? _accountService.Model.Account.Reviews.Count:1;

        public MapController(PresenterService presenterService,PopupPresenterService popupPresenterService,
            NavigationPresenterService navigationPresenterService,AccountService accountService,
            CreditSimulator creditSimulator, BusinessLoanSimulator businessLoanSimulator,
            FireSimulator fireSimulator, MapConfig mapConfig,VisualAssetDatabase visualAssetDatabase,GameConfig gameConfig,
            InvestmentSimulator investmentSimulator) : base(presenterService)
        {
            _gameConfig = gameConfig;
            _investmentSimulator = investmentSimulator;
            _popupPresenterService = popupPresenterService;
            _navigationPresenterService = navigationPresenterService;
            _accountService = accountService;
            _creditSimulator = creditSimulator;
            _businessLoanSimulator = businessLoanSimulator;
            _fireSimulator = fireSimulator;
            _mapConfig = mapConfig;
            _visualAssetDatabase = visualAssetDatabase;
        }
        
        internal void TryAddAppearedPlace(int placeId)
        {
            _accountService.Model.TryAddAppearedPlace(placeId);
        }
        internal T ResolveVisualAsset<T>(string id) where T : VisualAsset
        {
            return _visualAssetDatabase.GetResourceForAsset<T>(id);
        }

        internal IMapPlace[] GetMapPlaces()
        {
            return _mapConfig.Places.Append(GetDefaultCompanyPlace()).ToArray();
        }

        internal List<IMapPlace> GetAppearedMapPlaces(int currentWeek)
        {
            var places = GetMapPlaces();
            return _mapConfig.GetAppearedMapPlaces(places, currentWeek,_accountService.Model.Account.Level);
        }

        private IMapPlace GetDefaultCompanyPlace()
        {
            var account = _accountService.Model.Account;
            var place = _mapConfig.DefaultCompanyPlace;
            place.Name = $"{account.Company.CompanyName}\nLevel {account.Level}";
            place.SingleLineName = account.Company.CompanyName;//$"{account.Company.CompanyName} Level {account.Level}";
            place.IconVisualAsset = ResolveVisualAsset<SpriteVisualAsset>(account.Company.Logo.IconVisualAssetId);
            return place;
        }

        public Profile.Product GetCompetitionProduct()
        {
            var product = _accountService.GetSavedProduct();
            if (!string.IsNullOrEmpty(product.Id))
            {
                return product;
            }

            return  _accountService.GetCompetitionProduct();
        } 
        
        internal float CalculateCompanyRating()
        {
            return _accountService.Model.Account.CalculateCompanyRating(_gameConfig.SimulationInfo.MoodRanges,_accountService.Model.Account.WeeksV2);
        }
        

        internal void ShowCreditCardInfoPopup()
        {
            _popupPresenterService.Show<CreditCardPopup>().Forget();
        }
        
        internal void ShowBigOrderView()
        {
            if (_accountService.Model.Account.BigOrderGameData  == null)
            {
                _popupPresenterService.Show<BigOrderPopup>().Forget();
                return;
            }

            _navigationPresenterService.HideAll();
            PresenterService.HidePreviousAndShow<BigOrderDetailsView>(new BigOrderDetailsView.Data()
            {
                Type = MainNavigationType.Map
            }).Forget();

        }

        
        internal void ShowBusinessLoanPopup()
        {
            _popupPresenterService.Show<BusinessLoanPopup>().Forget();
        }
        
        internal void ShowBusinessLoanView()
        {
            _navigationPresenterService.HideAll();
            PresenterService.HidePreviousAndShow<BusinessLoanStatementView>(new BusinessLoanStatementView.Data()).Forget();
        }

        internal bool HasBusinessLoan()
        {
            return _businessLoanSimulator.HasActiveLoan;
        }
        
        internal bool HasCreditCard()
        {
            return _creditSimulator.HasActiveCard;
        }

        internal bool HasBigOrder()
        {
            return _accountService.Model.Account.BigOrderGameData is { IsActive: true };
        }

        internal void NavigateToMyCompany()
        {
            _navigationPresenterService.HideAll();
            _navigationPresenterService.Show<MainNavigation>(new MainNavigation.Data
            {
                Type = MainNavigationType.BusinessHub
            }).Forget();
        }

        public void ShowInvestmentView(IMapPlace place)
        {
            if (_investmentSimulator.IsBought(place.Id))
            {
                return;
            }
            _popupPresenterService.Show<BusinessLocationUnlockedPopup>(new BusinessLocationUnlockedPopup.Data
            {
                PlaceId = place.Id
            }).Forget();
        }

        public void ShowCompetitionPopup(bool isForce = false)
        {
            _popupPresenterService.Show<CompetitionPopup>(new CompetitionPopup.Data
            {
                ShowInfoPopup = isForce
            }).Forget();
        }
        
        internal void CheckAndShowFirePopup()
        {
            if (_fireSimulator.HasActiveFire == false)
                return;
            
            
            _popupPresenterService.Show<Popups.Fire.FirePopup>().Forget();
        }

        internal bool HasActiveFire()
        {
            return _fireSimulator.HasActiveFire;
        }
    }
}