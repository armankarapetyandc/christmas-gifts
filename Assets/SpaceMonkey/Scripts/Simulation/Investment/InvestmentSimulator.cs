using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Profile;
using Zenject;

namespace SpaceMonkey.Scripts.Simulation.Investment
{
    public class InvestmentSimulator
    {
        private GameConfig _gameConfig;
        private AccountService _accountService;

        [Inject]
        private void Inject(GameConfig gameConfig, AccountService accountService)
        {
            _accountService = accountService;
            _gameConfig = gameConfig;
        }

        public bool IsBought(int placeId)
        {
            var account = _accountService.Model.Account;
            return account.InvestmentGameData?.UnlockedPlaces.Contains(placeId) ?? false;
        }
        
        public bool BuyPlace(int placeId)
        {
            var account = _accountService.Model.Account;
            if (account.CanAfford(_gameConfig.NewLocationInfo.Price))
            {
                account.Buy(_gameConfig.NewLocationInfo.Price);
                if (account.InvestmentGameData == null)
                {
                    account.CreateInvestmentData();
                }
                account.InvestmentGameData.UnlockedPlaces.Add(placeId);
                _accountService.SaveAsync().Forget();
                return true;
            }

            return false;
        }
    }
}