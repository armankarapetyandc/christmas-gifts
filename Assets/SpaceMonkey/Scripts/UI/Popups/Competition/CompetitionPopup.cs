using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Asset.Database;
using SpaceMonkey.Scripts.UI.Popups.Core;
using SpaceMonkey.Scripts.UI.Views.Map;
using TMPro;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Popups.Competition
{
    public enum CompetitionState
    {
        Info,
        Win,
        Lose
    }

    public class CompetitionPopup : PopupPresenterWithController<CompetitionController>
    {



        [SerializeField] private Color redColor;
        [SerializeField] private Color greenColor;

        [SerializeField] private Button closeButton;
        [SerializeField] private Button okButton;
        [SerializeField] private TextMeshProUGUI okButtonText;
        [SerializeField] private Image infoBackground;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private Image icon;
        private Data _viewData;


        public override UniTask Initialize(IPresenterData data = null)
        {
            _viewData = data as Data;
            closeButton.OnClickAsObservable().Subscribe(_ => Controller.Close()).AddTo(this);
          
    
            CompetitionState state = CompetitionState.Info;
            var product = Controller.GetCompetitionProduct();

            var savedProduct = Controller.GetSavedProduct();
            
            var curProduct = string.IsNullOrEmpty(product.IconVisualAssetId)
                ? savedProduct
                : product;
            var visualAsset = Controller.ResolveVisualAsset<SpriteVisualAsset>(curProduct.IconVisualAssetId);
            icon.sprite = visualAsset.Sprite;
            
            if (PlayerPrefs.GetInt("competition") == 1)
            {
                if (_viewData != null && !_viewData.ShowInfoPopup)
                {
                    state = product.ProductPrice == null ? CompetitionState.Win : CompetitionState.Lose;
                }
            }
            switch (state)
            {
                case CompetitionState.Info:
                    titleText.text = CompetitionTexts.CompetitionTitleText;
                    okButtonText.text = string.Format(CompetitionTexts.CompetitionOkText, curProduct.Name);
                    descriptionText.text = string.Format(CompetitionTexts.CompetitionDescriptionText, product.Name);
                    infoBackground.color = redColor;
                    PlayerPrefs.SetInt("competition", 1);
                    PlayerPrefs.SetInt("competitionPin", 1);
                    PlayerPrefs.SetString("competitionItemId", curProduct.Id);
                    okButton.OnClickAsObservable().Subscribe(_ => Controller.ShowProductsView(curProduct)).AddTo(this);
                    break;
                case CompetitionState.Win:
                    titleText.text = string.Format(CompetitionTexts.CompetitionTitleWinText, curProduct.Name);
                    okButtonText.text = CompetitionTexts.CompetitionOkWinText;
                    descriptionText.text = string.Format(CompetitionTexts.CompetitionDescriptionWinText, curProduct.Name);
                    PlayerPrefs.SetInt("competition", 0);
                    PlayerPrefs.SetInt("competitionPin", 0);
                    PlayerPrefs.SetString("competitionItemId", "");
                    infoBackground.color = greenColor;
                    okButton.OnClickAsObservable().Subscribe(_ =>
                    {
                        Controller.HideMapIcon();
                        Controller.Close();
                    }).AddTo(this);
                    break;
                case CompetitionState.Lose:
                    titleText.text = CompetitionTexts.CompetitionTitleLoseText;
                    okButtonText.text =CompetitionTexts.CompetitionOkLoseText;
                    descriptionText.text = string.Format(CompetitionTexts.CompetitionDescriptionLoseText,product.Name);
                    PlayerPrefs.SetInt("competition", 0);
                    PlayerPrefs.SetInt("competitionPin", 0);
                    PlayerPrefs.SetString("competitionItemId", "");
                    infoBackground.color = redColor;
                    okButton.OnClickAsObservable().Subscribe(_ =>
                    {
                        Controller.HideMapIcon();
                        Controller.Close();
                    }).AddTo(this);
                    break;
            }
            return UniTask.CompletedTask;
        }
        

        public class Data : IPresenterData
        {
            public bool ShowInfoPopup { get; set; }
        }
    }
}