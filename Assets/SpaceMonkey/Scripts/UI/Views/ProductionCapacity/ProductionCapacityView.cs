using System.Linq;
using System.Threading.Tasks;
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

            equipmentComponent.Setup(
                 Controller.ResolveVisualAssets<LevelVisualAsset>().ToArray()
            ).Subscribe(UpdateUi).AddTo(this);
            UpdateUi(_account.LevelProdCaps[0]);
            scroll.Initialize(equipmentComponent.LevelItems);
            upgratedLevelCapComponent.OnUpgradedLevelUp.Subscribe(level=>
            {
                UpgradedLevelUIUpdate(level).Forget();
            }).AddTo(this);
            prodCapText.text = $"{_account.GetProductionCapacity():F2}";
            availableCashText.text = $"{_account.Money:F2}";
            return UniTask.CompletedTask;
        }

        private async UniTask UpgradedLevelUIUpdate(LevelProdCap level)
        {
            dimmerBackground.gameObject.SetActive(true);
            var updateLevel =await Controller.OpenUpgradeEquipmentPopup(level);
            bool exists = _account.LevelProdCaps.Any(prodCap => prodCap.Id == level.Id);
            scroll.SelectedLevelItem.UpdateLevelUi(exists);
            prodCapText.text = $"{_account.GetProductionCapacity():F2}";
            availableCashText.text = $"{_account.Money:F2}";
            UpdateUi(updateLevel);
            dimmerBackground.gameObject.SetActive(false);
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