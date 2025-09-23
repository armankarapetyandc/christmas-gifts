using System;
using SpaceMonkey.Scripts.Configs.Characters;
using SpaceMonkey.Scripts.UI.Asset.Database;
using SpaceMonkey.Scripts.UI.Views.Staff;
using SpaceMonkey.Scripts.Utilities;
using UnityEngine;
using UnityEngine.Serialization;

namespace SpaceMonkey.Scripts.Configs
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Space Monkey/Configs/Game Config", order = 1)]
    public class GameConfig : ScriptableObject
    {
        [field: SerializeField] public CategoryInfo[] Categories { get; private set; }
        [field: SerializeField] public ProductionLevelInfo[] ProductionLevels { get; private set; }
        [field: SerializeField] public CharacterConfig[] Characters { get; private set; }
        [field: SerializeField] public MarketingInfo[] MarketingInfos { get; private set; }
        [field: SerializeField] public Staff[] Staffs { get; private set; }
        [field: SerializeField] public SimulationInfo SimulationInfo { get; private set; }
    }

    [Serializable]
    public class CategoryInfo
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public SpriteVisualAsset Visual { get; private set; }
        [field: SerializeField] public HashtagInfo[] Tags { get; private set; }
        [field: SerializeField] public bool Enabled { get; private set; }
    }

    [Serializable]
    public class HashtagInfo
    {
        [field: SerializeField] public string Tag { get; private set; }
        [field: SerializeField] public float MaterialAdd { get; private set; }
        [field: SerializeField] public float PackagingAdd { get; private set; }
    }

    [Serializable]
    public class ProductionLevelInfo
    {
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public int ProdCapAdd { get; private set; }
        [field: SerializeField] public int ProdCapCost { get; private set; }
    }

    [Serializable]
    public class MarketingInfo
    {
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public int MinMult { get; private set; }
        [field: SerializeField] public int MaxMult { get; private set; }
        [field: SerializeField] public int Div { get; private set; }
        [field: SerializeField] public int Unlock { get; private set; }
    }

    [Serializable]
    public class Staff
    {
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public EmployeeProfession Profession { get; private set; }
        [field: SerializeField] public string Payroll { get; private set; }
        [field: SerializeField] public int Capacity { get; private set; }
        [field: SerializeField] public int Speed { get; private set; }
        [field: SerializeField] public int Experience { get; private set; }
        [field: SerializeField] public CharacterConfig Character { get; internal set; }
        
    }

    [Serializable]
    public class SimulationInfo
    {
        [field: SerializeField] public int CustomersMin { get; private set; } = 1;
        [field: SerializeField] public int CustomersMax { get; private set; } = 3;
        [field: SerializeField] public int MoodMin { get; private set; } = 55;
        [field: SerializeField] public int MoodMax { get; private set; } = 75;
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
    }
}