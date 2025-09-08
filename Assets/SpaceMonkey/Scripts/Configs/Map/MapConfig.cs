using System;
using UnityEngine;

namespace SpaceMonkey.Scripts.Configs.Map
{
    [Serializable]
    public struct MapPlace
    {
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public Vector2 Position { get; set; }
    }
    
    [CreateAssetMenu(fileName = "MapConfig", menuName = "Space Monkey/Configs/Map Config", order = 1)]
    public class MapConfig : ScriptableObject
    {
    }
}