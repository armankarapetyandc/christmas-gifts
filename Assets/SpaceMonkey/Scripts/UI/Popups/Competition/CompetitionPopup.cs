using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.Profile;
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

        private Data _viewData;


        public override UniTask Initialize(IPresenterData data = null)
        {
            _viewData = data as Data;
            closeButton.OnClickAsObservable().Subscribe(_ => Controller.Close()).AddTo(this);
            okButton.OnClickAsObservable().Subscribe(_ => Controller.Close()).AddTo(this);
            CompetitionState state = CompetitionState.Info;
            if (PlayerPrefs.GetInt("competition") == 1)
            {
                if (_viewData != null)
                {
                    state = _viewData.Product.ProductPrice == null ? CompetitionState.Win : CompetitionState.Lose;
                }
            }
            switch (state)
            {
                case CompetitionState.Info:
                    titleText.text = CompetitionTexts.CompetitionTitleText;
                    okButtonText.text = CompetitionTexts.CompetitionOkText;
                    descriptionText.text = string.Format(CompetitionTexts.CompetitionDescriptionText, _viewData?.Product.Name);
                    infoBackground.color = redColor;
                    PlayerPrefs.SetInt("competition", 1);
                    PlayerPrefs.SetString("productId", _viewData?.Product.Id);
                    break;
                case CompetitionState.Win:
                    var product = Controller.GetCompetitionProduct();
                    titleText.text = string.Format(CompetitionTexts.CompetitionTitleWinText, product.Name);
                    okButtonText.text = CompetitionTexts.CompetitionOkWinText;
                    descriptionText.text = CompetitionTexts.CompetitionDescriptionWinText;
                    PlayerPrefs.SetInt("competition", 0);
                    infoBackground.color = greenColor;
                    break;
                case CompetitionState.Lose:
                    titleText.text = CompetitionTexts.CompetitionTitleLoseText;
                    okButtonText.text =CompetitionTexts.CompetitionOkLoseText;
                    descriptionText.text = string.Format(CompetitionTexts.CompetitionDescriptionLoseText,_viewData?.Product.Name);
                    PlayerPrefs.SetInt("competition", 0);
                    infoBackground.color = redColor;
                    break;
            }
            return UniTask.CompletedTask;
        }

        public class Data : IPresenterData
        {
            public Product Product { get; set; }
        }
    }
}