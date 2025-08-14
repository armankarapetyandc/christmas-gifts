using System.Linq;
using NameGenerator.Generators;
using R3;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Views.Business.BusinessSetup.Panels.BusinessDetails.Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Business.BusinessSetup.Panels.BusinessDetails
{
    public class BusinessDetailsPanel : MonoBehaviour
    {
        [SerializeField] private TMP_InputField businessNameInputField;
        [SerializeField] private Image shapeImage;
        [SerializeField] private Image iconImage;
        [SerializeField] private Button generateBusinessNameButton;
        [SerializeField] private Button iconBuilderButton;
        [SerializeField] private Button hashTagPanelButton;
        [SerializeField] private TextMeshProUGUI hashtagsCountText;
        [SerializeField] private HashtagListItem hashtagListItemPrefab;
        [SerializeField] private RectTransform hashtagsContainer;
        [SerializeField] private Button saveButton;
        [SerializeField] private TextMeshProUGUI saveButtonText;
        [SerializeField] private Color enableSaveButtonColor;

        private readonly GamerTagGenerator _gamerTagGenerator = new GamerTagGenerator();

        private readonly ReactiveCommand<Unit> _saveCommand = new ReactiveCommand<Unit>();
        internal Observable<Unit> SaveCommand => _saveCommand;
        internal Observable<Unit> OnIconButtonClicked => iconBuilderButton.OnClickAsObservable();
        internal Observable<Unit> OnHashtagButtonClicked => hashTagPanelButton.OnClickAsObservable();


        [Inject] private AccountService _accountService;
        [Inject] private GameConfig _gameConfig;

        private void Start()
        {
            generateBusinessNameButton.OnClickAsObservable().Subscribe(_ => GenerateName()).AddTo(this);
            saveButton.OnClickAsObservable().Subscribe(_ => SaveButtonClicked()).AddTo(this);
            businessNameInputField.onValueChanged.AsObservable().Subscribe(_ => CheckPointsForSaveButton()).AddTo(this);
        }

        private void GenerateName()
        {
            businessNameInputField.text = _gamerTagGenerator.Generate();
            CheckPointsForSaveButton();
        }

        private void SaveButtonClicked()
        {
            _accountService.Model.Account.SetCompanyName(businessNameInputField.text);
        }

        internal void SetCompanyLogo(Sprite shapeSprite, Sprite iconSprite, Color backgroundColor)
        {
            shapeImage.sprite = shapeSprite;
            iconImage.sprite = iconSprite;
            shapeImage.color = backgroundColor;

            shapeImage.gameObject.SetActive(true);
            CheckPointsForSaveButton();
        }

        public void SetHashTags(string category, Hashtag[] tags)
        {
            while (hashtagsContainer.childCount > 0)
            {
                Destroy(hashtagsContainer.GetChild(0).gameObject);
            }

            var totalTagsInCategory =
                _gameConfig.Categories.SingleOrDefault(info => info.Name == category)!.Tags.Length;

            foreach (Hashtag hashtag in tags)
            {
                var item = Instantiate(hashtagListItemPrefab, hashtagsContainer);
                // item.Set(hashtag);
            }

            hashtagsCountText.text = $"{tags.Length}/{totalTagsInCategory}";
            CheckPointsForSaveButton();
        }

        private void CheckPointsForSaveButton()
        {
            saveButton.interactable = businessNameInputField.text.Length > 0 && iconImage.sprite != null &&
                                      hashtagsContainer.childCount > 0;
            saveButtonText.color = saveButton.interactable ? enableSaveButtonColor : Color.white;
            
        }
    }
}