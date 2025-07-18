using System;
using Cysharp.Threading.Tasks;
using R3;
using TMPro;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Popups.Alert
{
    public class AlertPopup : BasePresenterWithController<AlertPopupController>
    {
        [SerializeField] private Button okButton;
        [SerializeField] private Button cancelButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI errorMessageText;
        [SerializeField] private TextMeshProUGUI errorMessageNoTitleText;
        [SerializeField] private TextMeshProUGUI okButtonText;
        [SerializeField] private TextMeshProUGUI cancelButtonText;
        [SerializeField] private TextMeshProUGUI appInfoText;
        private Data _data;

        public override UniTask Initialize(IPresenterData data = null)
        {
            _data = (Data)data;
            okButtonText.text = _data?.OkButtonText;
            cancelButtonText.text = _data?.CancelButtonText;
            okButton.OnClickAsObservable().Subscribe(_ =>
            {
                _data.OnOk?.Invoke();
                _data.Result?.TrySetResult(true);
            }).AddTo(this);
            closeButton.OnClickAsObservable().Subscribe(_ =>
            {
                _data.OnCancel?.Invoke();
                _data.Result?.TrySetResult(false);
            }).AddTo(this);
            cancelButton.OnClickAsObservable().Subscribe(_ =>
            {
                _data.OnCancel?.Invoke();
                _data.Result?.TrySetResult(false);
            }).AddTo(this);

            var hasTitle = !string.IsNullOrEmpty(_data?.Title);

            if (!string.IsNullOrEmpty(_data?.ErrorMessage))
            {
                errorMessageText.text = _data.ErrorMessage;
                errorMessageNoTitleText.text = _data.ErrorMessage;
                errorMessageText.gameObject.SetActive(hasTitle);
                errorMessageNoTitleText.gameObject.SetActive(!hasTitle);
                GUIUtility.systemCopyBuffer = _data.ErrorMessage;
            }

            if (hasTitle)
            {
                titleText.text = _data.Title;
            }

            okButton.gameObject.SetActive(_data.OnOk != null || _data.Result != null);
            cancelButton.gameObject.SetActive(_data.OnCancel != null || _data.Result != null);
            closeButton.gameObject.SetActive(_data.OnCancel != null || _data.Result != null);
            appInfoText.text = $"v{Application.version}";
            return UniTask.CompletedTask;
        }

        public override void Dispose()
        {
        }


        public class Data : IPresenterData
        {
            public string Title { get; set; }
            public string ErrorMessage { get; set; }
            public Action OnOk { get; set; }

            public Action OnCancel { get; set; }
            public string OkButtonText { get; set; } = "OK";
            public string CancelButtonText { get; set; } = "Cancel";
            public string Source { get; set; }

            public UniTaskCompletionSource<bool> Result { get; set; }
        }
    }
}