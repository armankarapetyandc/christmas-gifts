using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.UI.Asset.Database;
using SpaceMonkey.Scripts.UI.Components;
using SpaceMonkey.Scripts.UI.Views.Business.BusinessSetup.Panels.BusinessDetails.Items;
using SpaceMonkey.Scripts.Utilities.Validation;
using TMPro;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Business.BusinessPreview
{
    public class BusinessPreviewView : BasePresenterWithController<BusinessPreviewViewController>
    {
        [SerializeField] private Button backButton;
        [SerializeField] private Button saveButton;

        [SerializeField] private TMP_InputField nameInputField;
        [SerializeField] private IconComponent iconComponent;
        [SerializeField] private Button generateNameButton;
        [SerializeField] private HashtagVerticalListComponent hashtagVerticalList;

        public override UniTask Initialize(IPresenterData data = null)
        {
            var account = Controller.GetAccount();
            backButton.OnClickAsObservable().Subscribe(_ => Controller.OnBack()).AddTo(this);
            saveButton.OnClickAsObservable().Subscribe(_ => Controller.OnSave()).AddTo(this);
            nameInputField.onValueChanged.AsObservable().Subscribe(value => account.SetCompanyName(value)).AddTo(this);
            iconComponent.OnClick.Subscribe(_ => Controller.BuildLogo()).AddTo(this);
            hashtagVerticalList.SelectMore.Subscribe(_ => Controller.SelectMoreTags()).AddTo(this);

            SetupDefaults();

            Validator
                .Validate(
                    nameInputField.NotEmpty(),
                    nameInputField.MinLength(6),
                    iconComponent.Fulfilled,
                    hashtagVerticalList.Fulfilled
                )
                .BindButton(saveButton)
                .AddTo(this);

            return UniTask.CompletedTask;
        }

        private void SetupDefaults()
        {
            var account = Controller.GetAccount();

            nameInputField.text = account.Company.CompanyName;

            var shapeVisualAsset =
                Controller.ResolveVisualAsset<SpriteVisualAsset>(account.Company.Logo.ShapeVisualAssetId);
            var iconVisualAsset =
                Controller.ResolveVisualAsset<SpriteVisualAsset>(account.Company.Logo.IconVisualAssetId);
            var colorVisualAsset =
                Controller.ResolveVisualAsset<ColorVisualAsset>(account.Company.Logo.BackgroundColorVisualAssetId);

            iconComponent.SetShape(shapeVisualAsset);
            iconComponent.SetIcon(iconVisualAsset);
            iconComponent.SetColor(colorVisualAsset);

            hashtagVerticalList.Setup(Controller.GetAvailableHashtags(account.Company.Category), account.Company.Tags);
        }

        public override void Dispose()
        {
        }
    }
}