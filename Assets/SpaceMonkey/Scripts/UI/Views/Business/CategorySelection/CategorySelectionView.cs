using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using R3;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.UI.Components;
using SpaceMonkey.Scripts.Utilities;
using TMPro;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Business.CategorySelection
{
    public class CategorySelectionView : BasePresenterWithController<CategorySelectionController>
    {
        [Header("General")] [SerializeField] private Button backButton;
        [SerializeField] private Button infoButton;
        [SerializeField] private RectTransform categorySelectionPanel;
        [SerializeField] private RectTransform categoryDetailsPanel;

        [Header("Category Selection")] [SerializeField]
        private CategoryItem[] categoryItems;

        [SerializeField] private CanvasGroup categorySelectionCanvasGroup;


        [Header("Category Details")] [SerializeField]
        private Image selectedIdeaIconImage;

        [SerializeField] private TextMeshProUGUI selectedIdeaNameText;
        [SerializeField] private Button nextButton;
        [SerializeField] private CanvasGroup categoryDetailsCanvasGroup;

        [Header("Tutorial")]
        [SerializeField] private RectTransform labelText;

        [SerializeField] private Image tutorialCategory;

        private readonly ReactiveCommand _categorySelected = new ReactiveCommand();

        public Observable<Unit> CategorySelectedObservable => _categorySelected;
        
        public RectTransform LabelText => labelText;
        public Image TutorialCategory => tutorialCategory;
        
        public Button NextButton => nextButton;

        public override UniTask Initialize(IPresenterData data = null)
        {
            backButton.OnClickAsObservable().Subscribe(_ => OnBackButtonClicked()).AddTo(this);
            infoButton.OnClickAsObservable().Subscribe(_ => OnInfoButtonClicked()).AddTo(this);
            nextButton.OnClickAsObservable().Subscribe(async _ =>
            {
                var value=Controller.GetScoreFor("Category");
                await XPParticleEffector.SpawnXpParticles(value, new Vector2(Screen.width/2f, Screen.height/2f), transform);
                Controller.OnNext();
            }).AddTo(this);
            SetupCategories();
            return UniTask.CompletedTask;
        }


        private void OnInfoButtonClicked()
        {
        }

        private void OnBackButtonClicked()
        {
            if (categorySelectionPanel.gameObject.activeSelf)
            {
                Controller.Back();
                return;
            }

            if (categoryDetailsPanel.gameObject.activeSelf)
            {
                categoryDetailsCanvasGroup.DOCrossfade(categorySelectionCanvasGroup, 0.5f, Ease.InOutQuad).Forget();
            }
        }


        private void SetupCategories()
        {
            var categories = Controller.RetrieveIdeas();
            var count = Mathf.Min(categoryItems.Length, categories.Length);
            for (int i = 0; i < count; i++)
            {
                var item = categoryItems[i];
                item.Set(categories[i]);
            }

            categoryItems
                .Where(item => item.HasIdea)
                .Select(item => item.Selected)
                .Merge()
                .Subscribe(CategorySelected)
                .AddTo(this);
        }

        private void CategorySelected(CategoryInfo category)
        {
            _categorySelected.Execute(Unit.Default);
            Controller.IdeaSelected(category.Name);
            selectedIdeaNameText.text = category.Name;
            selectedIdeaIconImage.sprite = category.Visual.Sprite;
            categorySelectionCanvasGroup.DOCrossfade(categoryDetailsCanvasGroup, 0.5f, Ease.InOutQuad).Forget();
        }

        public override void Dispose()
        {
        }
    }
}