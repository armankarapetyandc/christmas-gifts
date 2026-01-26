using System.Linq;
using AudioPlayer;
using AudioPlayerService.Runtime;
using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Asset.Database;
using SpaceMonkey.Scripts.UI.Components;
using SpaceMonkey.Scripts.UI.Popups;
using SpaceMonkey.Scripts.UI.Utility;
using SpaceMonkey.Scripts.UI.Utility.Locker;
using SpaceMonkey.Scripts.Utilities.Validation;
using TMPro;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.BusinessHub
{
    public class BusinessHubView : BasePresenterWithController<BusinessHubController>
    {
        [SerializeField] private IconComponent iconComponent;
        [SerializeField] private TextMeshProUGUI businessName;
        [SerializeField] private TextMeshProUGUI weekNumber;
        [SerializeField] private TextMeshProUGUI levelNumber;
        [SerializeField] private Slider levelSlider;
        
        [SerializeField] private TextMeshProUGUI moneyText;
        [SerializeField] private TextMeshProUGUI prodCapText;

        [SerializeField] private Button startButton;
        [SerializeField] private IconComponent productComponent;
        [SerializeField] private TextMeshProUGUI productsCountText;

        [SerializeField] private Button productionButton;
        [SerializeField] private Button marketingButton;
        [SerializeField] private Button staffButton;
        [SerializeField] private Button plmButton;
        [SerializeField] private Button reviewButton;

        [SerializeField] private ToastPopup toastPopup;

        [SerializeField] private LockByLevel[] lockedByLevels;
        [SerializeField] private LockByMoney[] lockedByMoney;
        [SerializeField] private LockByWeek[] lockByWeek;
        [SerializeField] private LockByReview[] lockByReview;

        private Data _viewData;
        public IconComponent ProductComponent => productComponent;

        public Button StartButton => startButton;

        public override UniTask Initialize(IPresenterData data = null)
        {
            _viewData = data as Data;
            productComponent.OnClick.Subscribe(_ =>
            {
                {
                    SfxPlayer.Play(Sounds.Button_Tap);
                    Controller.ShowProductView();
                }
            }).AddTo(this);
            marketingButton.OnClickAsObservable().Subscribe(_ =>
            {
                SfxPlayer.Play(Sounds.Button_Tap);
                if (PlayerPrefs.GetInt("marketing") != 1)
                {
                    var item = lockedByMoney.FirstOrDefault(level => level.Key == "marketing");
                    if (item != null)
                    {
                        toastPopup.ShowToast($"Unlocks at ", item.Value);
                        return;
                    }
                }


                Controller.ShowMarketingView();
            }).AddTo(this);

            staffButton.OnClickAsObservable().Subscribe(_ =>
            {
                SfxPlayer.Play(Sounds.Button_Tap);
                var item = lockedByLevels.FirstOrDefault(level => level.Key == "stuff");
                if (item != null && item.Locked)
                {
                    var unlockInfo = Controller.GetLevelUnlockConfig(item.Key);
                    toastPopup.ShowToast($"Unlocks at Company Level {unlockInfo.Level}");
                    return;
                }

                Controller.ShowStaffView();
            }).AddTo(this);
            startButton.OnClickAsObservable().Subscribe(_ =>
            {
                {
                    SfxPlayer.Play(Sounds.Button_Tap);
                    Controller.StartWeek();
                }
            }).AddTo(this);
            productionButton.OnClickAsObservable().Subscribe(_ =>
            {
                SfxPlayer.Play(Sounds.Button_Tap);
                var item = lockedByLevels.FirstOrDefault(level => level.Key == "production");
                if (item != null && item.Locked)
                {
                    var unlockInfo = Controller.GetLevelUnlockConfig(item.Key);
                    toastPopup.ShowToast($"Unlocks at Company Level {unlockInfo.Level}");
                    return;
                }

                Controller.ShowProductionView();
            }).AddTo(this);
            reviewButton.OnClickAsObservable().Subscribe(_ =>
            {
                SfxPlayer.Play(Sounds.Button_Tap);
                if (Controller.GetAccount().Reviews.Count <= 0)
                {
                    toastPopup.ShowToast($"Unlocks after 1st review");
                    return;
                }

                Controller.ShowAllReviewView();
            }).AddTo(this);

            plmButton.OnClickAsObservable().Subscribe(_ =>
            {
                SfxPlayer.Play(Sounds.Button_Tap);
                if (Controller.GetAccount().WeeksV2.Count <= 0)
                {
                    toastPopup.ShowToast($"Unlocks at 1st week complete");
                    return;
                }

                Controller.ShowPlmView();
            }).AddTo(this);


            Controller.OnLevelChanged.Subscribe(CheckForUnlockByLevel).AddTo(this);
            Controller.OnUnlockBy.Subscribe(UnlockItem).AddTo(this);
            CheckForUnlockByLevel(Controller.Level);
            CheckForUnlockByMoney(Controller.Money);
            CheckForUnlockByReview(Controller.GetAccount().Reviews.Count);
            CheckForUnlockByWeek(Controller.GetAccount().WeeksV2.Count);
            SetupDefaults();

            Controller.CheckAndShowFirePopup();
            return UniTask.CompletedTask;
        }

        private void CheckForUnlockByReview(int reviewCount)
        {
            Controller.CheckForUnlockByReview(lockByReview, reviewCount);
        }

        private void CheckForUnlockByWeek(int week)
        {
            Controller.CheckForUnlockByWeek(lockByWeek, week);
        }

        private void CheckForUnlockByMoney(float money)
        {
            Controller.CheckForUnlockByMoney(lockedByMoney, money);
        }

        private void CheckForUnlockByLevel(int level)
        {
            Controller.CheckForUnlockByMoney(lockedByLevels, level);
        }


        private void UnlockItem(LockBy lockBy)
        {
            lockBy.Unlock();
        }


        private void SetupDefaults()
        {
            var account = Controller.GetAccount();
            businessName.text = account.Company.CompanyName;
            levelNumber.text = account.Level.ToString();
            
            Configs.LevelInfo nextLevelInfo = Controller.GetLeveInfoData(Controller.Level + 1);
            levelSlider.value =  account.Score / (float) nextLevelInfo.Score;
            
            moneyText.text = $"${account.Money:F2}";
            prodCapText.text = $"{account.GetProductionCapacity()} hrs";
            weekNumber.text = account.Week.ToString();
            productsCountText.text = account.Products.Count == 0
                ? "Products"
                : $"Products ({account.Products.Count.ToString()})";

            var shapeVisualAsset =
                Controller.ResolveVisualAsset<SpriteVisualAsset>(account.Company.Logo.ShapeVisualAssetId);
            var iconVisualAsset =
                Controller.ResolveVisualAsset<SpriteVisualAsset>(account.Company.Logo.IconVisualAssetId);
            var colorVisualAsset =
                Controller.ResolveVisualAsset<ColorVisualAsset>(account.Company.Logo.BackgroundColorVisualAssetId);

            iconComponent.SetShape(shapeVisualAsset);
            iconComponent.SetIcon(iconVisualAsset);
            iconComponent.SetColor(colorVisualAsset);

            PreviewProductAtIndexIfExists(0);

            Validator
                .Validate(Observable.Return(account.Products.Count > 0))
                .BindButton(startButton)
                .AddTo(this);
        }

        private void PreviewProductAtIndexIfExists(int index)
        {
            var account = Controller.GetAccount();
            if (account.Products == null || account.Products.Count == 0 || index >= account.Products.Count)
            {
                return;
            }

            var product = account.Products[index];

            var iconVisualAsset = Controller.ResolveVisualAsset<SpriteVisualAsset>(product.IconVisualAssetId);
            var colorVisualAsset =
                Controller.ResolveVisualAsset<ColorVisualAsset>(product.BackgroundColorVisualAssetId);

            productComponent.SetIcon(iconVisualAsset);
            productComponent.SetColor(colorVisualAsset);
        }

        public override void Dispose()
        {
        }

        public class Data : IPresenterData
        {
            public bool IsWeekEnd;
        }
    }
}