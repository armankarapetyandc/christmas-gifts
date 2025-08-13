using R3;
using SpaceMonkey.Scripts.UI.Asset.Product;
using SpaceMonkey.Scripts.UI.Views.Business.BusinessSetup.Panels.IconBuilder;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Product.Panels
{
    public class ProductIconBuilderPanel : MonoBehaviour
    {
        public class Result
        {
            public Sprite IconSprite { get; }
            public Color BackgroundColor { get; }

            public Result(Sprite iconSprite, Color backgroundColor)
            {
                IconSprite = iconSprite;
                BackgroundColor = backgroundColor;
            }
        }
        
        //[SerializeField] private Button backButton;
        [SerializeField] private Button saveButton;
        [SerializeField] private TextMeshProUGUI saveButtonText;
        [SerializeField] private Color selectedSaveTextColor;
     
        [SerializeField] private Image builderIconImage;
        [SerializeField] private Image backgroundImage;
        
        [SerializeField] private IconBuilderTab iconBuilderTab;
        [SerializeField] private Sprite defaultIconSprite;
        [SerializeField] private Image iconHolder;
        
        [Inject] private ProductIconBuilderConfig _iconBuilderConfig;
        
        private readonly ReactiveCommand<Result> _saveCommand = new ReactiveCommand<Result>();
        public Observable<Result> SaveCommand => _saveCommand;

        private void Start()
        {
            iconBuilderTab.Initialize(_iconBuilderConfig.IconSprites, _iconBuilderConfig.BackgroundColors);
            iconBuilderTab.IconSelected.Subscribe(IconSelected).AddTo(this);
            iconBuilderTab.ColorSelected.Subscribe(ColorSelected).AddTo(this);
            saveButton.OnClickAsObservable().Subscribe(_ => OnSaveClicked()).AddTo(this);
        }
        
        private void OnSaveClicked()
        {
            _saveCommand.Execute(new Result(builderIconImage.sprite,
                backgroundImage.color));
        }
        
        private void IconSelected(Sprite sprite)
        {
            builderIconImage.gameObject.SetActive(true);
            builderIconImage.sprite = sprite;
            CheckPointsForSaveButton();
        }
        
        private void ColorSelected(Color color)
        {
            backgroundImage.gameObject.SetActive(true);
            backgroundImage.color = color;
        }

        public void Reset()
        {
            builderIconImage.gameObject.SetActive(false);
            iconHolder.sprite = defaultIconSprite;
            backgroundImage.sprite = defaultIconSprite;
            backgroundImage.color = Color.white;
        }
        
        private void CheckPointsForSaveButton()
        {
            saveButton.interactable = builderIconImage.sprite != null;
            saveButtonText.color = saveButton.interactable ? selectedSaveTextColor : Color.white;
            
        }
    }
}