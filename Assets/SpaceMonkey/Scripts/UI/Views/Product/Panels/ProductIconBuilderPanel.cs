using System;
using R3;
using SpaceMonkey.Scripts.UI.Asset.Product;
using SpaceMonkey.Scripts.UI.Views.Business.BusinessSetup.Panels.IconBuilder;
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
     
        [SerializeField] private Image builderIconImage;
        [SerializeField] private Image backgroundImage;
        
        [SerializeField] private IconBuilderTab iconBuilderTab;
        [SerializeField] private Sprite defaultIconSpace;
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
        }
        
        private void ColorSelected(Color color)
        {
            backgroundImage.gameObject.SetActive(true);
            backgroundImage.color = color;
        }

        public void Reset()
        {
            builderIconImage.gameObject.SetActive(false);
            iconHolder.sprite = defaultIconSpace;
            backgroundImage.sprite = null;
            backgroundImage.color = Color.white;
        }
    }
}