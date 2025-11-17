using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Profile;
using Zenject;

namespace SpaceMonkey.Scripts.Simulation.Fire
{
    public class FireSimulator
    {
        private AccountService _accountService;
        
        public FireGameData Data => _accountService.Model.Account.FireData;
        private int Week => _accountService.Model.Account.Week;
        public bool HasActiveFire => Data != null && Data.IsActive;
        public bool HasTriggered => Data != null;
        
        private const int FIRE_TRIGGER_MONTH = 7;
        private const int WEEKS_PER_MONTH = 4;
        private const int FIRE_TRIGGER_WEEK = FIRE_TRIGGER_MONTH * WEEKS_PER_MONTH;
        
        [Inject]
        private void Inject(AccountService accountService)
        {
            _accountService = accountService;
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
            // Fire triggers at week 28 (month 7) and hasn't been triggered yet
            return Week == FIRE_TRIGGER_WEEK && !HasTriggered;
        }
        
        private async UniTask TriggerFire()
        {
            if (HasTriggered)
                return;
            
            // Create fire data
            _accountService.Model.Account.CreateFireData();
            
            // Mark all equipment as needing repair (50% capacity reduction)
            foreach (var level in _accountService.Model.Account.LevelProdCaps)
            {
                _accountService.Model.Account.SetLevel(level);
            }
            
            await _accountService.SaveAsync();
        }
        
        public float GetRepairCost()
        {
            if (!HasActiveFire)
                return 0;

            return 500;
        }
        
        public async UniTask RepairFire(float cost)
        {
            if (!HasActiveFire)
                return;
            
            var account = _accountService.Model.Account;
            
            if (!account.CanAfford(cost))
                return;
            
            // Pay for repair
            account.Buy(cost);
            
            // Mark fire as inactive
            Data.IsActive = false;
            
            await _accountService.SaveAsync();
        }
        
        public int GetDamagedCapacity()
        {
            if (!HasActiveFire)
                return 0;
            
            // Calculate current capacity (should be 50% of original due to NeedRepair)
            int currentCapacity = _accountService.Model.Account.GetProductionCapacity();
            return (int)(currentCapacity * 0.5f);
        }
    }
}
