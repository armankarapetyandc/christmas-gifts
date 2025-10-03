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
        [SerializeField] private Image dimmerBackground;
        [SerializeField] private TextMeshProUGUI costText;
        
        private Data _data;
        private Account _account;

        public override UniTask Initialize(IPresenterData data = null)
        {
            _data = data as Data;
            dimmerBackground.gameObject.SetActive(true);
            _account = Controller.GetAccount();
            availableCashText.text = $"${_account.Money}";
            costText.text = $"Level {_data.UpgradeLevelProdCap.LevelNumber} Equipment Cost: ${_data.UpgradeLevelProdCap.ProdCapCost}";
            closeButton.OnClickAsObservable().Subscribe(_ =>
            {
                _data.Result?.TrySetResult(_data.UpgradeLevelProdCap);
                Controller.OnClose();
                dimmerBackground.gameObject.SetActive(false);
            }).AddTo(this);
            useCashButton.OnClickAsObservable()
                .Subscribe(_ => UpgradeLevel().Forget())
                .AddTo(this);
            useCreditButton.OnClickAsObservable()
                .Subscribe(_ => UseCreditCard().Forget())
                .AddTo(this);
            useCashButton.interactable = _data.UpgradeLevelProdCap.LevelNumber == _account.LevelProdCaps.Count;
            useCreditButton.interactable = _data.UpgradeLevelProdCap.LevelNumber == _account.LevelProdCaps.Count;


            return UniTask.CompletedTask;
        }

        private async UniTask UseCreditCard()
        {
            if (!Controller.CreditSimulator.HasActiveCard)
            {
                Controller.ShowCreditCardView();
                return;
            }
            var result = await Controller.MakeCreditCardPurchase(_data.UpgradeLevelProdCap);
            if (!result)
            {
                Debug.LogError("Credit card purchase failed");
                return;
            }
            var upgradedLevel = await Controller.UpgradeLevel(_data.UpgradeLevelProdCap,transform);
            _data.Result?.TrySetResult(upgradedLevel);
        }

        private async UniTask UpgradeLevel()
        {
            if (_account.CanAfford(_data.UpgradeLevelProdCap.ProdCapCost))
            {
                _account.Buy(_data.UpgradeLevelProdCap.ProdCapCost);
                var upgradedLevel = await Controller.UpgradeLevel(_data.UpgradeLevelProdCap,transform);
                _data.Result?.TrySetResult(upgradedLevel);
            }
            else
            {
                Debug.LogError($"Is not enough money to upgrade {_data.UpgradeLevelProdCap}");
            }
        }
    }
}