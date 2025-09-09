using System;
using SpaceMonkey.Scripts.UI.Asset.Database;
using UnityEngine;
using UnityEngine.Serialization;

namespace SpaceMonkey.Scripts.Configs.Map
{
    [Serializable]
    public class MapPlace
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public PlaceType Type { get; private set; }
        [field: SerializeField] public Vector2 Position { get; private set; }
        [field: SerializeField] public SpriteVisualAsset IconSprite { get; private set; }
        [field: SerializeField] public bool Locked { get; private set; }
    }
}