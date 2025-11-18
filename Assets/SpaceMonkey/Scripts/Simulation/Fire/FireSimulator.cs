using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Profile;
using Zenject;

namespace SpaceMonkey.Scripts.Simulation.Fire
{
    public class FireSimulator
    {
        private AccountService _accountService;
        private GameConfig _gameConfig;
        
        public FireGameData Data => _accountService.Model.Account.FireData;
        private int Week => _accountService.Model.Account.Week;
        public bool HasActiveFire => Data != null && Data.IsActive;
        public bool HasTriggered => Data != null;
        
        [Inject]
        private void Inject(AccountService accountService, GameConfig gameConfig)
        {
            _accountService = accountService;
            _gameConfig = gameConfig;
        }

        public void TryTriggerFire()
        {
            if (ShouldTriggerFire())
            {
                TriggerFire().Forget();
            }
        }
        private bool ShouldTriggerFire()
        {
            return Week == _gameConfig.FireInfo.TriggerWeek && !HasTriggered;
        }
        
        private async UniTask TriggerFire()
        {
            if (HasTriggered)
                return;
            
            _accountService.Model.Account.CreateFireData(_gameConfig.FireInfo.CapacityReductionPercent);
            await _accountService.SaveAsync();
        }
        
        public float GetRepairCost()
        {
            if (!HasActiveFire)
                return 0;

            return _gameConfig.FireInfo.RepairCost;
        }
        
        public async UniTask<bool> RepairFire(float cost)
        {
            if (!HasActiveFire)
                return false;
            
            var account = _accountService.Model.Account;
            
            if (!account.CanAfford(cost))
                return false;

            
            // Pay for repair
            account.Buy(cost);

            await RepairFire();
            return true;
        }

        public async UniTask RepairFire()
        {
            // Mark fire as inactive
            Data.IsActive = false;
            
            await _accountService.SaveAsync();
        }

        
        public int GetDamagedCapacity()
        {
            if (!HasActiveFire)
                return 0;
            
            // Calculate current capacity reduced by the configured percentage
            int currentCapacity = _accountService.Model.Account.GetProductionCapacity();
            return (int)(currentCapacity * _gameConfig.FireInfo.CapacityReductionPercent);
        }
    }
}
