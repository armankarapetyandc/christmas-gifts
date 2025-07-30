using Cysharp.Threading.Tasks;
using R3;
using TMPro;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Business.BusinessSetupCelebration
{
    public class BusinessSetupCelebrationView : BasePresenterWithController<BusinessSetupCelebrationController>
    {
        [SerializeField] private Button infoButton;
        [SerializeField] private Button backButton;
        [SerializeField] private Button nextButton;
        
        [SerializeField] private Image shapeImage;
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI businessName;
        
        public override UniTask Initialize(IPresenterData data = null)
        {
            if (Controller == null)
            {
                Debug.LogError("Error, something is wrong");
                return UniTask.CompletedTask;
            }
            backButton.OnClickAsObservable().Subscribe(_ => Controller.OnBack()).AddTo(this);
            shapeImage.sprite = Controller.ShapeSprite();
            iconImage.sprite = Controller.IconSprite();
            shapeImage.color = Controller.ShapeColor();
            businessName.text = Controller.GetBusinessName();
            nextButton.OnClickAsObservable().Subscribe(_ => Controller.OnNext()).AddTo(this);
            return UniTask.CompletedTask;
        }

        public override void Dispose()
        {
        }
    }
}