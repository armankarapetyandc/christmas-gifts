using System;
using SpaceMonkey.Scripts.Configs.Characters;
using SpaceMonkey.Scripts.UI.Asset.Database;
using UnityEngine;

namespace SpaceMonkey.Scripts.Configs
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Space Monkey/Configs/Game Config", order = 1)]
    public class GameConfig : ScriptableObject
    {
        [field: SerializeField] public CategoryInfo[] Categories { get; private set; }
        [field: SerializeField] public ProductionLevelInfo[] ProductionLevels { get; private set; }
        [field: SerializeField] public CharacterConfig[] Characters { get; private set; }
        [field: SerializeField] public SimulationInfo SimulationInfo { get; private set; }
    }

    [Serializable]
    public class CategoryInfo
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public SpriteVisualAsset Visual { get; private set; }
        [field: SerializeField] public HashtagInfo[] Tags { get; private set; }
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
    public class SimulationInfo
    {
        [field: SerializeField] public int CustomersMin { get; private set; } = 1;
        [field: SerializeField] public int CustomersMax { get; private set; } = 3;
        [field: SerializeField] public int MoodMin { get; private set; } = 55;
        [field: SerializeField] public int MoodMax { get; private set; } = 75;
        [field: SerializeField] public int CustomerMoodThreshold { get; private set; } = 30;
        [field: SerializeField] public int OrderQuantityMin { get; private set; } = 1;
        [field: SerializeField] public int OrderQuantityMax { get; private set; } = 3;
        [field: SerializeField] public int NewCustomersMin { get; private set; } = 1;
        [field: SerializeField] public int NewCustomersMax { get; private set; } = 2;
    }
}