using System;
using System.Collections.Generic;
using System.Linq;
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

        private MapPlaceItem _selectedPlaceItem;

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
                if ((place.AppearWeek != 0 && place.AppearWeek > Controller.CurrentWeek) || (place.AppearLevel != 0 &&
                        place.AppearLevel > Controller.CurrentLevel))
                {
                    continue;
                }

                if (place.IconVisualAsset == null)
                {
                    continue;
                }

                var item = Instantiate(placeItem, placesContainer);
                item.SetPlace(place);
                item.SetMapPlaceState(place is RuntimeMapPlace ? MapPlaceState.Open :
                    place.DefaultLocked ? MapPlaceState.Locked : MapPlaceState.Active);
                item.SetPlaceName(place.Name);
                item.SetPosition(place.Position);
                item.SetPlaceContent(place.IconVisualAsset.Sprite);
                item.SetBackgroundColor(place.BackgroundColor);
                if (place.Type == PlaceType.TreatyBird)
                {
                    item.gameObject.SetActive(PlayerPrefs.GetInt("competition") == 1);
                }

                item.OnClickAsObservable().Subscribe(_ =>
                {
                    if (place.Type == PlaceType.CreditCard && !Controller.HasCreditCard())
                    {
                        Controller.ShowCreditCardInfoPopup();
                    }
                    else if (place.Type == PlaceType.BigOrder)
                    {
                        Controller.ShowBigOrderView();
                    }
                    else if (place.Type == PlaceType.BusinessLoan && !Controller.HasBusinessLoan())
                    {
                        Controller.ShowBusinessLoanPopup();
                    }
                    else if (place.Type == PlaceType.BusinessHub)
                    {
                        Controller.NavigateToMyCompany();
                    }
                    else if (place.Type == PlaceType.Investment)
                    {
                        Controller.ShowInvestmentView(place);
                    }
                    else if (place.Type == PlaceType.PlaceHolder)
                    {
                        ShowPlaceHolder(place);
                    }   
                    else if (place.Type == PlaceType.TreatyBird)
                    {
                        Controller.ShowCompetitionPopup();
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
                item.Init(place.Type);
                item.SetPlaceName(place.SingleLineName);
                item.SetIcon(place.IconVisualAsset);
                var companyRating = Controller.CalculateCompanyRating();
                item.SetRatingStars(companyRating);
                item.SetRatingsCount(Controller.ReviewsCount);
                item.SetAverageRating(companyRating);
                
                item.GetClickHandler().Subscribe(_ =>
                {
                    if (place.Type == PlaceType.PlaceHolder)
                    {
                        Controller.NavigateToMyCompany();
                    }

                    if (place.Type == PlaceType.CreditCard)
                    {
                        Controller.ShowCreditCardInfoPopup();
                    }
                    else if (place.Type == PlaceType.BusinessLoan)
                    {
                        Controller.ShowBusinessLoanView();
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
        
        public void FocusOnPlace(int placeId)
        {
            var place = Controller.GetMapPlaces().FirstOrDefault(x => x.Id == placeId);
            if (place == null)
            {
                return;
            }

            // Center the selected place within the viewport at the default zoom
            panZoom.SetPosition(place.Position);
        }

        public override void Dispose()
        {
        }
    }
}