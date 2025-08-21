using System;
using System.Collections.Generic;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Configs.Characters;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.Utilities;
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
        public string ProductId { get; private set; }
        public int Quantity { get; private set; }

        public ProductOrder(string productId, int quantity)
        {
            ProductId = productId;
            Quantity = quantity;
        }
    }

    public class WeekSimulation : IDisposable
    {
        private readonly GameConfig _gameConfig;
        private readonly SimulationInfo _simulationInfo;
        private readonly Account _account;

        public List<Customer> Customers { get; private set; }

        public WeekSimulation(GameConfig gameConfig, AccountService accountService)
        {
            _gameConfig = gameConfig;
            _simulationInfo = gameConfig.SimulationInfo;
            _account = accountService.Model.Account;
        }

        public void Prepare()
        {
            Customers = new List<Customer>();
            var customersCount = Random.Range(_simulationInfo.CustomersMin, _simulationInfo.CustomersMax + 1);
            for (int i = 0; i < customersCount; i++)
            {
                var customer = PickCustomer();
                Customers.Add(customer);
            }
        }

        private Customer PickCustomer()
        {
            var character = _gameConfig.Characters.PickRandomElement();
            var mood = Random.Range(_simulationInfo.MoodMin, _simulationInfo.MoodMax + 1);
            var product = _account.Products[0]; //For now we just use for all customers our first product
            var quantity = Random.Range(_simulationInfo.OrderQuantityMin, _simulationInfo.OrderQuantityMax);
            var order = new ProductOrder(product.Id, quantity);

            var customer = new Customer(character, mood, order);
            return customer;
        }

        public void Run()
        {
        }


        public void Dispose()
        {
        }

        public class Factory : PlaceholderFactory<WeekSimulation>
        {
        }
    }
}