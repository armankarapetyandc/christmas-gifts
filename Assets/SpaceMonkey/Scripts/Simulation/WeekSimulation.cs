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
    
    // Process all customers who have current orders
    foreach (var customer in Customers)
    {
        // Check if current week has orders
        if (_weekInfo.Orders == null || _weekInfo.Orders.Length == 0)
            continue;
            
        // Find current order for this customer
        var currentOrderIndex = -1;
        for (int i = 0; i < _weekInfo.Orders.Length; i++)
        {
            if (_weekInfo.Orders[i].CharacterId.Equals(customer.Character.Id))
            {
                currentOrderIndex = i;
                break;
            }
        }
        
        if (currentOrderIndex == -1)
            continue;
            
        var currentOrder = _weekInfo.Orders[currentOrderIndex];
        
        // Check if current order has products
        if (currentOrder.Products == null || currentOrder.Products.Length == 0)
            continue;
            
        foreach (var currentProduct in currentOrder.Products)
        {
            var stringBuilder = new StringBuilder();
            bool hasReview = false;
            
            // Try to find previous order and product for comparison
            var hasPreviousProduct = false;
            ProductOrderInfo previousProduct = default(ProductOrderInfo);
            
            // Check if previous week exists and has orders
            if (previousWeek.Orders != null && previousWeek.Orders.Length > 0)
            {
                // Find previous order for this customer
                for (int i = 0; i < previousWeek.Orders.Length; i++)
                {
                    if (previousWeek.Orders[i].CharacterId.Equals(customer.Character.Id))
                    {
                        var previousOrder = previousWeek.Orders[i];
                        
                        // Check if previous order has products
                        if (previousOrder.Products != null && previousOrder.Products.Length > 0)
                        {
                            // Find matching product in previous order
                            for (int j = 0; j < previousOrder.Products.Length; j++)
                            {
                                if (previousOrder.Products[j].Product.Id.Equals(currentProduct.Product.Id))
                                {
                                    previousProduct = previousOrder.Products[j];
                                    hasPreviousProduct = true;
                                    break;
                                }
                            }
                        }
                        break;
                    }
                }
            }
            
            // If we have a previous product to compare with
            if (hasPreviousProduct && previousProduct.Product.ProductPrice != null && previousProduct.Product.ProductPrice.Value > 0)
            {
                // Calculate deltas for comparison
                var productPriceDelta = (currentProduct.Product.ProductPrice!.Value -
                                         previousProduct.Product.ProductPrice!.Value) /
                                        previousProduct.Product.ProductPrice!.Value;
                var materialPackagingPriceDelta =
                    (currentProduct.Product.MaterialPackagingPrice!.Value -
                     previousProduct.Product.MaterialPackagingPrice!.Value) /
                    previousProduct.Product.MaterialPackagingPrice!.Value;
                var materialPriceDelta =
                    (currentProduct.Product.MaterialPrice!.Value - previousProduct.Product.MaterialPrice!.Value) /
                    previousProduct.Product.MaterialPrice!.Value;
                var timeToProduceDelta =
                    ((currentProduct.Product.TimeToProduceIndex + 1.0f) -
                     (previousProduct.Product.TimeToProduceIndex + 1.0f)) /
                    (previousProduct.Product.TimeToProduceIndex + 1.0f);

                var ttpAbs = Mathf.Abs(timeToProduceDelta);
                var materialPriceAbs = Mathf.Abs(materialPriceDelta);
                var materialPackagingPriceAbs = Mathf.Abs(materialPackagingPriceDelta);
                var productPriceAbs = Mathf.Abs(productPriceDelta);

                // Check extreme settings
                if (ttpAbs < _gameConfig.SimulationInfo.ExtremeSettingLow)
                {
                    var reviewList = _customerReviewConfig.Reviews.Where(info => info.Type == ReviewType.ExtremeSlipshodTTP).ToList();
                    if (reviewList.Any())
                    {
                        stringBuilder.AppendLine(reviewList.PickRandomElement().GetMessage(currentProduct.Product.Name));
                        hasReview = true;
                    }
                }

                if (ttpAbs > _gameConfig.SimulationInfo.ExtremeSettingHigh)
                {
                    var reviewList = _customerReviewConfig.Reviews.Where(info => info.Type == ReviewType.ExtremeDiligentTTP).ToList();
                    if (reviewList.Any())
                    {
                        stringBuilder.AppendLine(reviewList.PickRandomElement().GetMessage(currentProduct.Product.Name));
                        hasReview = true;
                    }
                }

                if (materialPriceAbs < _gameConfig.SimulationInfo.ExtremeSettingLow)
                {
                    var reviewList = _customerReviewConfig.Reviews.Where(info => info.Type == ReviewType.ExtremeLowMaterial).ToList();
                    if (reviewList.Any())
                    {
                        stringBuilder.AppendLine(reviewList.PickRandomElement().GetMessage(currentProduct.Product.Name));
                        hasReview = true;
                    }
                }

                if (materialPriceAbs > _gameConfig.SimulationInfo.ExtremeSettingHigh)
                {
                    var reviewList = _customerReviewConfig.Reviews.Where(info => info.Type == ReviewType.ExtremeHighMaterial).ToList();
                    if (reviewList.Any())
                    {
                        stringBuilder.AppendLine(reviewList.PickRandomElement().GetMessage(currentProduct.Product.Name));
                        hasReview = true;
                    }
                }

                if (materialPackagingPriceAbs < _gameConfig.SimulationInfo.ExtremeSettingLow)
                {
                    var reviewList = _customerReviewConfig.Reviews.Where(info => info.Type == ReviewType.ExtremeLowPackaging).ToList();
                    if (reviewList.Any())
                    {
                        stringBuilder.AppendLine(reviewList.PickRandomElement().GetMessage(currentProduct.Product.Name));
                        hasReview = true;
                    }
                }

                if (materialPackagingPriceAbs > _gameConfig.SimulationInfo.ExtremeSettingHigh)
                {
                    var reviewList = _customerReviewConfig.Reviews.Where(info => info.Type == ReviewType.ExtremeHighPackaging).ToList();
                    if (reviewList.Any())
                    {
                        stringBuilder.AppendLine(reviewList.PickRandomElement().GetMessage(currentProduct.Product.Name));
                        hasReview = true;
                    }
                }

                if (productPriceAbs < _gameConfig.SimulationInfo.ExtremeSettingLow)
                {
                    var reviewList = _customerReviewConfig.Reviews.Where(info => info.Type == ReviewType.ExtremeLowPrice).ToList();
                    if (reviewList.Any())
                    {
                        stringBuilder.AppendLine(reviewList.PickRandomElement().GetMessage(currentProduct.Product.Name));
                        hasReview = true;
                    }
                }

                if (productPriceAbs > _gameConfig.SimulationInfo.ExtremeSettingHigh)
                {
                    var reviewList = _customerReviewConfig.Reviews.Where(info => info.Type == ReviewType.ExtremeHighPrice).ToList();
                    if (reviewList.Any())
                    {
                        stringBuilder.AppendLine(reviewList.PickRandomElement().GetMessage(currentProduct.Product.Name));
                        hasReview = true;
                    }
                }

                // Check for big changes
                if (ttpAbs > _gameConfig.SimulationInfo.BigProductChange)
                {
                    var reviewList = _customerReviewConfig.Reviews.Where(info => info.Type == ReviewType.BigChangeTTP).ToList();
                    if (reviewList.Any())
                    {
                        stringBuilder.AppendLine(reviewList.PickRandomElement().GetMessage(currentProduct.Product.Name));
                        hasReview = true;
                    }
                }
                
                if (materialPriceAbs > _gameConfig.SimulationInfo.BigProductChange)
                {
                    var reviewList = _customerReviewConfig.Reviews.Where(info => info.Type == ReviewType.BigChangeMaterial).ToList();
                    if (reviewList.Any())
                    {
                        stringBuilder.AppendLine(reviewList.PickRandomElement().GetMessage(currentProduct.Product.Name));
                        hasReview = true;
                    }
                }
                
                if (materialPackagingPriceAbs > _gameConfig.SimulationInfo.BigProductChange)
                {
                    var reviewList = _customerReviewConfig.Reviews.Where(info => info.Type == ReviewType.BigChangePackaging).ToList();
                    if (reviewList.Any())
                    {
                        stringBuilder.AppendLine(reviewList.PickRandomElement().GetMessage(currentProduct.Product.Name));
                        hasReview = true;
                    }
                }
            }
            
            // If no review was generated (no previous product or no deltas exceeded thresholds),
            // generate a review based on current product characteristics
            if (!hasReview)
            {
                // Generate review based on absolute values of current product
                if (currentProduct.Product.ProductPrice != null)
                {
                    // Check if price is particularly high or low (you may need to define base thresholds)
                    var basePrice = 100.0; // Adjust this based on your game's economy
                    var priceRatio = currentProduct.Product.ProductPrice.Value / basePrice;
                    
                    if (priceRatio > 2.0) // Price is more than double the base
                    {
                        var reviewList = _customerReviewConfig.Reviews.Where(info => info.Type == ReviewType.ExtremeHighPrice).ToList();
                        if (reviewList.Any())
                        {
                            stringBuilder.AppendLine(reviewList.PickRandomElement().GetMessage(currentProduct.Product.Name));
                            hasReview = true;
                        }
                    }
                    else if (priceRatio < 0.5) // Price is less than half the base
                    {
                        var reviewList = _customerReviewConfig.Reviews.Where(info => info.Type == ReviewType.ExtremeLowPrice).ToList();
                        if (reviewList.Any())
                        {
                            stringBuilder.AppendLine(reviewList.PickRandomElement().GetMessage(currentProduct.Product.Name));
                            hasReview = true;
                        }
                    }
                }
                
                // If still no review, pick a random review type based on product characteristics
                if (!hasReview)
                {
                    var allReviewTypes = new List<ReviewType>();
                    
                    // Build a list of applicable review types based on product stats
                    if (currentProduct.Product.TimeToProduceIndex < 2)
                        allReviewTypes.Add(ReviewType.ExtremeSlipshodTTP);
                    else if (currentProduct.Product.TimeToProduceIndex > 5)
                        allReviewTypes.Add(ReviewType.ExtremeDiligentTTP);
                    
                    if (currentProduct.Product.MaterialPrice != null)
                    {
                        if (currentProduct.Product.MaterialPrice < 50)
                            allReviewTypes.Add(ReviewType.ExtremeLowMaterial);
                        else if (currentProduct.Product.MaterialPrice > 200)
                            allReviewTypes.Add(ReviewType.ExtremeHighMaterial);
                    }
                    
                    if (currentProduct.Product.MaterialPackagingPrice != null)
                    {
                        if (currentProduct.Product.MaterialPackagingPrice < 10)
                            allReviewTypes.Add(ReviewType.ExtremeLowPackaging);
                        else if (currentProduct.Product.MaterialPackagingPrice > 50)
                            allReviewTypes.Add(ReviewType.ExtremeHighPackaging);
                    }
                    
                    // If we have applicable types, pick one randomly
                    if (allReviewTypes.Any())
                    {
                        var selectedType = allReviewTypes.PickRandomElement();
                        var reviewList = _customerReviewConfig.Reviews.Where(info => info.Type == selectedType).ToList();
                        if (reviewList.Any())
                        {
                            stringBuilder.AppendLine(reviewList.PickRandomElement().GetMessage(currentProduct.Product.Name));
                            hasReview = true;
                        }
                    }
                    
                    // Final fallback: pick any available review
                    if (!hasReview && _customerReviewConfig.Reviews.Any())
                    {
                        var anyReview = _customerReviewConfig.Reviews.PickRandomElement();
                        stringBuilder.AppendLine(anyReview.GetMessage(currentProduct.Product.Name));
                    }
                }
            }
            
            // Add the review if we have any content
            var reviewText = stringBuilder.ToString().Trim();
            if (!string.IsNullOrEmpty(reviewText))
            {
                reviews.Add((customer, currentProduct.Product, reviewText));
            }
        }
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