using AudioPlayer;
using AudioPlayerService.Runtime;
using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.UI.Popups.Core;
using UIService.Runtime.Core;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Popups.DeleteProduct
{
    public class DeleteProductPopup : PopupPresenterWithController<DeleteProductPopupController>
    {
        public class Data : IPresenterData
        {
            internal UniTaskCompletionSource<bool> Result { get; } = new UniTaskCompletionSource<bool>();

            internal void SetResult(bool state)
            {
                Result.TrySetResult(state);
            }

            public UniTask<bool> GetAwaiter()
            {
                return Result.Task;
            }
        }

        [SerializeField] private Button closeButton;
        [SerializeField] private Button cancelButton;
        [SerializeField] private Button confirmButton;

        public override UniTask Initialize(IPresenterData data = null)
        {
            var presenterData = data as Data;
            closeButton.OnClickAsObservable().Subscribe(_ =>
            {
                SfxPlayer.Play(Sounds.Button_Tap);
                presenterData!.SetResult(false);
                Controller.Close();
            }).AddTo(this);
            cancelButton.OnClickAsObservable().Subscribe(_ =>
            {
                SfxPlayer.Play(Sounds.Button_Tap);
                presenterData!.SetResult(false);
                Controller.Close();
            }).AddTo(this);
            confirmButton.OnClickAsObservable().Subscribe(_ =>
            {
                SfxPlayer.Play(Sounds.Button_Tap);
                presenterData!.SetResult(true);
                Controller.Close();
            }).AddTo(this);
            return UniTask.CompletedTask;
        }
    }
}