using Cysharp.Threading.Tasks;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using Zenject;

namespace SpaceMonkey.Scripts.UI.Navigation.Core
{
    public abstract class NavigationPresenter : InitializablePresenter
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
            gameObject.SetActive(false);
        }

        public override void Enable()
        {
            gameObject.SetActive(true);
        }
    }

    public abstract class NavigationPresenterWithController<T> : NavigationPresenter where T : BasePresenterController
    {
        protected T Controller;

        [Inject]
        private void Inject(T controller)
        {
            Controller = controller;
        }
    }
}