using Cysharp.Threading.Tasks;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.Utilities
{
    public static class UIServiceExtensions
    {
        public static async UniTask HidePreviousAndShow<T>(this PresenterService service, IPresenterData data = null)
            where T : BasePresenter
        {
            await service.Hide();
            await service.Show<T>(data, hidePrevious: true);
        }
    }
}