using Cysharp.Threading.Tasks;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using Zenject;

namespace SpaceMonkey.Scripts.UI.Popups.Core
{
    public abstract class PopupPresenter : InitializablePresenter
    {
        public override UniTask Show()
        {
            return UniTask.CompletedTask;
        }

        public override UniTask Hide()
        {
            return UniTask.CompletedTask;
        }

        public override void Disable()
        {
        }

        public override void Enable()
        {
        }
    }

    public abstract class PopupPresenterWithController<T> : PopupPresenter where T : BasePresenterController
    {
        protected T Controller;

        [Inject]
        private void Inject(T controller)
        {
            Controller = controller;
        }
    }
}