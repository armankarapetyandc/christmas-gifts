using System.Linq;
using Cysharp.Threading.Tasks;
using R3;
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
        public class Data : IPresenterData
        {
            public int LevelNumber { get; internal set; }
        }

        [SerializeField] private Button backButton;
        [SerializeField] private EquipmentComponent equipmentComponent;
        [SerializeField] private CurrentLevelCapComponent currentLevelCapComponent;
        [SerializeField] private UpgratedLevelCapComponent upgratedLevelCapComponent;
        [SerializeField] private ScrollSnap scroll;
        [SerializeField] private TextMeshProUGUI availableCashText;
        [SerializeField] private TextMeshProUGUI prodCapText;

        private Account _account;
        private Data _data;
        private LevelVisualAsset[] _levelVisualAssets;

        public override UniTask Initialize(IPresenterData data = null)
        {
            _data = data as Data;
            _levelVisualAssets = Controller.ResolveVisualAssets<LevelVisualAsset>().ToArray();
            backButton.OnClickAsObservable().Subscribe(_ => Controller.OnBack()).AddTo(this);
            _account = Controller.GetAccount();

            equipmentComponent.Setup(
                _levelVisualAssets
            ).Subscribe(UpdateUi).AddTo(this);
            scroll.Initialize(equipmentComponent.LevelItems);
            scroll.SnapToIndex(_data?.LevelNumber ?? 0);
            UpdateUi(_data == null ? _account.LevelProdCaps[0] : _account.LevelProdCaps[_data.LevelNumber]);
            upgratedLevelCapComponent.OnUpgradedLevelUp.Subscribe(level => { UpgradedLevelUIUpdate(level).Forget(); })
                .AddTo(this);
            prodCapText.text = $"{_account.GetProductionCapacity():F2}";
            availableCashText.text = $"{_account.Money:F2}";
            return UniTask.CompletedTask;
        }

        private async UniTask UpgradedLevelUIUpdate(LevelProdCap level)
        {
            var updateLevel = await Controller.OpenUpgradeEquipmentPopup(level);
            int index = _account.LevelProdCaps.FindIndex(prodCap => prodCap.Id == level.Id);
            scroll.SelectedLevelItem.UpdateLevelUi(index != -1);
            prodCapText.text = $"{_account.GetProductionCapacity():F2}";
            availableCashText.text = $"{_account.Money:F2}";
            UpdateUi(updateLevel);
            if (index != -1)
            {
                int assetIndex = index / _levelVisualAssets.Length;
                var visualAsset = _levelVisualAssets[assetIndex];
                Controller.UpgradeLevel(index,visualAsset.LevelIconSprite, visualAsset.FontAsset);
            }
        }

        private void UpdateUi(LevelProdCap level)
        {
            bool exists = _account.LevelProdCaps.Any(prodCap => prodCap.Id == level.Id);
            if (exists && !level.NeedRepair)
            {
                upgratedLevelCapComponent.gameObject.SetActive(false);
                currentLevelCapComponent.UpdateUi(_account.GetProductionCapacity());
                currentLevelCapComponent.gameObject.SetActive(true);
            }
            else if (exists && level.NeedRepair)
            {
                Debug.LogError("log");
            }
            else
            {
                currentLevelCapComponent.gameObject.SetActive(false);
                upgratedLevelCapComponent.UpdateUi(level, _account.GetProductionCapacity()
                );
                upgratedLevelCapComponent.gameObject.SetActive(true);
            }
        }

        public override void Dispose()
        {
        }
    }
}