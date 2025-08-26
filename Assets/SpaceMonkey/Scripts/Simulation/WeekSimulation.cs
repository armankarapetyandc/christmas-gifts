using System;
using System.Collections.Generic;
using System.Linq;
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
        public string ProductId { get; private set; }
        public int Quantity { get; private set; }
        public float ProdCapCost { get; private set; }

        public ProductOrder(string productId, float prodCapCost, int quantity)
        {
            ProductId = productId;
            ProdCapCost = prodCapCost;
            Quantity = quantity;
        }
    }

    public class WeekSimulation : IDisposable
    {
        private readonly GameConfig _gameConfig;
        private readonly SimulationInfo _simulationInfo;
        private readonly Account _account;

        public List<Customer> Customers { get; private set; }
        private readonly ReactiveProperty<int> _availableProdCap;

        public ReadOnlyReactiveProperty<int> AvailableProdCap => _availableProdCap;

        public WeekSimulation(GameConfig gameConfig, AccountService accountService)
        {
            _gameConfig = gameConfig;
            _simulationInfo = gameConfig.SimulationInfo;
            _account = accountService.Model.Account;
            _availableProdCap = new ReactiveProperty<int>(_account.GetProductionCapacity());
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
                .Select(p => new ProductOrder(p.Id, p.ProdCapCost!.Value, Random.Range(1, 18) * 2)).ToArray();

            var customer = new Customer(character, mood, orders);
            return customer;
        }

        public void Run()
        {
        }

        public bool TryShipOrder(Customer customer)
        {
            var neededProdCap = Mathf.RoundToInt(customer.Orders.Sum(order => order.ProdCapCost * order.Quantity));
            if (neededProdCap > _availableProdCap.Value)
            {
                return false;
            }

            _availableProdCap.Value -= neededProdCap;
            return true;
        }

        public void Dispose()
        {
        }

        public class Factory : PlaceholderFactory<WeekSimulation>
        {
        }
    }
}