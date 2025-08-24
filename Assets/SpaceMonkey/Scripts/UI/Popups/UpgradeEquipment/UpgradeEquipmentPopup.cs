using System.Linq;
using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Popups.Core;
using TMPro;
using UIService.Runtime.Core;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Popups.UpgradeEquipment
{
    public class UpgradeEquipmentPopup : PopupPresenterWithController<UpgradeEquipmentController>
    {
        public class Data : IPresenterData
        {
            public LevelProdCap UpgradeLevelProdCap { get; set; }
            public UniTaskCompletionSource<LevelProdCap> Result { get; set; }
        }

        [SerializeField] private Button closeButton;
        [SerializeField] private TextMeshProUGUI availableCashText;
        [SerializeField] private Button useCashButton;
        [SerializeField] private Button useCreditButton;
        private Data _data;
        private Account _account;

        public override UniTask Initialize(IPresenterData data = null)
        {
            _data = data as Data;
            _account = Controller.GetAccount();
            availableCashText.text = $"${_account.Money}";
            closeButton.OnClickAsObservable().Subscribe(_ =>
            {
                _data.Result?.TrySetResult(_data.UpgradeLevelProdCap);
                Controller.OnClose();
            }).AddTo(this);
            useCashButton.OnClickAsObservable()
                .Subscribe(_ => UpgradeLevel())
                .AddTo(this);

            return UniTask.CompletedTask;
        }

        private void UpgradeLevel()
        {
            if (CanUpgrade())
            {
                _account.Money -= _data.UpgradeLevelProdCap.ProdCapCost;
                var upgradedLevel = Controller.UpgradeLevel(_data.UpgradeLevelProdCap);
                _data.Result?.TrySetResult(upgradedLevel.Result);
            }
            else
            {
                Debug.LogError($"Is not enough money to upgrade {_data.UpgradeLevelProdCap}");
            }
        }

        private bool CanUpgrade()
        {
            return (_account.Money - _data.UpgradeLevelProdCap.ProdCapCost > 0);
        }
    }
}