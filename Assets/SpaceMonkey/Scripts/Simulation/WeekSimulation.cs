using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Configs.Characters;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.Profile.Simulation;
using SpaceMonkey.Scripts.Simulation.CreditCard;
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
        private readonly CustomerReviewConfig _customerReviewConfig;
        private readonly AccountService _accountService;
        private readonly CreditSimulator _creditSimulator;
        private readonly SimulationInfo _simulationInfo;
        private readonly Account _account;

        private WeekInfo _weekInfo;

        public List<Customer> Customers { get; private set; }
        private readonly ReactiveProperty<int> _availableProdCap;
        private readonly ReactiveProperty<float> _money;

        public ReadOnlyReactiveProperty<int> AvailableProdCap => _availableProdCap;
        public ReadOnlyReactiveProperty<float> Money => _money;

        public WeekInfo WeekInfo => _weekInfo;

        public float SellScore { get; set; }

        public WeekSimulation(GameConfig gameConfig, CustomerReviewConfig customerReviewConfig,
            AccountService accountService, CreditSimulator creditSimulator)
        {
            _gameConfig = gameConfig;
            _customerReviewConfig = customerReviewConfig;
            _accountService = accountService;
            _creditSimulator = creditSimulator;
            _simulationInfo = gameConfig.SimulationInfo;
            _account = accountService.Model.Account;
            _availableProdCap = new ReactiveProperty<int>(_account.GetProductionCapacity());
            _money = new ReactiveProperty<float>(_account.Money);
        }

        public void Prepare()
        {
            var customersCount = _account.Week == 0
                ? Random.Range(_simulationInfo.CustomersMin, _simulationInfo.CustomersMax + 1)
                : Mathf.RoundToInt(Random.Range(_simulationInfo.NewCustomersMin,
                    _simulationInfo.NewCustomersMax + _account.GetMarketingCustAdd()));
            Customers = new List<Customer>(customersCount);
            var characters = _gameConfig.Characters.PickRandomElements(customersCount);
            for (int i = 0; i < customersCount; i++)
            {
                var customer = PickCustomer(characters[i]);
                if (customer == null)
                {
                    continue;
                }

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
                    customer.Orders.Sum(order =>
                        order.Product.ProdCapCost!.Value *
                        _account.Employees.Sum(e => e.Speed * (1f - 0.05f * e.Experience)) * order.Quantity)).Sum()),
                Orders = Customers.Select(customer => new OrderInfo
                {
                    CharacterId = customer.Character.Id,
                    Mood = customer.Mood,
                    Products = customer.Orders.Select(order => new ProductOrderInfo
                    {
                        Product = order.Product,
                        Quantity = order.Quantity
                    }).ToArray()
                }).ToArray()
            };
        }

        private Customer PickCustomer(CharacterConfig character)
        {
            var orderQuantity = Random.Range(_simulationInfo.OrderQuantityMin, _simulationInfo.OrderQuantityMax);

            var orders = _account.Products.PickRandomElements(orderQuantity)
                .Select(p => new ProductOrder(p, DetermineProductQuantity(character.Id, p))).ToArray();

            var mood = orders.Select(order => DetermineCustomerMood(character.Id, order.Product)).Sum();
            mood = Mathf.Clamp(mood, _simulationInfo.MoodMin, _simulationInfo.MoodMax);
            if (mood < _simulationInfo.MoodLeave)
            {
                return null;
            }

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

        private int DetermineCustomerMood(string characterId, Product product)
        {
            if (_account.Weeks.Count == 0)
            {
                return Random.Range(_simulationInfo.MoodMin, _simulationInfo.MoodMax + 1);
            }

            var lastWeek = _account.Weeks.LastOrDefault();
            if (!lastWeek.Orders.Any(o => o.CharacterId.Equals(characterId)))
            {
                return Random.Range(_simulationInfo.MoodMin, _simulationInfo.MoodMax + 1);
            }

            var orderInfo = lastWeek.Orders.FirstOrDefault(order => order.CharacterId.Equals(characterId));
            if (!orderInfo.Products.Any(o => o.Product.Id.Equals(product.Id)))
            {
                return Random.Range(_simulationInfo.MoodMin, _simulationInfo.MoodMax + 1);
            }

            var productOrderInfo = orderInfo.Products.FirstOrDefault(o => o.Product.Id.Equals(product.Id));

            var ttpDeltaPercent =
                ((product.TimeToProduceIndex + 1) - (productOrderInfo.Product.TimeToProduceIndex + 1)) *
                _gameConfig.SimulationInfo.MoodTtpCoefficient;
            var materialDeltaPercent =
                (product.MaterialPrice!.Value - productOrderInfo.Product.MaterialPrice!.Value) *
                _gameConfig.SimulationInfo.MoodMaterialCoefficient;
            var packagingDeltaPercent =
                (product.MaterialPackagingPrice!.Value - productOrderInfo.Product.MaterialPackagingPrice!.Value) *
                _gameConfig.SimulationInfo.MoodPackagingCoefficient;

            var moodDeltaPercent = ttpDeltaPercent + materialDeltaPercent + packagingDeltaPercent;
            var mood = orderInfo.Mood + orderInfo.Mood * moodDeltaPercent;
            return Mathf.Clamp(Mathf.RoundToInt(mood), 1, 100);
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

            var reviews = DetermineCustomerReviewsV2();
            foreach ((Customer, Product, string) tuple in reviews)
            {
                Debug.Log($" {tuple.Item1.Character.Name}, {tuple.Item2.Name}, {tuple.Item3} ");
                _account.Reviews ??= new List<CustomerReviewInfo>();
                _account.Reviews.Add( new CustomerReviewInfo
                {
                    WeekId = _weekInfo.Id,
                    CharacterId = tuple.Item1.Character.Id,
                    ProductId = tuple.Item2.Id,
                    Message = tuple.Item3
                });
            }
            
            _account.PushFinishedWeek(_weekInfo);
            _account.Money = _money.Value;
            _account.Score += SellScore;
            _accountService.SaveAsync().Forget();
            
            _creditSimulator.NextWeek();
        }

        private List<(Customer, Product, string)> DetermineCustomerReviewsV2()
        {
            var reviews = new List<(Customer, Product, string)>();
            var previousWeek = _account.Weeks.LastOrDefault();
            if (previousWeek.Orders == null)
            {
                // No previous week, return an empty list
                return reviews;
            }
            var candidate =
                from customer in Customers
                let canReview = Random.Range(0, 100) <= _gameConfig.SimulationInfo.ReviewChance
                where canReview
                let hasPrevious = previousWeek.Orders.Any(o => o.CharacterId.Equals(customer.Character.Id))
                where hasPrevious
                let currentOrder = _weekInfo.Orders.First(o => o.CharacterId.Equals(customer.Character.Id))
                let previousOrder = previousWeek.Orders.First(o => o.CharacterId.Equals(customer.Character.Id))
                from currentProduct in currentOrder.Products
                let hasPreviousProduct = previousOrder.Products.Any(p => p.Product.Id.Equals(currentProduct.Product.Id))
                where hasPreviousProduct
                let previousProduct = previousOrder.Products.First(p => p.Product.Id.Equals(currentProduct.Product.Id))
                select new
                {
                    Customer = customer,
                    Current = currentProduct,
                    Previous = previousProduct,
                    ProductPriceDelta = (currentProduct.Product.ProductPrice!.Value -
                                         previousProduct.Product.ProductPrice!.Value) /
                                        previousProduct.Product.ProductPrice!.Value,
                    MaterialPackagingPriceDelta =
                        (currentProduct.Product.MaterialPackagingPrice!.Value -
                         previousProduct.Product.MaterialPackagingPrice!.Value) /
                        previousProduct.Product.MaterialPackagingPrice!.Value,
                    MaterialPriceDelta =
                        (currentProduct.Product.MaterialPrice!.Value - previousProduct.Product.MaterialPrice!.Value) /
                        previousProduct.Product.MaterialPrice!.Value,
                    TimeToProduceDelta =
                        ((currentProduct.Product.TimeToProduceIndex + 1) -
                         (previousProduct.Product.TimeToProduceIndex + 1)) /
                        (previousProduct.Product.TimeToProduceIndex + 1)
                };

            foreach (var item in candidate)
            {
                var ttpAbs = Mathf.Abs(item.TimeToProduceDelta);
                var materialPriceAbs = Mathf.Abs(item.MaterialPriceDelta);
                var materialPackagingPriceAbs = Mathf.Abs(item.MaterialPackagingPriceDelta);
                var productPriceAbs = Mathf.Abs(item.ProductPriceDelta);

                var stringBuilder = new StringBuilder();

                if (ttpAbs < _gameConfig.SimulationInfo.ExtremeSettingLow)
                {
                    var review = _customerReviewConfig.Reviews.Where(info => info.Type == ReviewType.ExtremeSlipshodTTP)
                        .PickRandomElement();
                    stringBuilder.AppendLine(review.GetMessage(item.Current.Product.Name));
                }

                if (ttpAbs > _gameConfig.SimulationInfo.ExtremeSettingHigh)
                {
                    var review =
                        _customerReviewConfig.Reviews.Where(info => info.Type == ReviewType.ExtremeDiligentTTP)
                            .PickRandomElement();
                    stringBuilder.AppendLine(review.GetMessage(item.Current.Product.Name));
                }

                if (materialPriceAbs < _gameConfig.SimulationInfo.ExtremeSettingLow)
                {
                    var review =
                        _customerReviewConfig.Reviews.Where(info => info.Type == ReviewType.ExtremeLowMaterial)
                            .PickRandomElement();
                    stringBuilder.AppendLine(review.GetMessage(item.Current.Product.Name));
                }

                if (materialPriceAbs > _gameConfig.SimulationInfo.ExtremeSettingHigh)
                {
                    var review =
                        _customerReviewConfig.Reviews.Where(info => info.Type == ReviewType.ExtremeHighMaterial)
                            .PickRandomElement();
                    stringBuilder.AppendLine(review.GetMessage(item.Current.Product.Name));
                }


                if (materialPackagingPriceAbs < _gameConfig.SimulationInfo.ExtremeSettingLow)
                {
                    var review =
                        _customerReviewConfig.Reviews.Where(info => info.Type == ReviewType.ExtremeLowPackaging)
                            .PickRandomElement();
                    stringBuilder.AppendLine(review.GetMessage(item.Current.Product.Name));
                }

                if (materialPackagingPriceAbs > _gameConfig.SimulationInfo.ExtremeSettingHigh)
                {
                    var review =
                        _customerReviewConfig.Reviews.Where(info => info.Type == ReviewType.ExtremeHighPackaging)
                            .PickRandomElement();
                    stringBuilder.AppendLine(review.GetMessage(item.Current.Product.Name));
                }

                if (productPriceAbs < _gameConfig.SimulationInfo.ExtremeSettingLow)
                {
                    var review =
                        _customerReviewConfig.Reviews.Where(info => info.Type == ReviewType.ExtremeLowPrice)
                            .PickRandomElement();
                    stringBuilder.AppendLine(review.GetMessage(item.Current.Product.Name));
                }

                if (productPriceAbs > _gameConfig.SimulationInfo.ExtremeSettingHigh)
                {
                    var review =
                        _customerReviewConfig.Reviews.Where(info => info.Type == ReviewType.ExtremeHighPrice)
                            .PickRandomElement();
                    stringBuilder.AppendLine(review.GetMessage(item.Current.Product.Name));
                }

                if (ttpAbs > _gameConfig.SimulationInfo.BigProductChange)
                {
                    var review =
                        _customerReviewConfig.Reviews.Where(info => info.Type == ReviewType.BigChangeTTP)
                            .PickRandomElement();
                    stringBuilder.AppendLine(review.GetMessage(item.Current.Product.Name));
                }
                
                if (materialPriceAbs > _gameConfig.SimulationInfo.BigProductChange)
                {
                    var review =
                        _customerReviewConfig.Reviews.Where(info => info.Type == ReviewType.BigChangeMaterial)
                            .PickRandomElement();
                    stringBuilder.AppendLine(review.GetMessage(item.Current.Product.Name));
                }
                
                if (materialPackagingPriceAbs > _gameConfig.SimulationInfo.BigProductChange)
                {
                    var review =
                        _customerReviewConfig.Reviews.Where(info => info.Type == ReviewType.BigChangePackaging)
                            .PickRandomElement();
                    stringBuilder.AppendLine(review.GetMessage(item.Current.Product.Name));
                }

                reviews.Add((item.Customer, item.Current.Product, stringBuilder.ToString()));
            }

            return reviews;
        }

        public void Dispose()
        {
        }

        public class Factory : PlaceholderFactory<WeekSimulation>
        {
        }
    }
}