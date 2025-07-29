using System.Collections.Generic;
using System.Linq;
using R3;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Asset.IconBuilder;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Business.BusinessSetup.Panels
{
    public class IconBuilderPanel : MonoBehaviour
    {
        public class Result
        {
            public Sprite ShapeSprite { get; }
            public Sprite IconSprite { get; }
            public Color BackgroundColor { get; }

            public Result(Sprite shapeSprite, Sprite iconSprite, Color backgroundColor)
            {
                ShapeSprite = shapeSprite;
                IconSprite = iconSprite;
                BackgroundColor = backgroundColor;
            }
        }

        [SerializeField] private Button backButton;
        [SerializeField] private Button saveButton;

        [SerializeField] private Image builderShapeImage;
        [SerializeField] private Image builderIconImage;

        [SerializeField] private List<ShapeItem> shapeItems;
        [SerializeField] private IconBuilderTab iconBuilderTab;
        [SerializeField] private IconBuilderConfig iconBuilderConfig;

        private readonly ReactiveCommand<Result> _saveCommand = new ReactiveCommand<Result>();
        public Observable<Result> SaveCommand => _saveCommand;

        private void Start()
        {
            for (var i = 0; i < shapeItems.Count; ++i)
            {
                shapeItems[i].Set(iconBuilderConfig.ShapesSprites[i]);
            }

            shapeItems.Select(item => item.SelectedShapeSprite).Merge().Subscribe(ShapeSelected).AddTo(this);
            iconBuilderTab.IconSelected.Subscribe(IconSelected).AddTo(this);
            iconBuilderTab.ColorSelected.Subscribe(ColorSelected).AddTo(this);
            iconBuilderTab.Initialize(iconBuilderConfig.IconSprites, iconBuilderConfig.BackgroundColors);
            saveButton.OnClickAsObservable().Subscribe(_ => OnSaveClicked()).AddTo(this);
        }

        private void OnSaveClicked()
        {
            _saveCommand.Execute(new Result(builderShapeImage.sprite, builderIconImage.sprite,
                builderShapeImage.color));
        }

        private void ShapeSelected(Sprite sprite)
        {
            builderShapeImage.gameObject.SetActive(true);
            builderShapeImage.sprite = sprite;
        }

        private void IconSelected(Sprite sprite)
        {
            builderIconImage.gameObject.SetActive(true);
            builderIconImage.sprite = sprite;
        }

        private void ColorSelected(Color color)
        {
            builderShapeImage.gameObject.SetActive(true);
            builderShapeImage.color = color;
        }
    }
}