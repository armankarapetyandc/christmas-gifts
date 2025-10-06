using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.Configs.Map;
using SpaceMonkey.Scripts.UI.Components;
using SpaceMonkey.Scripts.UI.Views.Map.Items;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.EventSystems;

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

            Observable.EveryUpdate().Subscribe(_ =>
            {
                if (EventSystem.current == null)
                {
                    return;
                }

                if (EventSystem.current.currentSelectedGameObject == null)
                {
                    // Debug.LogError(EventSystem.current.currentSelectedGameObject);
                    HidePlaceHolder();
                }
            }).AddTo(this);

            PopulatePlaces();
        }

        private void Update()
        {
        }

        private void PopulatePlaces()
        {
            var places = Controller.GetMapPlaces();
            foreach (var place in places)
            {
                var item = Instantiate(placeItem, placesContainer);
                item.SetPlace(place);
                item.SetPlaceName(place.Name);
                item.SetPosition(place.Position);
                item.SetIcon(place.IconVisualAsset);
                item.OnClickAsObservable().Subscribe(_ =>
                {
                    if (place.Name.Equals("Credit Card") && !Controller.HasCreditCard())
                    {
                        Controller.ShowCreditCardInfoPopup();
                    }
                    else
                    {
                        ShowPlaceHolder(place);
                    }
                    
                }).AddTo(this);
            }
        }

        private void ShowPlaceHolder(IMapPlace place)
        {
            foreach (var holder in placeHolders)
            {
                var item = holder.Holder;
                item.gameObject.SetActive(holder.Type == place.Type);
                item.Init();
                item.SetPlaceName(place.SingleLineName);
                item.SetIcon(place.IconVisualAsset);
                item.GetClickHandler().Subscribe(_ =>
                {

                    if (place is RuntimeMapPlace runtimeMapPlace)
                    {
                        Controller.NavigateToMyCompany();
                    }

                    if (place.Type == PlaceType.CreditCard)
                    {
                        Controller.ShowCreditCardInfoPopup();
                    }
                    
                });
            }
        }

        private void HidePlaceHolder()
        {
            foreach (var holder in placeHolders)
            {
                var item = holder.Holder;
                item.gameObject.SetActive(false);
            }
        }

        public override void Dispose()
        {
        }
    }
}