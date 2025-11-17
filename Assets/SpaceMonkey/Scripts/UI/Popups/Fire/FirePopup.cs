using System;
using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.UI.Popups.Core;
using TMPro;
using UIService.Runtime.Core;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Popups.Fire
{
    public class FirePopup : PopupPresenterWithController<FirePopupController>
    {
        [SerializeField] private Button viewCapacityButton;
        [SerializeField] private Button repairButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private TextMeshProUGUI capacityLossText;
        [SerializeField] private TextMeshProUGUI repairCostText;
        
        private Data _data;

        public override UniTask Initialize(IPresenterData data = null)
        {
            _data = (Data)data;
            
            titleText.text = "🔥 FIRE EMERGENCY!";
            descriptionText.text = "A fire has broken out at your facility! Your production capacity has been severely damaged.";
            
            if (_data != null)
            {
                capacityLossText.text = $"Capacity Lost: -{_data.CapacityLoss} hrs (50%)";
                repairCostText.text = $"Repair Cost: ${_data.RepairCost:F0}";
            }
            
            viewCapacityButton.OnClickAsObservable().Subscribe(_ =>
            {
                _data?.OnViewCapacity?.Invoke();
                Controller.Close();
            }).AddTo(this);
            
            repairButton.OnClickAsObservable().Subscribe(_ =>
            {
                _data?.OnRepair?.Invoke();
                Controller.Close();
            }).AddTo(this);
            
            closeButton.OnClickAsObservable().Subscribe(_ =>
            {
                Controller.Close();
            }).AddTo(this);
            
            return UniTask.CompletedTask;
        }

        public class Data : IPresenterData
        {
            public int CapacityLoss { get; set; }
            public float RepairCost { get; set; }
            public Action OnViewCapacity { get; set; }
            public Action OnRepair { get; set; }
        }
    }
}
