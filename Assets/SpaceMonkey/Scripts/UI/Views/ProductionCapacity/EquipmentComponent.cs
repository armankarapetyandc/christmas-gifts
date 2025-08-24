using System.Collections.Generic;
using System.Linq;
using R3;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Asset.Database;
using UnityEngine;
using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.ProductionCapacity
{
    public class EquipmentComponent : MonoBehaviour
    {
        [SerializeField] private LevelItemComponent levelItemPrefab;
        [SerializeField] private RectTransform content;
        
        [Inject] private GameConfig _gameConfig;
        [Inject] private AccountService _accountService;

        public List<LevelItemComponent> LevelItems { get; } = new List<LevelItemComponent>();
        private List<Observable<LevelProdCap>> _observables = new List<Observable<LevelProdCap>>();

        public Observable<LevelProdCap> Setup(LevelVisualAsset[] assets)
        {
            while (LevelItems.Count > 0)
            {
                Destroy(LevelItems[0].gameObject);
            }

            LevelItems.Clear();
            var levels = GetLevelProdCapData(_gameConfig.ProductionLevels);
            
            for (var i = 0; i < _gameConfig.ProductionLevels.Length; i++)
            {
                var level = levels[i];
                var item = Instantiate(levelItemPrefab, content);
                int assetIndex = i / assets.Length;
                item.Setup(assets[assetIndex], level, i, !IsUpgraded(level));
                _observables.Add(item.OnSelected);
                LevelItems.Add(item);
            }
            return _observables.Merge();
        }

        private bool IsUpgraded(LevelProdCap level)
        {
            return _accountService.Model.Account.LevelProdCaps.Any(prodCap => prodCap.Id == level.Id);
        }
        
        private LevelProdCap[] GetLevelProdCapData(ProductionLevelInfo[] levels)
        {
            return levels
                .Select(SetLevel)
                .ToArray();
        }

        private LevelProdCap SetLevel(ProductionLevelInfo levelInfo, int levelIndex)
        {
            LevelProdCap level = default;
            level.ProdCapAdd = levelInfo.ProdCapAdd;
            level.ProdCapCost = levelInfo.ProdCapCost;
            level.Id = levelInfo.Id;
            return level;
        }
    }
}