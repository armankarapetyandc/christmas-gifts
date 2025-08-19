using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.UI.Components;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Views.Map
{
    public class MapView : BasePresenterWithController<MapController>
    {
        [SerializeField] private CanvasPanZoom panZoom;
        [SerializeField] private float defaultMapZoom;
        [SerializeField] private Vector2 defaultMapPosition;
        public override async UniTask Initialize(IPresenterData data = null)
        {
            await UniTask.Yield();
            panZoom.SetZoom(defaultMapZoom);
            panZoom.SetPosition(defaultMapPosition);
        }

        public override void Dispose()
        {
        }
    }
}