using System.Linq;
using AudioPlayer;
using AudioPlayerService.Runtime;
using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.UI.Components;
using SpaceMonkey.Scripts.Utilities;
using SpaceMonkey.Scripts.Utilities.Validation;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;
using Logger = DCLogger.Runtime.Logger;

namespace SpaceMonkey.Scripts.UI.Views.Startup
{
    public class StartupView : BasePresenterWithController<StartupViewController>
    {
        [SerializeField] private Button startNewButton;
        [SerializeField] private Button loadButton;
        [SerializeField] private SocialPlatformButton[] socialButtons;

        public override UniTask Initialize(IPresenterData data = null)
        {
            SfxPlayer.Play(Sounds.Title_Screen);
            Controller.UIInteractable.Subscribe(UIInteractableChanged).AddTo(this);
            startNewButton.OnClickAsObservable().Subscribe(_ =>
            {
                Controller.StartNewBusiness();
                SfxPlayer.Play(Sounds.Button_Tap);
            }).AddTo(this);
            loadButton.OnClickAsObservable().Subscribe(_ =>
            {
                SfxPlayer.Play(Sounds.Button_Tap);
                Controller.LoadCurrentBusiness().Forget();
            }).AddTo(this);
            socialButtons.Select(button => button.Selected).Merge().Subscribe(SocialPlatformSelected).AddTo(this);

            Validator
                .Validate(Observable.Return(!Controller.AccountService.IsFreshAccount))
                .BindButton(loadButton)
                .AddTo(this);
            return UniTask.CompletedTask;
        }

        private void UIInteractableChanged(bool value)
        {
            startNewButton.interactable = value;
            loadButton.interactable = value;
        }

        private void SocialPlatformSelected(SocialPlatform platform)
        {
            Logger.Log($"Social Platform: {platform}", SpaceMonkeyLogChannels.Default);
        }

        public override void Dispose()
        {
        }
    }
}