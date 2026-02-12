using System;
using AudioPlayer;
using AudioPlayerService.Runtime;
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
            closeButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    SfxPlayer.Play(Sounds.sfx_ClickSmall);
                    _data.CompletionSource.TrySetResult(Data.CloseResult.Close);
                    Controller.Close();
                }).AddTo(this);
            endWeekButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    _data.CompletionSource.TrySetResult(Data.CloseResult.EndWeek);
                    Controller.Close();
                }).AddTo(this);
            return UniTask.CompletedTask;
        }

        public class Data : IPresenterData
        {
            public enum CloseResult
            {
                EndWeek,
                Close
            }

            public UniTaskCompletionSource<CloseResult> CompletionSource { get; } =
                new UniTaskCompletionSource<CloseResult>();
        }
    }
}