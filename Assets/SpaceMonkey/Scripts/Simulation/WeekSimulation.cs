using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Configs.Characters;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.Profile.Simulation;
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

        private WeekInfo _weekInfo;

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

            ConfigureWeekInfo();
        }

        private void ConfigureWeekInfo()
        {
            _weekInfo = new WeekInfo
            {
                Id = Guid.NewGuid().ToString(),
                Week = _account.Week,
                NeededCap = Mathf.RoundToInt(Customers.Select(customer =>
                    customer.Orders.Sum(order => order.Product.ProdCapCost!.Value * order.Quantity)).Sum()),
                Orders = Customers.Select(customer => new OrderInfo
                {
                    CharacterId = customer.Character.Id,
                    Mood = customer.Mood,
                    Products = customer.Orders.Select(order => new ProductOrderInfo()
                    {
                        Product = order.Product,
                        Quantity = order.Quantity
                    }).ToArray()
                }).ToArray()
            };
        }

        private Customer PickCustomer(CharacterConfig character)
        {
            var mood = Random.Range(_simulationInfo.MoodMin, _simulationInfo.MoodMax + 1);
            var orderQuantity = Random.Range(_simulationInfo.OrderQuantityMin, _simulationInfo.OrderQuantityMax);

            var orders = _account.Products.PickRandomElements(orderQuantity)
                .Select(p => new ProductOrder(p, DetermineProductQuantity(character.Id, p))).ToArray();

            var customer = new Customer(character, mood, orders);
            return customer;
        }

        private int DetermineProductQuantity(string characterId, Product product)
        {
            if (_account.Weeks.Count == 0)
            {
                return Random.Range(1, 18) * 2;
            }
            
            var lastWeek = _account.Weeks.LastOrDefault();
            if (!lastWeek.Orders.Any(o => o.CharacterId.Equals(characterId)))
            {
                return Random.Range(1, 18) * 2;
            }
            var orderInfo = lastWeek.Orders.FirstOrDefault(order => order.CharacterId.Equals(characterId));
            if (!orderInfo.Products.Any(o => o.Product.Id.Equals(product.Id)))
            {
                return Random.Range(1, 18) * 2;
            }
            var productOrderInfo = orderInfo.Products.FirstOrDefault(o => o.Product.Id.Equals(product.Id));
            var deltaPercent = -((product.ProductPrice!.Value - productOrderInfo.Product.ProductPrice!.Value) /
                productOrderInfo.Product.ProductPrice!.Value * _gameConfig.SimulationInfo.PriceSensitivity);
            var nextOrderQuantity = productOrderInfo.Quantity + productOrderInfo.Quantity * deltaPercent;
            return Mathf.RoundToInt(nextOrderQuantity);
        }

        public Dictionary<Product, int> GetTotalQuantitiesByProduct()
        {
            return _weekInfo.Orders
                .Where(o => o.Shipped)
                .SelectMany(o => o.Products)
                .GroupBy(p => p.Product)
                .ToDictionary(g => g.Key, g => g.Sum(o => o.Quantity));
        }

        public void Run()
        {
        }

        public bool TryShipOrder(Customer customer)
        {
            var neededProdCap =
                Mathf.RoundToInt(customer.Orders.Sum(order => order.Product.ProdCapCost!.Value * order.Quantity));
            if (neededProdCap > _availableProdCap.Value)
            {
                return false;
            }

            _availableProdCap.Value -= neededProdCap;
            var profit = customer.Orders.Sum(order => order.Product.Profit * order.Quantity)!.Value;
            _money.Value += profit;
            
            for (int i = 0; i < _weekInfo.Orders.Length; i++)
            {
                ref OrderInfo orderInfo = ref _weekInfo.Orders[i];
                if (orderInfo.CharacterId.Equals(customer.Character.Id))
                {
                    orderInfo.Shipped = true; // direct update
                    break;
                }
            }

            return true;
        }

        public void Finish()
        {
            //collect not shipped customers
            _account.PushFinishedWeek(_weekInfo);
            _account.Money = _money.Value;
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