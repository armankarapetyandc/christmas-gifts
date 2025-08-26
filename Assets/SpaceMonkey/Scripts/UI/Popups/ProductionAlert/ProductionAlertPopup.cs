using System;
using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.UI.Popups.Core;
using UIService.Runtime.Core;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Popups.ProductionAlert
{
    public class ProductionAlertPopup : PopupPresenterWithController<ProductionAlertController>
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private Button endWeekButton;
        private Data _data;

        public override UniTask Initialize(IPresenterData data = null)
        {
            _data = data as Data;
            closeButton.OnClickAsObservable().Subscribe(_ => _data.OnClose?.Invoke()).AddTo(this);
            endWeekButton.OnClickAsObservable().Subscribe(_ => _data.OnEndWeek?.Invoke()).AddTo(this);
            return UniTask.CompletedTask;
        }

        public class Data : IPresenterData
        {
            public Action OnClose;
            public Action OnEndWeek;
        }
    }
}