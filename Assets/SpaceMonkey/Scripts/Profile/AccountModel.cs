using System;
using R3;

namespace SpaceMonkey.Scripts.Profile
{
    public class AccountModel:IDisposable
    {
        private readonly ReactiveProperty<float> _money = new ReactiveProperty<float>();
        private readonly ReactiveProperty<float> _score = new ReactiveProperty<float>();
        private readonly ReactiveProperty<float> _productionCapacity = new ReactiveProperty<float>();

        public ReadOnlyReactiveProperty<float> Money => _money;
        public ReadOnlyReactiveProperty<float> Score => _score;
        public ReadOnlyReactiveProperty<float> ProductionCapacity => _productionCapacity;

        public Account Account { get; private set; }

        public AccountModel(Account account)
        {
            _money.Value = account.Money;
            _score.Value = account.Score;
            _productionCapacity.Value = account.ProductionCapacity;
            Account = account;
        }

        public bool TryChangeMoney(float amount)
        {
            float newValue = _money.Value + amount;
            if (newValue < 0)
            {
                return false;
            }

            _money.Value = newValue;
            Account.Money = newValue;
            return true;
        }

        public void AddScore(float amount)
        {
            _score.Value += amount;
            Account.Score += amount;
        }

        public void AddProductionCapacity(float amount)
        {
            _productionCapacity.Value += amount;
            Account.ProductionCapacity += amount;
        }

        public void Dispose()
        {
            _money?.Dispose();
            _score?.Dispose();
            _productionCapacity?.Dispose();
        }
    }
}