using System;
using R3;
using SpaceMonkey.Scripts.Profile;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Business.BusinessSetup.Panels
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
        [SerializeField] private HashtagListItem[] hashtagItems;
        [SerializeField] private Button saveButton;

        private readonly ReactiveCommand<Unit> _saveCommand = new ReactiveCommand<Unit>();
        internal Observable<Unit> SaveCommand => _saveCommand;
        internal Observable<Unit> OnIconButtonClicked => iconBuilderButton.OnClickAsObservable();


        [Inject] private AccountService _accountService;
        
        private void Start()
        {
            saveButton.OnClickAsObservable().Subscribe(_ => SaveButtonClicked()).AddTo(this);
        }

        private void SaveButtonClicked()
        {
            _accountService.Account.SetCompanyName(businessNameInputField.text);
        }

        internal void SetCompanyLogo(Sprite shapeSprite, Sprite iconSprite, Color backgroundColor)
        {
            shapeImage.sprite = shapeSprite;
            iconImage.sprite = iconSprite;
            shapeImage.color = backgroundColor;

            shapeImage.gameObject.SetActive(true);
        }
    }
}