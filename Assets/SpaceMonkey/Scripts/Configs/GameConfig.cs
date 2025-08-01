using System;
using UnityEngine;

namespace SpaceMonkey.Scripts.Configs
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Space Monkey/Configs/Game Config", order = 1)]
    public class GameConfig : ScriptableObject
    {
        [field: SerializeField] public CategoryInfo[] Categories { get; private set; }
    }

    [Serializable]
    public class CategoryInfo
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public HashtagInfo[] Tags { get; private set; }
    }

    [Serializable]
    public class HashtagInfo
    {
        [field: SerializeField] public string Tag { get; private set; }
        [field: SerializeField] public float MaterialAdd { get; private set; }
        [field: SerializeField] public float PackagingAdd { get; private set; }
    }
}