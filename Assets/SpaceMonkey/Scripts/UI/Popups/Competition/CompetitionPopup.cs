using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Asset.Database;
using SpaceMonkey.Scripts.UI.Popups.Core;
using TMPro;
using UIService.Runtime.Core;
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
            var visualAsset = Controller.ResolveVisualAsset<SpriteVisualAsset>(product.IconVisualAssetId);
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
                    okButtonText.text = CompetitionTexts.CompetitionOkText;
                    descriptionText.text = string.Format(CompetitionTexts.CompetitionDescriptionText, product.Name);
                    infoBackground.color = redColor;
                    PlayerPrefs.SetInt("competition", 1);
                    PlayerPrefs.SetInt("competitionPin", 1);
                    okButton.OnClickAsObservable().Subscribe(_ => Controller.ShowProductsView()).AddTo(this);
                    break;
                case CompetitionState.Win:
                    titleText.text = string.Format(CompetitionTexts.CompetitionTitleWinText, product.Name);
                    okButtonText.text = CompetitionTexts.CompetitionOkWinText;
                    descriptionText.text = string.Format(CompetitionTexts.CompetitionDescriptionWinText, product.Name);
                    PlayerPrefs.SetInt("competition", 0);
                    PlayerPrefs.SetInt("competitionPin", 0);
                    infoBackground.color = greenColor;
                    okButton.OnClickAsObservable().Subscribe(_ => Controller.Close()).AddTo(this);
                    break;
                case CompetitionState.Lose:
                    titleText.text = CompetitionTexts.CompetitionTitleLoseText;
                    okButtonText.text =CompetitionTexts.CompetitionOkLoseText;
                    descriptionText.text = string.Format(CompetitionTexts.CompetitionDescriptionLoseText,product.Name);
                    PlayerPrefs.SetInt("competition", 0);
                    PlayerPrefs.SetInt("competitionPin", 0);
                    infoBackground.color = redColor;
                    okButton.OnClickAsObservable().Subscribe(_ => Controller.Close()).AddTo(this);
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