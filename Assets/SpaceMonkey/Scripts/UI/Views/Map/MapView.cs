using Cysharp.Threading.Tasks;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Map
{
    public class MapView : BasePresenterWithController<MapController>
    {
        public override UniTask Initialize(IPresenterData data = null)
        {
            return UniTask.CompletedTask;
        }

        public override void Dispose()
        {
        }
    }
}