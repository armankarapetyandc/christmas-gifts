using System.Collections.Generic;
using R3;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.UI.Asset.Database;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Views.ProductionCapacity
{
    public class EquipmentComponent : MonoBehaviour
    {
        [SerializeField] private LevelItemComponent levelItemPrefab;
        [SerializeField] private RectTransform content;
        
        private readonly List<LevelItemComponent> _levelItems = new List<LevelItemComponent>();
        
        public void Setup(LevelVisualAsset[] assets, ProductionLevelInfo[]  levels)
        {
            while (_levelItems.Count > 0)
            {
                Destroy(_levelItems[0].gameObject);
            }

            _levelItems.Clear();
            
            for (var i = 0; i < levels.Length; i++)
            {
                var levelInfo = levels[i];
                var item = Instantiate(levelItemPrefab, content);
                item.Setup(assets[0],levelInfo);
                _levelItems.Add(item);
            }
        }

        private void SetAssetsDictionary(LevelVisualAsset[] assets, int count)
        {
            
        }
        
    }
}