using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.Configs.Map;
using SpaceMonkey.Scripts.UI.Asset.Database;
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
        [SerializeField] private MapPlace fireEventMapPlace;
        [SerializeField] private List<MapPlaceHolder> placeHolders;
        private List<MapPlaceItem> _places;
        private MapPlaceItem _selectedPlaceItem;

        public void SetMapInteractable(bool state)
        {
            panZoom.SetInteractable(state);
        }

        public override async UniTask Initialize(IPresenterData data = null)
        {
            await UniTask.Yield();
            panZoom.SetZoom(defaultMapZoom);
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
            panZoom.SetPosition(defaultMapPosition);
            //
            // Observable.EveryUpdate().Subscribe(_ =>
            // {
            //     if (EventSystem.current == null)
            //     {
            //         return;
            //     }
            //
            //     if (EventSystem.current.currentSelectedGameObject == null)
            //     {
            //         // Debug.LogError(EventSystem.current.currentSelectedGameObject);
            //       //  HidePlaceHolder();
            //     }
            // }).AddTo(this);

            PopulatePlaces();

            Controller.CheckAndShowFirePopup();
        }

        private void Update()
        {
        }

        private void PopulatePlaces()
        {
            _places?.Clear();
            _places = new List<MapPlaceItem>();
            var places = Controller.GetAppearedMapPlaces(Controller.CurrentWeek);
            foreach (var place in places)
            {
                var item = PopulatePlace(place);
                _places.Add(item);
            }
        }

        private MapPlaceItem PopulatePlace(IMapPlace place)
        {
            Controller.TryAddAppearedPlace(place.Id);
            var item = Instantiate(placeItem, placesContainer);
            item.SetPlace(place);
            if (Controller.HasActiveFire() && place is RuntimeMapPlace)
            {
                item.SetPlaceContent(fireEventMapPlace.IconVisualAsset?.Sprite);
            }
            else
            {
                item.SetPlaceContent(place.IconVisualAsset?.Sprite);
            }

            item.SetMapPlaceState(place is RuntimeMapPlace
                ?
                !Controller.HasActiveFire() ? MapPlaceState.Open : MapPlaceState.Event
                :
                place.DefaultLocked
                    ? MapPlaceState.Locked
                    : MapPlaceState.Active);
            item.SetPlaceName(place.Name);
            item.SetPosition(place.Position);

            item.SetBackgroundColor(place.BackgroundColor);

            if (place.Type == PlaceType.TreatyBird)
            {
                var isActive = PlayerPrefs.GetInt("competitionPin") == 1;
                item.gameObject.SetActive(isActive);
                if (isActive)
                {
                    var product = Controller.GetCompetitionProduct();
                    var visualAsset = Controller.ResolveVisualAsset<SpriteVisualAsset>(product.IconVisualAssetId);
                    item.SetPlaceContent(visualAsset.Sprite);
                    item.SetMapPlaceState(MapPlaceState.Event);   
                }
            }

            item.OnClickAsObservable().Subscribe(_ => { ProcessMapPlaceItemSelect(place); }).AddTo(this);
            return item;
        }
        
        private void ProcessMapPlaceItemSelect(IMapPlace place, bool isForce = false)
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
                Controller.ShowCompetitionPopup(!isForce);
            }
        }

        public MapPlaceHolder SelectMyPlace()
        {
            MapPlaceItem place = _places.FirstOrDefault(item => item.Place.GetType() == typeof(RuntimeMapPlace));
            if (place != null)
            {
               return ShowPlaceHolder(place.Place);
            }

            return null;
        }
        
        public MapPlaceHolder ShowPlaceHolder(IMapPlace place)
        {
            MapPlaceHolder placeHolder = null;
            foreach (var holder in placeHolders)
            {
                var item = holder.Holder;
                var isCurrentHolderType = holder.Type == place.Type;
                item.gameObject.SetActive(isCurrentHolderType);
                if (isCurrentHolderType)
                {
                    item.Init(place.Type);
                    item.SetPlaceName(place.SingleLineName);
                    item.SetLevel(Controller.CurrentLevel);
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
                    placeHolder = holder;
                    break;
                }
            }

            return placeHolder;
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
            var item = _places.FirstOrDefault(x => x.Place.Id == placeId);
            if (item == null)
            {
                return;
            }

            panZoom.SetZoom(defaultMapZoom);
            panZoom.NavigateToTarget((RectTransform)item.transform);
        }

        public async UniTaskVoid FocusAndShowDialog(int placeId)
        {
            FocusOnPlace(placeId);
            await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: destroyCancellationToken);
            var item = _places.FirstOrDefault(x => x.Place.Id == placeId);
            ProcessMapPlaceItemSelect(item?.Place, true);
        }

        public override void Dispose()
        {
        }
    }
}