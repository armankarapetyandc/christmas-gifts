using System;
using System.Linq;
using R3;

namespace SpaceMonkey.Scripts.Profile
{
    public class AccountModel : IDisposable
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
            Account = account;
        }

        public bool TryAddAppearedPlace(int placeId)
        {
            if (Account.AppearedPlaces.Contains(placeId))
            {
                return false;
            }

            Account.AppearedPlaces.Add(placeId);
            return true;
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


        public void AddProductionCapacity(float amount)
        {
            _productionCapacity.Value += amount;
        }

        public void Dispose()
        {
            _money?.Dispose();
            _score?.Dispose();
            _productionCapacity?.Dispose();
        }
    }
}