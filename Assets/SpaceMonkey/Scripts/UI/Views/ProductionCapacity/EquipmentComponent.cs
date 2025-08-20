using System.Collections.Generic;
using R3;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Asset.Database;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Views.ProductionCapacity
{
    public class EquipmentComponent : MonoBehaviour
    {
        [SerializeField] private LevelItemComponent levelItemPrefab;
        [SerializeField] private RectTransform content;

        public List<LevelItemComponent> LevelItems { get; } = new List<LevelItemComponent>();

        public Observable<LevelProdCap> Setup(LevelVisualAsset[] assets, LevelProdCap[]  levels)
        {
            while (LevelItems.Count > 0)
            {
                Destroy(LevelItems[0].gameObject);
            }

            LevelItems.Clear();
            var observables = new List<Observable<LevelProdCap>>();
            for (var i = 0; i < levels.Length; i++)
            {
                var level = levels[i];
                var item = Instantiate(levelItemPrefab, content);
                int assetIndex = i / assets.Length;
                item.Setup(assets[assetIndex], level, i);
                observables.Add(item.OnSelected);
                LevelItems.Add(item);
            }

            return observables.Merge();
        }

        
        
    }
}