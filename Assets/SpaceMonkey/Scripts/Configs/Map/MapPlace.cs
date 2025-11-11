using System;
using SpaceMonkey.Scripts.UI.Asset.Database;
using UnityEngine;

namespace SpaceMonkey.Scripts.Configs.Map
{
    public interface IMapPlace
    {
        public int Id { get; }
        string Name { get; }
        string SingleLineName { get; }
        PlaceType Type { get; }
        Vector2 Position { get; }
        SpriteVisualAsset IconVisualAsset { get; }
     
        int AppearWeek { get;}
        int AppearLevel { get;}
        
         Color BackgroundColor { get; }
         bool DefaultLocked { get; }
    }

    [Serializable]
    public class MapPlace : IMapPlace
    {
        [field: SerializeField] public int Id { get; set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string SingleLineName { get; private set; }
        [field: SerializeField] public PlaceType Type { get; private set; }
        [field: SerializeField] public Vector2 Position { get; private set; }
        [field: SerializeField] public SpriteVisualAsset IconVisualAsset { get; private set; }
        [field: SerializeField] public int AppearWeek { get; private set; }
        [field: SerializeField] public int AppearLevel { get; private set; }
        [field: SerializeField] public Color BackgroundColor { get; private set; }
        [field: SerializeField] public bool DefaultLocked { get; private set; }
    }

    [Serializable]
    public class RuntimeMapPlace : IMapPlace
    {
        [field: SerializeField] public int Id { get; private set; }
        [field: SerializeField] public PlaceType Type { get; private set; }
        [field: SerializeField] public Vector2 Position { get; private set; }
        public string SingleLineName { get; set; }
        public string Name { get; set; }
        public SpriteVisualAsset IconVisualAsset { get; set; }
        [field: SerializeField] public int AppearWeek { get; private set; }
        [field: SerializeField] public int AppearLevel { get; private set; }
        [field: SerializeField] public Color BackgroundColor { get; private set; }
        [field: SerializeField] public bool DefaultLocked { get; private set; } = false;
    }
}