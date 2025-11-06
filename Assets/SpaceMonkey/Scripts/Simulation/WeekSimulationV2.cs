using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using R3;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Configs.Characters;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.Profile.Simulation;
using SpaceMonkey.Scripts.Simulation.CreditCard;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;
using ReviewType = SpaceMonkey.Scripts.Profile.Simulation.ReviewType;

namespace SpaceMonkey.Scripts.Simulation
{
    public class WeekSimulationV2
    {
        private readonly AccountService _accountService;
        private readonly GameConfig _gameConfig;
        private readonly CreditSimulator _creditSimulator;
        private readonly SimulationInfo _simulationInfo;
        private Account Account => _accountService.Model.Account;

        [Serializable]
        public struct CustomerData : IEquatable<CustomerData>
        {
            public string CharacterId { get; set; }
            public int Mood { get; set; }
            public int Frequency { get; set; }
            public int LastAppearanceWeek { get; set; }
            public bool Active { get; set; }

            // Implement IEquatable<CustomerData>
            public bool Equals(CustomerData other)
            {
                // Only compare CharacterId since it's the unique identifier
                return CharacterId == other.CharacterId;
            }

            // Override Equals for object comparison
            public override bool Equals(object obj)
            {
                if (obj is CustomerData other)
                    return Equals(other);
                return false;
            }

            // Override GetHashCode to be consistent with Equals
            public override int GetHashCode()
            {
                // Return hash of CharacterId, or 0 if null
                return CharacterId?.GetHashCode() ?? 0;
            }

            // Optional: Add equality operators for convenience
            public static bool operator ==(CustomerData left, CustomerData right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(CustomerData left, CustomerData right)
            {
                return !left.Equals(right);
            }
        }

        [Serializable]
        public struct OrderEntry
        {
            public Product Product { get; set; }
            public int Quantity { get; set; }
            public bool Ship { get; set; } // Indicates if this entry was shipped

            [JsonIgnore] public float OrderProfit => Product.Profit!.Value * Quantity;
            [JsonIgnore] public float OrderProdCost => Product.ProdCapCost!.Value * Quantity;
        }

        [Serializable]
        public struct Order
        {
            public CustomerData Customer { get; set; }
            public List<OrderEntry> OrderEntries { get; set; }
            public bool WasFulfilled { get; set; }
        }

        [Serializable]
        public struct Week
        {
            public string Id { get; set; }
            public int WeekNumber { get; set; }
            public List<Order> Orders { get; set; }
        }

        public Week? CurrentWeek { get; private set; }
        private List<CustomerData> allCustomers;
        private readonly ReactiveProperty<float> _availableProdCap;
        private readonly ReactiveProperty<float> _money;
        public ReadOnlyReactiveProperty<float> AvailableProdCap => _availableProdCap;
        public ReadOnlyReactiveProperty<float> Money => _money;

        public float SellScore { get; set; }

        public WeekSimulationV2(AccountService accountService, GameConfig gameConfig, CreditSimulator creditSimulator)
        {
            _accountService = accountService;
            _gameConfig = gameConfig;
            _creditSimulator = creditSimulator;
            _simulationInfo = gameConfig.SimulationInfo;
            _availableProdCap = new ReactiveProperty<float>(accountService.Model.Account.GetProductionCapacity());
            _money = new ReactiveProperty<float>(accountService.Model.Account.Money);
        }


        public void Initialize()
        {
            if (Account.AllCustomers == null || Account.AllCustomers.Count == 0)
            {
                allCustomers = new List<CustomerData>();
                foreach (var character in _gameConfig.Characters.Where(c =>
                             c.Appearance.HasFlag(CharacterAppearance.Customer)))
                {
                    var customerData = new CustomerData
                    {
                        CharacterId = character.Id,
                        Mood = Random.Range(_simulationInfo.MoodMin, _simulationInfo.MoodMax),
                        Frequency = Random.Range(_simulationInfo.FrequencyMin, _simulationInfo.FrequencyMax),
                        LastAppearanceWeek = 0,
                        Active = true
                    };
                    allCustomers.Add(customerData);
                }

                Debug.Log($"Initialized all customers");
                return;
            }

            allCustomers = Account.AllCustomers.Select(c => c).ToList();
        }

        public void StartNewWeek(int weekNumber)
        {
            Debug.Log($"\n=== STARTING WEEK {weekNumber} ===");

            // STEP 1: Generate who will visit this week (based on frequency/random)
            var visitingCustomers = GenerateWeekCustomers(weekNumber);

            // STEP 2: Adjust moods ONLY for customers who are visiting (skip week 1)
            if (weekNumber > 1 && visitingCustomers.Count > 0)
            {
                visitingCustomers = AdjustVisitingCustomerMoods(visitingCustomers, weekNumber - 1);
            }
            else if (weekNumber == 1)
            {
                Debug.Log("Week 1: No mood adjustments (no previous orders)");
            }

            CurrentWeek = new Week
            {
                Id = Guid.NewGuid().ToString(),
                WeekNumber = weekNumber,
                Orders = new List<Order>()
            };

            foreach (var customer in visitingCustomers)
            {
                var orderEntries = GenerateOrderEntries(customer);
                if (orderEntries.Count == 0)
                {
                    continue;
                }

                CurrentWeek.Value.Orders.Add(new Order
                {
                    Customer = customer,
                    OrderEntries = orderEntries,
                    WasFulfilled = false // default, to be updated later via TryShipOrder
                });
            }

            Debug.Log($"Prepared {CurrentWeek.Value.Orders.Count} orders for Week {weekNumber}");
        }

        private List<OrderEntry> GenerateOrderEntries(CustomerData customer)
        {
            var productList = Account.Products;
            if (productList == null || productList.Count == 0)
                return new List<OrderEntry>();

            // Always start with one product
            var selectedProduct = productList[Random.Range(0, productList.Count)];

            // Initial quantity
            int baseQuantity = Random.Range(_simulationInfo.OrderQuantityMin, _simulationInfo.OrderQuantityMax + 1);
            int finalQuantity = baseQuantity;

            // Mood-based adjustment
            if (customer.Mood < _simulationInfo.MoodLeave)
            {
                // Customer will leave, no order
                return new List<OrderEntry>();
            }

            if (customer.Mood <= _simulationInfo.MoodLow)
            {
                float percent = Random.Range(_simulationInfo.OrderQuantReduceLow,
                    _simulationInfo.OrderQuantReduceHigh + 1) / 100f;
                finalQuantity = Mathf.Max(Mathf.FloorToInt(baseQuantity * (1f - percent)), 0);
            }
            else if (customer.Mood <= _simulationInfo.MoodMedium)
            {
                float percent = Random.Range(_simulationInfo.OrderQuantIncreaseMediumLow,
                    _simulationInfo.OrderQuantIncreaseMediumHigh + 1) / 100f;
                finalQuantity = Mathf.FloorToInt(baseQuantity * (1f + percent));
            }
            else if (customer.Mood <= _simulationInfo.MoodHigh)
            {
                float percent = Random.Range(_simulationInfo.OrderQuantIncreaseLow,
                    _simulationInfo.OrderQuantIncreaseHigh + 1) / 100f;
                finalQuantity = Mathf.FloorToInt(baseQuantity * (1f + percent));
            }

            if (finalQuantity < 1)
                return new List<OrderEntry>();

            var entries = new List<OrderEntry>
            {
                new OrderEntry
                {
                    Product = selectedProduct,
                    Quantity = finalQuantity
                }
            };

            // Chance to add an extra product
            bool addExtra = false;
            if (customer.Mood > _simulationInfo.MoodMedium)
                addExtra = Random.value < 0.5f;
            else if (customer.Mood > _simulationInfo.MoodLow)
                addExtra = Random.value < 0.35f;

            if (addExtra && productList.Count > 1)
            {
                var extraProduct = productList
                    .Where(p => p != selectedProduct)
                    .OrderBy(_ => Guid.NewGuid())
                    .FirstOrDefault();

                if (extraProduct.Id != null)
                {
                    int extraQuantity = Random.Range(_simulationInfo.OrderQuantityMin,
                        _simulationInfo.OrderQuantityMax + 1);
                    entries.Add(new OrderEntry
                    {
                        Product = extraProduct,
                        Quantity = extraQuantity
                    });
                }
            }

            return entries;
        }

        public void FinishWeek()
        {
            if (CurrentWeek == null)
            {
                Debug.LogWarning("No current week to finish.");
                return;
            }

            if (Account.WeeksV2 == null)
                Account.WeeksV2 = new List<Week>();

            Account.WeeksV2.Add(CurrentWeek.Value);
            Account.AllCustomers = allCustomers;
            Account.Reviews = ProcessCustomerReviews();
        
            Debug.Log($"Finished Week {CurrentWeek.Value.WeekNumber} and saved to account.");
            allCustomers = null;

            _creditSimulator.NextWeek();
            _accountService.SaveAsync().Forget();
        }


        public void GrantReward()
        {
            Account.Money += _money.Value;
            Account.Score += SellScore;
        }
        

        /// <summary>
        /// Generate which customers will visit this week (BEFORE mood adjustments)
        /// </summary>
        private List<CustomerData> GenerateWeekCustomers(int weekNumber)
        {
            var activeCustomers = allCustomers.Where(c => c.Active).ToList();

            if (activeCustomers.Count == 0)
            {
                Debug.Log("GAME OVER: No active customers remaining!");
                Account.IsGameOver = true;
                return new List<CustomerData>();
            }

            List<CustomerData> selectedCustomers;

            int baseCount = Random.Range(_simulationInfo.CustomersMin, _simulationInfo.CustomersMax + 1);
            // Week 1: Just base random customers
            if (weekNumber == 1)
            {
                selectedCustomers = activeCustomers
                    .OrderBy(_ => Guid.NewGuid())
                    .Take(baseCount)
                    .ToList();

                Debug.Log($"Week 1: Selected {baseCount} random customers");
            }
            else
            {
                // Week 2+: Frequency-based returns + new customers + marketing
                var returningCustomers = activeCustomers
                    .Where(c => ShouldReturnThisWeek(c, weekNumber))
                    .ToList();

                int marketingBonus = Account.GetMarketingCustAdd();
                int totalNeeded = Mathf.Min(
                    baseCount + marketingBonus + returningCustomers.Count,
                    activeCustomers.Count
                );

                var additionalCustomers = activeCustomers
                    .Except(returningCustomers)
                    .OrderByDescending(c => c.Mood)
                    .Take(totalNeeded - returningCustomers.Count)
                    .ToList();

                selectedCustomers = returningCustomers.Concat(additionalCustomers).ToList();

                Debug.Log(
                    $"Selected customers: {returningCustomers.Count} returning + {additionalCustomers.Count} new = {selectedCustomers.Count} total");
                if (marketingBonus > 0)
                    Debug.Log($"  (Marketing bonus: +{marketingBonus} slots)");
            }

            // Update last appearance for selected customers
            foreach (var customer in selectedCustomers)
            {
                var idx = allCustomers.FindIndex(c => c.CharacterId == customer.CharacterId);
                if (idx >= 0)
                {
                    // Create a copy of the struct
                    var updatedCustomer = allCustomers[idx];
                    // Modify the copy
                    updatedCustomer.LastAppearanceWeek = weekNumber;
                    // Replace the original with the modified copy
                    allCustomers[idx] = updatedCustomer;
                }
            }

            return selectedCustomers;
        }

        /// <summary>
        /// Adjust moods ONLY for customers who are visiting this week
        /// Returns updated list (some customers might leave if mood drops too low)
        /// </summary>
        private List<CustomerData> AdjustVisitingCustomerMoods(List<CustomerData> visitingCustomers,
            int previousWeekNumber)
        {
            Debug.Log($"\n--- Adjusting moods for {visitingCustomers.Count} visiting customers ---");

            var previousWeek = Account.WeeksV2?.FirstOrDefault(w => w.WeekNumber == previousWeekNumber);
            var updatedVisitors = new List<CustomerData>();

            foreach (var visitor in visitingCustomers)
            {
                var customerIndex = allCustomers.FindIndex(c => c.CharacterId == visitor.CharacterId);
                if (customerIndex < 0) continue;

                var customer = allCustomers[customerIndex];
                float moodDelta = 0;

                // Check if this customer had an order last week
                if (previousWeek?.Orders != null)
                {
                    var lastWeekOrder =
                        previousWeek.Value.Orders.FirstOrDefault(o => o.Customer.CharacterId == customer.CharacterId);

                    if (lastWeekOrder.OrderEntries != null && lastWeekOrder.OrderEntries.Count > 0)
                    {
                        // This customer ordered last week, calculate mood change
                        var currentProducts = lastWeekOrder.OrderEntries.Select(e => e.Product).ToList();
                        var previousProducts =
                            GetCustomerPreviousProducts(customer.CharacterId, previousWeekNumber - 1);

                        if (previousProducts != null && previousProducts.Count > 0)
                        {
                            // Compare products
                            var currentMain = currentProducts.OrderByDescending(p => p.ProductPrice ?? 0).First();
                            var previousMain = previousProducts.OrderByDescending(p => p.ProductPrice ?? 0).First();

                            float ttpDelta = (currentMain.TimeToProduceIndex - previousMain.TimeToProduceIndex) *
                                             _simulationInfo.MoodTtpCoefficient;

                            float matDelta = 0;
                            if (currentMain.MaterialPrice.HasValue && previousMain.MaterialPrice.HasValue)
                            {
                                matDelta = (previousMain.MaterialPrice.Value - currentMain.MaterialPrice.Value) /
                                           _simulationInfo.MoodMaterialCoefficient;
                            }

                            float packDelta = 0;
                            if (currentMain.MaterialPackagingPrice.HasValue &&
                                previousMain.MaterialPackagingPrice.HasValue)
                            {
                                packDelta = (previousMain.MaterialPackagingPrice.Value -
                                             currentMain.MaterialPackagingPrice.Value) /
                                            _simulationInfo.MoodPackagingCoefficient;
                            }

                            moodDelta = ttpDelta + matDelta + packDelta;
                            Debug.Log(
                                $"  {customer.CharacterId}: Product changes TTP={ttpDelta:F1}, Mat={matDelta:F1}, Pack={packDelta:F1}");
                        }

                        // Apply fulfillment bonus/penalty
                        if (lastWeekOrder.WasFulfilled)
                        {
                            moodDelta += _simulationInfo.MoodOrderFulfillmentCoefficient;
                            Debug.Log(
                                $"  {customer.CharacterId}: Last order fulfilled! +{_simulationInfo.MoodOrderFulfillmentCoefficient}");
                        }
                        else
                        {
                            moodDelta += _simulationInfo.MoodOrderNotFulfillmentCoefficient;
                            Debug.Log(
                                $"  {customer.CharacterId}: Last order NOT fulfilled! {_simulationInfo.MoodOrderNotFulfillmentCoefficient}");
                        }
                    }
                    else
                    {
                        Debug.Log($"  {customer.CharacterId}: Visiting but didn't order last week, no mood change");
                    }
                }

                // Apply mood change
                int oldMood = customer.Mood;
                customer.Mood = Mathf.Clamp(customer.Mood + Mathf.RoundToInt(moodDelta), 0, 100);
                allCustomers[customerIndex] = customer;

                if (moodDelta != 0)
                {
                    Debug.Log($"  {customer.CharacterId}: Mood {oldMood} → {customer.Mood} (Δ{moodDelta:F1})");
                }

                // Check if customer leaves DURING their visit
                if (customer.Mood < _simulationInfo.MoodLeave)
                {
                    customer.Active = false;
                    allCustomers[customerIndex] = customer;
                    Debug.Log(
                        $"  WARNING: {customer.CharacterId} left during visit (mood {customer.Mood} < {_simulationInfo.MoodLeave})!");
                    // They leave but still appear this week (their last week)
                }

                // Add to updated visitors list (even if they're leaving, they're here this week)
                updatedVisitors.Add(customer);
            }

            return updatedVisitors;
        }

        /// <summary>
        /// Get products a customer ordered in a specific week
        /// </summary>
        private List<Product> GetCustomerPreviousProducts(string customerId, int weekNumber)
        {
            if (Account.WeeksV2 == null || weekNumber < 1) return null;
            var week = Account.WeeksV2.FirstOrDefault(w => w.WeekNumber == weekNumber);
            var customerOrder = week.Orders?.FirstOrDefault(o => o.Customer.CharacterId == customerId);
            return customerOrder?.OrderEntries?.Select(e => e.Product).ToList();
        }

        /// <summary>
        /// Determine if customer returns based on frequency
        /// </summary>
        private bool ShouldReturnThisWeek(CustomerData c, int weekNumber)
        {
            if (!c.Active || c.Mood < _simulationInfo.MoodLeave)
                return false;

            int period = 4;
            int appearances = Mathf.Clamp(c.Frequency, 1, 4);
            int weekInCycle = (weekNumber - 1) % period;

            double interval = (double)period / appearances;
            for (int i = 0; i < appearances; i++)
            {
                int scheduledWeek = (int)Math.Round(i * interval);
                if (scheduledWeek == weekInCycle)
                    return true;
            }

            return false;
        }

        public bool TryShipOrder(string customerId)
        {
            var orderIndex = CurrentWeek.Value.Orders.FindIndex(o => o.Customer.CharacterId == customerId);
            if (orderIndex < 0)
            {
                Debug.LogWarning($"No order found for customer {customerId}.");
                return false;
            }

            var order = CurrentWeek.Value.Orders[orderIndex];
            float totalProdCost = order.OrderEntries.Sum(e => e.OrderProdCost);

            bool canFulfill = _availableProdCap.Value >= totalProdCost;
            order.WasFulfilled = canFulfill;
            CurrentWeek.Value.Orders[orderIndex] = order;
            if (canFulfill)
            {
                _availableProdCap.Value -= totalProdCost;
                _money.Value += order.OrderEntries.Sum(e => e.OrderProfit);
            }

            Debug.Log(
                $"{(canFulfill ? "Fulfilled" : "NOT fulfilled")} order for {customerId}. Cost: {totalProdCost:F2}");
            return canFulfill;
        }

        private List<CustomerReviewInfo> ProcessCustomerReviews()
        {
            if (CurrentWeek == null || CurrentWeek.Value.Orders == null) return new List<CustomerReviewInfo>();

            var reviews = new List<CustomerReviewInfo>();

            foreach (var order in CurrentWeek.Value.Orders)
            {
                // Skip customers who didn't order or order wasn't fulfilled
                if (!order.WasFulfilled || order.OrderEntries == null || order.OrderEntries.Count == 0)
                    continue;

                // Step 0: Check if customer will consider leaving a review
                if (Random.value > _simulationInfo.ReviewChance / 100f)
                    continue;

                // Process each product the customer ordered
                foreach (var entry in order.OrderEntries)
                {
                    var review = GenerateReviewForProduct(order.Customer, entry.Product, CurrentWeek.Value.WeekNumber);
                    if (review != null)
                    {
                        reviews.Add(review.Value);
                        Debug.Log(
                            $"Review generated: {order.Customer.CharacterId} - {review.Value.Type} - {review.Value.StarRating} stars");
                    }
                }
            }

            return reviews;
        }

        /// <summary>
        /// Generate a review for a specific product if conditions are met
        /// </summary>
        private CustomerReviewInfo? GenerateReviewForProduct(CustomerData customer, Product currentProduct,
            int weekNumber)
        {
            CustomerReviewInfo? review = null;

            // Step 1: Check for Big Change review
            var previousProduct = GetCustomerLastProductOrder(customer.CharacterId, currentProduct.Id, weekNumber);
            if (previousProduct != null)
            {
                review = CheckBigChangeReview(customer, previousProduct.Value, currentProduct);
            }

            // Step 2: If no big change review, check for Extreme Setting review
            if (review == null)
            {
                review = CheckExtremeSettingReview(customer, currentProduct);
            }

            // If we have a review, add star rating based on mood
            if (review != null)
            {
                var finalReview = review.Value;
                finalReview.WeekId = CurrentWeek.Value.Id;
                finalReview.StarRating = CalculateStarRating(customer.Mood);
                return finalReview;
            }

            return null;
        }

        /// <summary>
        /// Get the last time this customer ordered this specific product
        /// </summary>
        private Product? GetCustomerLastProductOrder(string customerId, string productId, int currentWeek)
        {
            if (Account.WeeksV2 == null) return null;

            // Search backwards through weeks
            for (int week = currentWeek - 1; week >= 1; week--)
            {
                var weekData = Account.WeeksV2.FirstOrDefault(w => w.WeekNumber == week);
                var order = weekData.Orders?.FirstOrDefault(o => o.Customer.CharacterId == customerId);

                if (order?.OrderEntries != null)
                {
                    var productEntry = order.Value.OrderEntries.FirstOrDefault(e => e.Product.Id == productId);
                    if (productEntry.Product.Id != null)
                        return productEntry.Product;
                }
            }

            return null;
        }

        /// <summary>
        /// Calculate star rating based on customer mood
        /// </summary>
        private int CalculateStarRating(int mood)
        {
            // Based on mood ranges from CSV
            if (mood >= 80) return 5; // Happy
            if (mood >= 60) return 4;
            if (mood >= 40) return 3;
            if (mood >= 20) return 2;
            return 1; // Sad
        }

        /// <summary>
        /// Check if product changes warrant a Big Change review
        /// </summary>
        private CustomerReviewInfo? CheckBigChangeReview(CustomerData customer, Product oldProduct, Product newProduct)
        {
            float priceDelta = 0, packDelta = 0, matDelta = 0, ttpDelta = 0;

            // Calculate % changes
            if (oldProduct.ProductPrice.HasValue && newProduct.ProductPrice.HasValue &&
                oldProduct.ProductPrice.Value > 0)
            {
                priceDelta = (newProduct.ProductPrice.Value - oldProduct.ProductPrice.Value) /
                    oldProduct.ProductPrice.Value * 100;
            }

            if (oldProduct.MaterialPackagingPrice.HasValue && newProduct.MaterialPackagingPrice.HasValue &&
                oldProduct.MaterialPackagingPrice.Value > 0)
            {
                packDelta = (newProduct.MaterialPackagingPrice.Value - oldProduct.MaterialPackagingPrice.Value) /
                    oldProduct.MaterialPackagingPrice.Value * 100;
            }

            if (oldProduct.MaterialPrice.HasValue && newProduct.MaterialPrice.HasValue &&
                oldProduct.MaterialPrice.Value > 0)
            {
                matDelta = (newProduct.MaterialPrice.Value - oldProduct.MaterialPrice.Value) /
                    oldProduct.MaterialPrice.Value * 100;
            }

            // TTP is just difference, not percentage
            ttpDelta = newProduct.TimeToProduceIndex - oldProduct.TimeToProduceIndex;

            // Check if any delta exceeds bigProductChange threshold (40%)
            bool bigChange = Mathf.Abs(priceDelta) > _simulationInfo.BigProductChange ||
                             Mathf.Abs(packDelta) > _simulationInfo.BigProductChange ||
                             Mathf.Abs(matDelta) > _simulationInfo.BigProductChange ||
                             Mathf.Abs(ttpDelta) > _simulationInfo.BigProductChange;

            if (bigChange)
            {
                // Determine which change was biggest to pick appropriate review message
                string triggerReason = "";
                if (Mathf.Abs(priceDelta) > _simulationInfo.BigProductChange)
                    triggerReason = $"Price {priceDelta:+0;-0}%";
                else if (Mathf.Abs(packDelta) > _simulationInfo.BigProductChange)
                    triggerReason = $"Packaging {packDelta:+0;-0}%";
                else if (Mathf.Abs(matDelta) > _simulationInfo.BigProductChange)
                    triggerReason = $"Materials {matDelta:+0;-0}%";
                else if (Mathf.Abs(ttpDelta) > _simulationInfo.BigProductChange)
                    triggerReason = $"Production Time {ttpDelta:+0;-0}";

                return new CustomerReviewInfo
                {
                    CharacterId = customer.CharacterId,
                    ProductId = newProduct.Id,
                    Type = ReviewType.BigChange,
                    TriggerReason = triggerReason,
                    Message = GetBigChangeReviewMessage(priceDelta, packDelta, matDelta, ttpDelta)
                };
            }

            return null;
        }

        /// <summary>
        /// Get review message for big changes, selecting based on what changed most
        /// </summary>
        private string GetBigChangeReviewMessage(float priceDelta, float packDelta, float matDelta, float ttpDelta)
        {
            // Determine the dominant change
            float maxAbsDelta = 0;
            string changeType = "";
            bool isPositive = false;

            if (Mathf.Abs(matDelta) > maxAbsDelta)
            {
                maxAbsDelta = Mathf.Abs(matDelta);
                changeType = "Material";
                isPositive = matDelta > 0; // Higher material cost = lower quality for customer
            }

            if (Mathf.Abs(ttpDelta) > maxAbsDelta)
            {
                maxAbsDelta = Mathf.Abs(ttpDelta);
                changeType = "TTP";
                isPositive = ttpDelta > 0; // Higher TTP = better quality
            }

            if (Mathf.Abs(packDelta) > maxAbsDelta)
            {
                maxAbsDelta = Mathf.Abs(packDelta);
                changeType = "Packaging";
                isPositive = packDelta > 0; // Higher packaging cost = lower quality for customer
            }

            // Select appropriate review based on change type and direction
            if (changeType == "Material" || changeType == "TTP")
            {
                if (isPositive)
                    return GetRandomMaterialTTPPositiveReview();
                else
                    return GetRandomMaterialTTPNegativeReview();
            }
            else if (changeType == "Packaging")
            {
                if (isPositive)
                    return GetRandomPackagingPositiveReview();
                else
                    return GetRandomPackagingNegativeReview();
            }

            // Default fallback
            return GetRandomMaterialTTPPositiveReview();
        }

        /// <summary>
        /// Helper to calculate percentage within a range (0-100 scale)
        /// </summary>
        private float CalculatePercentage(float? value, float? min, float? max)
        {
            if (!value.HasValue || !min.HasValue || !max.HasValue)
                return 50f; // Default to middle if any value is null

            if (max.Value <= min.Value)
                return 50f; // Avoid division by zero, return middle value

            // Clamp the value within the min/max range first
            float clampedValue = Mathf.Clamp(value.Value, min.Value, max.Value);

            // Calculate percentage (0-100)
            float percentage = ((clampedValue - min.Value) / (max.Value - min.Value)) * 100f;

            return percentage;
        }

        /// <summary>
        /// Check if product has extreme settings warranting a review
        /// </summary>
        /// <summary>
        /// Check if product has extreme settings warranting a review
        /// </summary>
        private CustomerReviewInfo? CheckExtremeSettingReview(CustomerData customer, Product product)
        {
            // Convert absolute values to percentages (0-100 scale)
            float pricePercent =
                CalculatePercentage(product.ProductPrice, product.MinProductPrice, product.MaxProductPrice);

            // ADJUSTMENT: Need to know the actual min/max ranges for materials and packaging
            // These should come from your game configuration
            // For now, assuming reasonable ranges based on typical game economy
            float matMin = 0f, matMax = 10f; // Adjust based on your actual game values
            float packMin = 0f, packMax = 10f; // Adjust based on your actual game values

            float matPercent = CalculatePercentage(product.MaterialPrice, matMin, matMax);
            float packPercent = CalculatePercentage(product.MaterialPackagingPrice, packMin, packMax);

            // ADJUSTMENT: Fix TTP percentage calculation
            // TTP is 1-5, so convert to 0-100% properly
            float ttpPercent = ((product.TimeToProduceIndex - 1) / 4f) * 100f;
            // This gives: 1=0%, 2=25%, 3=50%, 4=75%, 5=100%

            string triggerReason = "";
            string message = "";
            ReviewType type = ReviewType.ExtremeSettingHigh;

            // ADJUSTMENT: According to CSV, extremeSettingHigh is 80% and extremeSettingLow is 20%
            // Using the config values rather than hardcoding

            // Check for extreme HIGH settings (>80%)
            if (pricePercent > _simulationInfo.ExtremeSettingHigh)
            {
                triggerReason = $"High Price ({pricePercent:F0}%)";
                message = GetRandomHighPriceReview();
                type = ReviewType.ExtremeSettingHigh;
            }
            else if (packPercent > _simulationInfo.ExtremeSettingHigh)
            {
                triggerReason = $"High Packaging ({packPercent:F0}%)";
                message = GetRandomHighPackagingReview();
                type = ReviewType.ExtremeSettingHigh;
            }
            else if (matPercent > _simulationInfo.ExtremeSettingHigh)
            {
                triggerReason = $"High Materials ({matPercent:F0}%)";
                message = GetRandomHighMaterialReview();
                type = ReviewType.ExtremeSettingHigh;
            }
            else if (ttpPercent > _simulationInfo.ExtremeSettingHigh)
            {
                triggerReason = $"High TTP ({product.TimeToProduceIndex})";
                message = GetRandomDiligentTTPReview();
                type = ReviewType.ExtremeSettingHigh;
            }
            // Check for extreme LOW settings (<20%)
            else if (pricePercent < _simulationInfo.ExtremeSettingLow)
            {
                triggerReason = $"Low Price ({pricePercent:F0}%)";
                message = GetRandomLowPriceReview();
                type = ReviewType.ExtremeSettingLow;
            }
            else if (packPercent < _simulationInfo.ExtremeSettingLow)
            {
                triggerReason = $"Low Packaging ({packPercent:F0}%)";
                message = GetRandomLowPackagingReview();
                type = ReviewType.ExtremeSettingLow;
            }
            else if (matPercent < _simulationInfo.ExtremeSettingLow)
            {
                triggerReason = $"Low Materials ({matPercent:F0}%)";
                message = GetRandomLowMaterialReview();
                type = ReviewType.ExtremeSettingLow;
            }
            else if (ttpPercent < _simulationInfo.ExtremeSettingLow)
            {
                triggerReason = $"Low TTP ({product.TimeToProduceIndex})";
                message = GetRandomSlipshodTTPReview();
                type = ReviewType.ExtremeSettingLow;
            }
            else
            {
                // No extreme settings found
                return null;
            }

            return new CustomerReviewInfo
            {
                CharacterId = customer.CharacterId,
                ProductId = product.Id,
                Type = type,
                TriggerReason = triggerReason,
                Message = message
            };
        }

        // Review text selection methods with actual review strings
        private string GetRandomLowMaterialReview()
        {
            string[] reviews =
            {
                "It tasted fine, but the overall texture felt cheap, like they skimped on the ingredients.",
                "I was hoping for something more substantial, but the ingredients used clearly weren't the best, leaving a disappointing aftertaste.",
                "The flavor was acceptable, but the components felt low-quality, and it just didn't have that 'premium' feel."
            };
            return reviews[Random.Range(0, reviews.Length)];
        }

        private string GetRandomHighMaterialReview()
        {
            string[] reviews =
            {
                "You could really taste the quality of the ingredients; everything felt fresh and high-grade.",
                "I was impressed by the richness and depth of flavor, a testament to the excellent ingredients they used.",
                "The texture was just perfect, clearly made with top-notch materials that made a real difference."
            };
            return reviews[Random.Range(0, reviews.Length)];
        }

        private string GetRandomLowPackagingReview()
        {
            string[] reviews =
            {
                "The packaging felt flimsy and cheap, and the item arrived slightly damaged. It didn't give me a sense of quality.",
                "While the product was okay, the packaging was incredibly wasteful and difficult to open, which was frustrating.",
                "The labeling was blurry and hard to read, and the overall presentation felt very unprofessional."
            };
            return reviews[Random.Range(0, reviews.Length)];
        }

        private string GetRandomHighPackagingReview()
        {
            string[] reviews =
            {
                "Attractive and also environmentally friendly packaging! Much appreciated.",
                "The packaging was beautiful and practical, keeping the product fresh and looking great.",
                "I loved the attention to detail in the packaging; it was sturdy, elegant, and clearly designed with care."
            };
            return reviews[Random.Range(0, reviews.Length)];
        }

        private string GetRandomHighPriceReview()
        {
            string[] reviews =
            {
                "For the quality received, the price was simply too high. I felt like I overpaid significantly.",
                "It was an okay product, but the price point didn't match the value. It felt overpriced compared to similar items.",
                "I was expecting more for the cost. The price didn't justify the overall experience."
            };
            return reviews[Random.Range(0, reviews.Length)];
        }

        private string GetRandomLowPriceReview()
        {
            string[] reviews =
            {
                "The price was very reasonable. I felt like I got excellent value for my money.",
                "The price was surprisingly fair. I might buy even more next time!",
                "I was happy to pay such a low price. Keep it up!"
            };
            return reviews[Random.Range(0, reviews.Length)];
        }

        private string GetRandomSlipshodTTPReview()
        {
            string[] reviews =
            {
                "It tasted rushed, like minimal effort was put into it. I expected more attention to detail.",
                "It lacked the finesse I associate with handcrafted goods. It felt mass-produced and impersonal.",
                "The presentation was sloppy, and it just didn't feel like a product made with care."
            };
            return reviews[Random.Range(0, reviews.Length)];
        }

        private string GetRandomDiligentTTPReview()
        {
            string[] reviews =
            {
                "You could tell a lot of time and care went into making this. It tasted like it was made with passion.",
                "The intricate details and perfect texture showed a real dedication to the craft. It was evident they took their time.",
                "It felt like a labor of love. The quality and flavor were a testament to the effort put into it."
            };
            return reviews[Random.Range(0, reviews.Length)];
        }

        private string GetRandomMaterialTTPPositiveReview()
        {
            string[] reviews =
            {
                "The increase in quality is truly impressive. Keep up the fine work, [company name]!",
                "I wasn't crazy about this product the first time, but it's just about perfect now – clearly, they haven't compromised on anything.",
                "My previous experience with [product name] was less than stellar. I decided to give it another chance and BOY am I glad I did! So much better this time around.",
                "I wasn't overly impressed with my previous purchase, but this time it was absolutely delicious; they've clearly made some improvements.",
                "I was hesitant to try it again after my initial experience, but I'm so glad I did; it was a complete turnaround.",
                "My last impression wasn't great, but this recent purchase of [product name] has completely changed my mind; I'm now a fan."
            };
            string review = reviews[Random.Range(0, reviews.Length)];
            // Replace placeholders
            review = review.Replace("[company name]", "our company"); // Replace with actual company name
            review = review.Replace("[product name]", "this product"); // Replace with actual product name
            return review;
        }

        private string GetRandomMaterialTTPNegativeReview()
        {
            string[] reviews =
            {
                "[product name] was fantastic last time, but this recent purchase just didn't taste the same; I'm not sure what changed.",
                "I raved about [product name] after my last order, but this time, it was noticeably different and unfortunately, not in a good way.",
                "I was so excited when I received my [product name] again after how much I enjoyed it before, but this batch was a real letdown."
            };
            string review = reviews[Random.Range(0, reviews.Length)];
            review = review.Replace("[product name]", "this product"); // Replace with actual product name
            return review;
        }

        private string GetRandomPackagingPositiveReview()
        {
            // These seem to be missing from your list, using placeholders
            string[] reviews =
            {
                "The packaging has really improved! Much more professional looking.",
                "I noticed the better packaging immediately. It really adds to the experience.",
                "Great improvement on the packaging. It feels much more premium now."
            };
            return reviews[Random.Range(0, reviews.Length)];
        }

        private string GetRandomPackagingNegativeReview()
        {
            string[] reviews =
            {
                "The packaging used to be so much better. This feels like a downgrade.",
                "I'm disappointed with the new packaging. The old one was much more practical.",
                "Why did you change the packaging? The previous version was perfect."
            };
            return reviews[Random.Range(0, reviews.Length)];
        }

        public class Factory : PlaceholderFactory<WeekSimulationV2>
        {
        }
    }
}