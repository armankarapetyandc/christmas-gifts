using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.UI.Asset.Database;
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
        [SerializeField] private MapPlaceItem placeItem;
        [SerializeField] private RectTransform placesContainer;

        public override async UniTask Initialize(IPresenterData data = null)
        {
            await UniTask.Yield();
            panZoom.SetZoom(defaultMapZoom);
            panZoom.SetPosition(defaultMapPosition);

            PopulatePlaces();
        }

        private void PopulatePlaces()
        {
            var places = Controller.GetMapPlaces();
            foreach (var place in places)
            {
                var item = Instantiate(placeItem, placesContainer);
                item.SetPlaceName(place.Name);
                item.SetPosition(place.Position);
                item.SetIcon(place.IconVisualAsset);
                item.SetLocked(place.Locked);
            }
        }

        public override void Dispose()
        {
        }
    }
}