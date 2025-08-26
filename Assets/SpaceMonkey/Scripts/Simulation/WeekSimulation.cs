using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Configs.Characters;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.Utilities;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace SpaceMonkey.Scripts.Simulation
{
    public class Customer
    {
        public CharacterConfig Character { get; private set; }
        public int Mood { get; private set; }
        public ProductOrder[] Orders { get; private set; }

        public Customer(CharacterConfig character, int mood, params ProductOrder[] orders)
        {
            Character = character;
            Mood = mood;
            Orders = orders;
        }
    }

    public class ProductOrder
    {
        public Product Product { get; private set; }
        public int Quantity { get; private set; }

        public ProductOrder(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }
    }

    public class WeekSimulation : IDisposable
    {
        private readonly GameConfig _gameConfig;
        private readonly AccountService _accountService;
        private readonly SimulationInfo _simulationInfo;
        private readonly Account _account;

        public List<Customer> Customers { get; private set; }
        private readonly ReactiveProperty<int> _availableProdCap;
        private readonly ReactiveProperty<float> _money;

        public ReadOnlyReactiveProperty<int> AvailableProdCap => _availableProdCap;
        public ReadOnlyReactiveProperty<float> Money => _money;

        public WeekSimulation(GameConfig gameConfig, AccountService accountService)
        {
            _gameConfig = gameConfig;
            _accountService = accountService;
            _simulationInfo = gameConfig.SimulationInfo;
            _account = accountService.Model.Account;
            _availableProdCap = new ReactiveProperty<int>(_account.GetProductionCapacity());
            _money = new ReactiveProperty<float>(_account.Money);
        }

        public void Prepare()
        {
            var customersCount = Random.Range(_simulationInfo.CustomersMin, _simulationInfo.CustomersMax + 1);
            Customers = new List<Customer>(customersCount);
            var characters = _gameConfig.Characters.PickRandomElements(customersCount);
            for (int i = 0; i < customersCount; i++)
            {
                var customer = PickCustomer(characters[i]);
                Customers.Add(customer);
            }
        }

        private Customer PickCustomer(CharacterConfig character)
        {
            var mood = Random.Range(_simulationInfo.MoodMin, _simulationInfo.MoodMax + 1);
            var productsQuantity = Random.Range(_simulationInfo.OrderQuantityMin, _simulationInfo.OrderQuantityMax);

            var orders = _account.Products.PickRandomElements(productsQuantity)
                .Select(p => new ProductOrder(p, Random.Range(1, 18) * 2)).ToArray();

            var customer = new Customer(character, mood, orders);
            return customer;
        }

        public void Run()
        {
        }

        public bool TryShipOrder(Customer customer)
        {
            var neededProdCap = Mathf.RoundToInt(customer.Orders.Sum(order => order.Product.ProdCapCost!.Value * order.Quantity));
            if (neededProdCap > _availableProdCap.Value)
            {
                return false;
            }

            _availableProdCap.Value -= neededProdCap;
            var profit = customer.Orders.Sum(order => order.Product.Profit * order.Quantity)!.Value;
            _account.Earn(profit);
            return true;
        }

        public void Finish()
        {
            //collect not shipped customers 
            _account.IncreaseWeek();
            _accountService.SaveAsync().Forget();
        }

        public void Dispose()
        {
        }

        public class Factory : PlaceholderFactory<WeekSimulation>
        {
        }
    }
}