using System;
using System.Collections.Generic;
using System.Linq;
using SpaceMonkey.Scripts.Configs.Characters;
using SpaceMonkey.Scripts.UI.Asset.Database;
using SpaceMonkey.Scripts.UI.Views.Staff;
using SpaceMonkey.Scripts.Utilities;
using UnityEngine;

namespace SpaceMonkey.Scripts.Configs
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Space Monkey/Configs/Game Config", order = 1)]
    public class GameConfig: ScriptableObject
    {
        [field: SerializeField] public InsuranceInfo InsuranceInfo { get; private set; }
        [field: SerializeField] public CategoryInfo[] Categories { get; private set; }
        [field: SerializeField] public ProductionLevelInfo[] ProductionLevels { get; private set; }
        [field: SerializeField] public CharacterConfig[] Characters { get; private set; }
        [field: SerializeField] public MarketingInfo[] MarketingInfos { get; private set; }
        [field: SerializeField] public Staff[] Staffs { get; private set; }
        [field: SerializeField] public BusinessExample[] BusinessExamples { get; private set; }
        [field: SerializeField] public SimulationInfo SimulationInfo { get; private set; }
        [field: SerializeField] public CreditCardInfo CreditCardInfo { get; private set; }
        [field: SerializeField] public BusinessLoanInfo BusinessLoanInfo { get; private set; }
        [field: SerializeField] public LevelInfo[] LevelInfos { get; private set; }
        [field: SerializeField] public FireInfo FireInfo { get; private set; }
        
        [field: SerializeField] public NewLocationInfo NewLocationInfo { get; private set; }
        
        [field: SerializeField] public int CompetitionValue { get; private set; }

        public void PatchCategories(CategoryInfo[] infos)
        {
            if (infos == null || infos.Length == 0)
            {
                return;
            }

            var patchedCategories = new List<CategoryInfo>();
            foreach (var info in infos)
            {
                // Find matching category by Name
                var existing = Categories?.FirstOrDefault(c => c.Name == info.Name);

                if (existing != null)
                {
                    // Found match - preserve Visual, overwrite other properties
                    var patched = CategoryInfo.Create(
                        info.Name,
                        existing.Visual, // Keep existing Visual
                        info.Tags,
                        info.Enabled
                    );
                    patchedCategories.Add(patched);
                }
            }

            Categories = patchedCategories.ToArray();
            Debug.Log($"Patched {Categories.Length} categories");
        }

        public void PatchProductionLevelInfos(ProductionLevelInfo[] infos)
        {
            if (infos == null || infos.Length == 0)
            {
                return;
            }

            ProductionLevels = infos.ToArray();
            Debug.Log($"Patched {ProductionLevels.Length} production levels");
        }

        public void PatchMarketingInfos(MarketingInfo[] infos)
        {
            if (infos == null || infos.Length == 0)
            {
                return;
            }

            MarketingInfos = infos.ToArray();
            Debug.Log($"Patched {MarketingInfos.Length} marketing infos");
        }

        public void PatchBusinessExamples(BusinessExample[] infos)
        {
            if (infos == null || infos.Length == 0)
            {
                return;
            }

            BusinessExamples = infos.ToArray();
            Debug.Log($"Patched {BusinessExamples.Length} business infos");
        }

        public void PatchStaffs(Staff[] infos)
        {
            if (infos == null || infos.Length == 0)
            {
                return;
            }

            var patchedStaffs = new List<Staff>();

            foreach (var info in infos)
            {
                // Find matching staff by Id
                var existing = Staffs?.FirstOrDefault(s => s.Id == info.Id);

                if (existing != null)
                {
                    // Found match - preserve Character, overwrite other properties
                    var patched = Staff.Create(
                        info.Id,
                        info.Profession,
                        info.Payroll,
                        info.Capacity,
                        info.Speed,
                        info.Experience,
                        info.Quirk,
                        info.WorkEthic,
                        existing.Character // Keep existing Character
                    );
                    patchedStaffs.Add(patched);
                }
            }

            Staffs = patchedStaffs.ToArray();
            Debug.Log($"Patched {Staffs.Length} staffs");
        }

        public void PatchLevelInfo(LevelInfo[] infos)
        {
            if (infos == null || infos.Length == 0)
            {
                return;
            }

            LevelInfos = infos;
            Debug.Log($"Patched {LevelInfos.Length} level infos");
        }
        

        public void PatchCreditCardInfo(CreditCardInfo info)
        {
            if (info == null)
            {
                return;
            }

            CreditCardInfo = info;
        }

        public void PatchSimulationInfo(SimulationInfo info)
        {
            if (info==null)
            {
                return;
            }

            SimulationInfo = info;
        }
    }


    [Serializable]
    public class LevelInfo
    {
        [field: SerializeField] public int Level { get; private set; }
        [field: SerializeField] public int Score { get; private set; }
        [field: SerializeField] public List<LevelUnlockInfo> UnlockInfo { get; private set; }
    }

    [Serializable]
    public class LevelUnlockInfo
    {
        [field: SerializeField] public string Key { get; private set; }
        [field: SerializeField] public string IconVisualAssetId { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
    }

    [Serializable]
    public class InsuranceInfo
    {
        
        [field: SerializeField] public int InsurancePrice { get; private set; }
        [field: SerializeField] public int OccurrenceLimit { get; private set; }
    }

    [Serializable]
    public class FireInfo
    {
        [field: SerializeField] public int TriggerWeek { get; private set; } = 3;
        [field: SerializeField] public float RepairCost { get; private set; } = 500f;
        [field: SerializeField] 
        [Range(0f, 1f)]
        public float CapacityReductionPercent { get; private set; } = 0.5f;

        public static FireInfo Create(int triggerWeek, float repairCost, float capacityReductionPercent)
        {
            return new FireInfo
            {
                TriggerWeek = triggerWeek,
                RepairCost = repairCost,
                CapacityReductionPercent = capacityReductionPercent
            };
        }
    }

    [Serializable]
    public class CreditCardInfo
    {
        [field: SerializeField] public float CreditLimit { get; private set; }
        [field: SerializeField] public float Apr { get; private set; }
        [field: SerializeField] public float MinimumPayment { get; private set; }
        [field: SerializeField] public float MinimumPatmentCoff { get; private set; }

        public static CreditCardInfo Create(float creditLimit, float apr, float minimumPayment,
            float minimumPatmentCoff)
        {
            return new CreditCardInfo
            {
                CreditLimit = creditLimit,
                Apr = apr,
                MinimumPayment = minimumPayment,
                MinimumPatmentCoff = minimumPatmentCoff
            };
        }
    }

    [Serializable]
    public class CategoryInfo
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public SpriteVisualAsset Visual { get; private set; }
        [field: SerializeField] public HashtagInfo[] Tags { get; private set; }
        [field: SerializeField] public bool Enabled { get; private set; }

        public static CategoryInfo Create(string name, HashtagInfo[] tags, bool enabled)
        {
            return new CategoryInfo
            {
                Name = name,
                Tags = tags,
                Enabled = enabled
            };
        }

        public static CategoryInfo Create(string name, SpriteVisualAsset visualAsset, HashtagInfo[] tags, bool enabled)
        {
            return new CategoryInfo
            {
                Name = name,
                Visual = visualAsset,
                Tags = tags,
                Enabled = enabled
            };
        }
    }

    [Serializable]
    public class HashtagInfo
    {
        [field: SerializeField] public string Tag { get; private set; }
        [field: SerializeField] public float MaterialAdd { get; private set; }
        [field: SerializeField] public float PackagingAdd { get; private set; }

        public static HashtagInfo Create(string tag, float materialAdd, float packagingAdd)
        {
            return new HashtagInfo
            {
                Tag = tag,
                MaterialAdd = materialAdd,
                PackagingAdd = packagingAdd
            };
        }
    }

    [Serializable]
    public class ProductionLevelInfo
    {
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public int ProdCapAdd { get; private set; }
        [field: SerializeField] public int ProdCapCost { get; private set; }

        public static ProductionLevelInfo Create(string id, int prodCapAdd, int prodCapCost)
        {
            return new ProductionLevelInfo
            {
                Id = id,
                ProdCapAdd = prodCapAdd,
                ProdCapCost = prodCapCost
            };
        }
    }

    [Serializable]
    public class MarketingInfo
    {
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public int MinMult { get; private set; }
        [field: SerializeField] public int MaxMult { get; private set; }
        [field: SerializeField] public int Div { get; private set; }
        [field: SerializeField] public int Unlock { get; private set; }

        public static MarketingInfo Create(string id, int minMult, int maxMult, int div, int unlock)
        {
            return new MarketingInfo
            {
                Id = id,
                MinMult = minMult,
                MaxMult = maxMult,
                Div = div,
                Unlock = unlock
            };
        }
    }

    [Serializable]
    public class Staff
    {
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public EmployeeProfession Profession { get; private set; }
        [field: SerializeField] public int Payroll { get; private set; }
        [field: SerializeField] public int Capacity { get; private set; }
        [field: SerializeField] public int Speed { get; private set; }
        [field: SerializeField] public int Experience { get; private set; }
        [field: SerializeField] public string Quirk { get; private set; }
        [field: SerializeField] public string WorkEthic { get; private set; }
        [field: SerializeField] public CharacterConfig Character { get; internal set; }

        public static Staff Create(string id, EmployeeProfession profession, int payroll, int capacity, int speed,
            int experience, string quirk, string workEthic, CharacterConfig character)
        {
            return new Staff
            {
                Id = id,
                Profession = profession,
                Payroll = payroll,
                Capacity = capacity,
                Speed = speed,
                Experience = experience,
                Quirk = quirk,
                WorkEthic = workEthic,
                Character = character
            };
        }
    }

    [Serializable]
    public class BusinessExample
    {
        [field: SerializeField] public string Category { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Title { get; private set; }
        [field: SerializeField] public string Description { get; private set; }

        public static BusinessExample Create(string category, string name, string title, string description)
        {
            return new BusinessExample
            {
                Category = category,
                Name = name,
                Title = title,
                Description = description
            };
        }
    }

    [Serializable]
    public class SimulationInfo
    {
        [field: SerializeField] public int CustomersMin { get; private set; } = 1;
        [field: SerializeField] public int CustomersMax { get; private set; } = 3;
        [field: SerializeField] public int MoodMin { get; private set; } = 55;
        [field: SerializeField] public int MoodMax { get; private set; } = 75;
        [field: SerializeField] public int MoodLow { get; private set; } = 50;
        [field: SerializeField] public int MoodMedium { get; private set; } = 80;
        [field: SerializeField] public int MoodHigh { get; private set; } = 100;
        [field: SerializeField] public int MoodLeave { get; private set; } = 30;
        [field: SerializeField] public float MoodTtpCoefficient { get; private set; } = 2.5f;
        [field: SerializeField] public float MoodMaterialCoefficient { get; private set; } = 10f;
        [field: SerializeField] public float MoodPackagingCoefficient { get; private set; } = 10f;
        [field: SerializeField] public float MoodOrderFulfillmentCoefficient { get; private set; } = 10f;
        [field: SerializeField] public float MoodOrderNotFulfillmentCoefficient { get; private set; } = -15f;
        [field: SerializeField] public int CustomerMoodThreshold { get; private set; } = 30;
        [field: SerializeField] public RangeValue[] MoodRanges { get; private set; }
        [field: SerializeField] public int OrderQuantityMin { get; private set; } = 1;
        [field: SerializeField] public int OrderQuantityMax { get; private set; } = 3;
        [field: SerializeField] 
        [Range(0, 100f)]
        public float OrderQuantReduceLow { get; private set; } = 30f;
        [field: SerializeField] 
        [Range(0, 100f)]
        public float OrderQuantReduceHigh { get; private set; } = 60f;
        [field: SerializeField] public float OrderQuantIncreaseMediumLow { get; private set; } = 30f;
        [field: SerializeField] public float OrderQuantIncreaseMediumHigh { get; private set; } = 60f;
        [field: SerializeField] public float OrderQuantIncreaseLow { get; private set; } = 60f;
        [field: SerializeField] public float OrderQuantIncreaseHigh { get; private set; } = 100f;
        [field: SerializeField] public int NewCustomersMin { get; private set; } = 1;
        [field: SerializeField] public int NewCustomersMax { get; private set; } = 2;
        [field: SerializeField] public float PriceSensitivity { get; private set; } = 0.3f;

        [field: SerializeField]
        [Range(0, 100f)]
        public float ReviewChance { get; private set; } = 75f;

        [field: SerializeField]
        [Range(0, 100f)]
        public float BigProductChange { get; private set; } = 40f;

        [field: SerializeField]
        [Range(0, 100f)]
        public float ExtremeSettingHigh { get; private set; } = 80f;

        [field: SerializeField]
        [Range(0, 100f)]
        public float ExtremeSettingLow { get; private set; } = 20f;

        [field: SerializeField]
        [Range(1, 10)] public int FrequencyMin { get; private set; } = 1;
        
        [field:SerializeField]
        [Range(1,10)]
        public int FrequencyMax { get; private set; } = 4;
        public static SimulationInfo Create(int customersMin, int customersMax, int moodMin, int moodMax,
            int moodLeave, float moodTtpCoefficient, float moodMaterialCoefficient,
            float moodPackagingCoefficient, float moodOrderFulfillmentCoefficient,
            float moodOrderNotFulfillmentCoefficient, int customerMoodThreshold,
            RangeValue[] moodRanges, int orderQuantityMin, int orderQuantityMax,
            float orderQuantReduceLow,float orderQuantReduceHigh,
            int newCustomersMin, int newCustomersMax, float priceSensitivity,
            float reviewChance, float bigProductChange, float extremeSettingHigh,
            float extremeSettingLow)
        {
            return new SimulationInfo
            {
                CustomersMin = customersMin,
                CustomersMax = customersMax,
                MoodMin = moodMin,
                MoodMax = moodMax,
                MoodLeave = moodLeave,
                MoodTtpCoefficient = moodTtpCoefficient,
                MoodMaterialCoefficient = moodMaterialCoefficient,
                MoodPackagingCoefficient = moodPackagingCoefficient,
                MoodOrderFulfillmentCoefficient = moodOrderFulfillmentCoefficient,
                MoodOrderNotFulfillmentCoefficient = moodOrderNotFulfillmentCoefficient,
                CustomerMoodThreshold = customerMoodThreshold,
                MoodRanges = moodRanges,
                OrderQuantityMin = orderQuantityMin,
                OrderQuantityMax = orderQuantityMax,
                OrderQuantReduceLow = orderQuantReduceLow,
                OrderQuantReduceHigh = orderQuantReduceHigh,
                NewCustomersMin = newCustomersMin,
                NewCustomersMax = newCustomersMax,
                PriceSensitivity = priceSensitivity,
                ReviewChance = reviewChance,
                BigProductChange = bigProductChange,
                ExtremeSettingHigh = extremeSettingHigh,
                ExtremeSettingLow = extremeSettingLow
            };
        }
    }
}