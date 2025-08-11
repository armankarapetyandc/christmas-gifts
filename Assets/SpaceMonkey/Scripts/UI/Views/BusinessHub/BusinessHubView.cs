using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.Profile;
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
        [SerializeField] private Image shapeImage;
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI businessName;
        [SerializeField] private TextMeshProUGUI weekNumber;
        [SerializeField] private TextMeshProUGUI levelNumber;
        
        [SerializeField] private TextMeshProUGUI moneyText;
        [SerializeField] private TextMeshProUGUI prodCapText;
        [SerializeField] private TextMeshProUGUI scoreText;
        
        
        [SerializeField] private Button startButton;
        [SerializeField] private Button productButton;
        [SerializeField] private TextMeshProUGUI productsCountText;
        
        [Inject] private AccountService _accountService;
        
        public override UniTask Initialize(IPresenterData data = null)
        {
            shapeImage.sprite = Controller.ShapeSprite();
            iconImage.sprite = Controller.IconSprite();
            shapeImage.color = Controller.ShapeColor();
            businessName.text = Controller.GetBusinessName();
            levelNumber.text = $"Level {_accountService.Model.Account.Level.ToString()}";
            moneyText.text = $"${_accountService.Model.Account.Money.ToString()}";
            prodCapText.text = $"{_accountService.Model.ProductionCapacity.ToString()} hrs";
            scoreText.text = _accountService.Model.Account.Score.ToString();
            
            productButton.OnClickAsObservable().Subscribe(_ => Controller.ShowProductView()).AddTo(this);
            productsCountText.text = $"Products({_accountService.Model.Account.Products.Count.ToString()})";
            return UniTask.CompletedTask;
        }
        

        public override void Dispose()
        {
        }
    }
}