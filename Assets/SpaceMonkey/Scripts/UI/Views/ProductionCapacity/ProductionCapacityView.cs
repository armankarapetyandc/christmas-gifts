using System.Linq;
using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Asset.Database;
using SpaceMonkey.Scripts.UI.Utility;
using TMPro;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.ProductionCapacity
{
    public class ProductionCapacityView : BasePresenterWithController<ProductionCapacityViewController>
    {
        [SerializeField] private Button backButton;
        [SerializeField] private EquipmentComponent equipmentComponent;
        [SerializeField] private CurrentLevelCapComponent currentLevelCapComponent;
        [SerializeField] private UpgratedLevelCapComponent upgratedLevelCapComponent;
        [SerializeField] private ScrollSnap scroll;
        [SerializeField] private TextMeshProUGUI availableCashText;
        [SerializeField] private TextMeshProUGUI prodCapText;
        [SerializeField] private Image dimmerBackground;

        private Account _account;

        public override UniTask Initialize(IPresenterData data = null)
        {
            backButton.OnClickAsObservable().Subscribe(_ => Controller.OnBack()).AddTo(this);
            _account = Controller.GetAccount();
            var prodCaps = (_account.LevelProdCaps == null || _account.LevelProdCaps.Count == 0)
                ? GetLevelProdCapData()
                : _account.LevelProdCaps.ToArray();

            equipmentComponent.Setup(
                Controller.ResolveVisualAssets<LevelVisualAsset>().ToArray(),
                prodCaps.ToArray()
            ).Subscribe(UpdateUi).AddTo(this);
            
            scroll.Initialize(equipmentComponent.LevelItems);
            upgratedLevelCapComponent.OnUpgradedLevelUp.Subscribe(level =>
            {
                Controller.OpenUpgradeEquipmentPopup(level);
            }).AddTo(this);
            
            return UniTask.CompletedTask;
        }

        private LevelProdCap[] GetLevelProdCapData()
        {
            var levels = Controller.RetrieveInfo();

            return levels
                .Select(SetLevel)
                .ToArray();
        }

        private void UpdateAvailableCashText(float value)
        {
            availableCashText.text = $"{value:F2}";
        }

        private void UpdateProdCapText(float value)
        {
            prodCapText.text = $"{value:F2}";
        }

        private LevelProdCap SetLevel(ProductionLevelInfo levelInfo, int levelIndex)
        {
            LevelProdCap level = default;
            level.AssignId();
            level.ProdCapAdd = levelInfo.ProdCapAdd;
            level.ProdCapCost = levelInfo.ProdCapCost;
            level.IsLocked = levelIndex != 0;
            _account.SetLevel(level);
            return level;
        }

        private void UpdateUi(LevelProdCap level)
        {
            if (string.IsNullOrEmpty(level.Id)) return;
            if (level.IsLocked)
            {
                currentLevelCapComponent.gameObject.SetActive(false);
                upgratedLevelCapComponent.UpdateUi(level, _account.GetProductionCapacity()
                );
                upgratedLevelCapComponent.gameObject.SetActive(true);
            }
            else
            {
                upgratedLevelCapComponent.gameObject.SetActive(false);
                currentLevelCapComponent.UpdateUi(_account.GetProductionCapacity());
                currentLevelCapComponent.gameObject.SetActive(true);
            }
        }

        public override void Dispose()
        {
        }
    }
}