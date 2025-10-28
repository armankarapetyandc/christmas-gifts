using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Configs.Characters;
using SpaceMonkey.Scripts.Profile;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SpaceMonkey.Scripts.Simulation
{
    public class WeekSimulationV2
    {
        private readonly AccountService _accountService;
        private readonly GameConfig _gameConfig;
        private readonly SimulationInfo _simulationInfo;
        private Account Account => _accountService.Model.Account;

        [Serializable]
        public struct Customer
        {
            public string CharacterId { get; set; }
            public int Mood { get; set; }
            public int Frequency { get; set; }
        }

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

            [JsonIgnore] public float OrderCost => Product.ProductPrice!.Value * Quantity;
            [JsonIgnore] public float OrderProdCost => Product.ProdCapCost!.Value * Quantity;
        }

        [Serializable]
        public struct Order
        {
            public Customer Customer { get; set; }
            public List<OrderEntry> OrderEntries { get; set; }
            public bool WasFulfilled { get; set; }
        }

        [Serializable]
        public struct Week
        {
            public string Id { get; set; }
            public int WeekNumber { get; set; }
            public List<Customer> CustomersThisWeek { get; set; }
            public List<Order> Orders { get; set; }
            public float WeekRevenue { get; set; }
            public float WeekProfit { get; set; }
            public int OrdersFulfilled { get; set; }
            public int OrdersMissed { get; set; }
        }

        private Week? currentWeek;
        private List<CustomerData> allCustomers;
        
        public WeekSimulationV2(AccountService accountService, GameConfig gameConfig)
        {
            _accountService = accountService;
            _gameConfig = gameConfig;
            _simulationInfo = gameConfig.SimulationInfo;
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
            
            if (Account.WeeksV2 == null)
                Account.WeeksV2 = new List<Week>();
            var weekData = Account.WeeksV2.FirstOrDefault(w => w.WeekNumber == weekNumber);
            if (weekData.Orders == null)
            {
                weekData = new Week
                {
                    Id = Guid.NewGuid().ToString(),
                    WeekNumber = weekNumber,
                    CustomersThisWeek = new List<Customer>(),
                    Orders = new List<Order>()
                };
                Account.WeeksV2.Add(weekData);
            }
            
            foreach (var customer in visitingCustomers)
            {
                var orderEntries = GenerateOrderEntries(customer);

                weekData.CustomersThisWeek.Add(new Customer
                {
                    CharacterId = customer.CharacterId,
                    Mood = customer.Mood,
                    Frequency = customer.Frequency
                });

                weekData.Orders.Add(new Order
                {
                    Customer = new Customer
                    {
                        CharacterId = customer.CharacterId,
                        Mood = customer.Mood,
                        Frequency = customer.Frequency
                    },
                    OrderEntries = orderEntries,
                    WasFulfilled = false // default, to be updated later via TryShipOrder
                });
            }
            
            
            Debug.Log($"Prepared {weekData.Orders.Count} orders for Week {weekNumber}");
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
                float percent = Random.Range(_simulationInfo.OrderQuantReduceLow, _simulationInfo.OrderQuantReduceHigh + 1) / 100f;
                finalQuantity = Mathf.Max(Mathf.FloorToInt(baseQuantity * (1f - percent)), 0);
            }
            else if (customer.Mood <= _simulationInfo.MoodMedium)
            {
                float percent = Random.Range(_simulationInfo.OrderQuantIncreaseMediumLow, _simulationInfo.OrderQuantIncreaseMediumHigh + 1) / 100f;
                finalQuantity = Mathf.FloorToInt(baseQuantity * (1f + percent));
            }
            else if (customer.Mood <= _simulationInfo.MoodHigh)
            {
                float percent = Random.Range(_simulationInfo.OrderQuantIncreaseLow, _simulationInfo.OrderQuantIncreaseHigh + 1) / 100f;
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
            if (currentWeek == null)
            {
                Debug.LogWarning("No current week to finish.");
                return;
            }

            if (Account.WeeksV2 == null)
                Account.WeeksV2 = new List<Week>();

            Account.WeeksV2.Add(currentWeek.Value);
            Account.AllCustomers = allCustomers;

            Debug.Log($"Finished Week {currentWeek.Value.WeekNumber} and saved to account.");
            currentWeek = null;
            allCustomers = null;
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

            return Random.value < 0.1;
        }
        public bool TryShipOrder(string customerId, float companyProdCapacity)
        {
            var currentWeek = Account.WeeksV2?.LastOrDefault();
            if (currentWeek == null || currentWeek.Value.Orders == null)
            {
                Debug.LogWarning("No current week or orders found.");
                return false;
            }

            var orderIndex = currentWeek.Value.Orders.FindIndex(o => o.Customer.CharacterId == customerId);
            if (orderIndex < 0)
            {
                Debug.LogWarning($"No order found for customer {customerId}.");
                return false;
            }

            var order = currentWeek.Value.Orders[orderIndex];
            float totalProdCost = order.OrderEntries.Sum(e => e.OrderProdCost);

            bool canFulfill = companyProdCapacity >= totalProdCost;
            order.WasFulfilled = canFulfill;
            currentWeek.Value.Orders[orderIndex] = order;

            Debug.Log($"{(canFulfill ? "Fulfilled" : "NOT fulfilled")} order for {customerId}. Cost: {totalProdCost:F2}, Capacity: {companyProdCapacity:F2}");
            return canFulfill;
        }
    }
}