using Cysharp.Threading.Tasks;
using R3;
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

        private const string CompetitionOkText = "Modify Dilly Donut";
        private const string CompetitionOkWinText = "Awesome!";
        private const string CompetitionOkLoseText = "Bummer";

        private const string CompetitionTitleText = "You’ve Got Company!";
        private const string CompetitionTitleWinText = "{0} Is Back On Top!";
        private const string CompetitionTitleLoseText = "Customers Lost!";

        private const string CompetitionDescriptionText =
            "A competing Cooking company has come out with a new product called [Sunset Sprinkles]. It is [cheaper/better quality] than {0}. " +
            "/n/n  Tweak your product or lose customers!";

        private const string CompetitionDescriptionWinText =
            "[Sunset Sprinkles] can’t compete with your new price.  You retained your customers!";

        private const string CompetitionDescriptionLoseText = "[Dilly Donut]’s price could not compete with [Sunset Sprinkles]. They stole your customers!";


        [SerializeField] private Color redColor;
        [SerializeField] private Color greenColor;
        
        [SerializeField] private Button closeButton;
        [SerializeField] private Button okButton;
        [SerializeField] private TextMeshProUGUI okButtonText;
        [SerializeField] private Image infoBackground;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        
        
        

        public override UniTask Initialize(IPresenterData data = null)
        {
            closeButton.OnClickAsObservable().Subscribe(_ => Controller.Close()).AddTo(this);
            okButton.OnClickAsObservable().Subscribe(_ => Controller.Close()).AddTo(this);
            CompetitionState state = CompetitionState.Info;
            string productName = "Qaq";
            switch (state)
            {
                case CompetitionState.Info:
                    titleText.text = CompetitionTitleText;
                    okButtonText.text = CompetitionOkText;
                    descriptionText.text = string.Format(CompetitionDescriptionText, productName);
                    break;
                case CompetitionState.Win:
                    titleText.text = string.Format(CompetitionTitleWinText, productName);;
                    okButtonText.text = CompetitionOkWinText;
                    descriptionText.text = CompetitionDescriptionWinText;
                    break;
                case CompetitionState.Lose:
                    titleText.text = CompetitionTitleLoseText;
                    okButtonText.text = CompetitionOkLoseText;
                    descriptionText.text = CompetitionDescriptionLoseText;
                    break;
            }
            return UniTask.CompletedTask;
        }
    }
}