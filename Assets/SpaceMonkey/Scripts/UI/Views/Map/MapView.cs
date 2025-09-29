using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.Configs.Map;
using SpaceMonkey.Scripts.UI.Components;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Views.Map
{
    [Serializable]
    public class MapPlaceHolder
    {
        public PlaceType Type;
        public MapPlaceHolderItem Holder;
    }
    public class MapView : BasePresenterWithController<MapController>
    {
        [SerializeField] private CanvasPanZoom panZoom;
        [SerializeField] private float defaultMapZoom;
        [SerializeField] private Vector2 defaultMapPosition;
        [SerializeField] private MapPlaceItem placeItem;
        [SerializeField] private RectTransform placesContainer;
        [SerializeField] private List<MapPlaceHolder> placeHolders;
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
                item.OnClickAsObservable().Subscribe(_=> ShowPlaceHolder(place)).AddTo(this);
            }
        }

        private void ShowPlaceHolder(IMapPlace place)
        {
            foreach (var holder in placeHolders)
            {
                var item = holder.Holder;
                item.gameObject.SetActive(holder.Type == place.Type);
                item.SetPlaceName(place.Name);
                item.SetIcon(place.IconVisualAsset);
                item.SetLocked(place.Locked);
            }
        }

        public override void Dispose()
        {
        }
    }
}