using System.Linq;
using Cysharp.Threading.Tasks;
using R3;
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
        private IdeaItem[] ideaItems;


        [Header("Category Details")] [SerializeField]
        private Image selectedIdeaIconImage;

        [SerializeField] private TextMeshProUGUI selectedIdeaNameText;
        [SerializeField] private Button nextButton;


        public override UniTask Initialize(IPresenterData data = null)
        {
            backButton.OnClickAsObservable().Subscribe(_ => OnBackButtonClicked()).AddTo(this);
            infoButton.OnClickAsObservable().Subscribe(_ => OnInfoButtonClicked()).AddTo(this);
            nextButton.OnClickAsObservable().Subscribe(_ => Controller.OnNext()).AddTo(this);
            SetupIdeas();
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
                categoryDetailsPanel.gameObject.SetActive(false);
                categorySelectionPanel.gameObject.SetActive(true);
            }
        }

        private void SetupIdeas()
        {
            var ideas = Controller.RetrieveIdeas();
            var count = Mathf.Min(ideaItems.Length, ideas.Count);
            for (int i = 0; i < count; i++)
            {
                var item = ideaItems[i];
                item.Set(ideas[i]);
            }

            ideaItems
                .Where(item => item.HasIdea)
                .Select(item => item.Selected)
                .Merge()
                .Subscribe(IdeaSelected)
                .AddTo(this);
        }

        private void IdeaSelected(IdeaInfo ideaInfo)
        {
            Controller.IdeaSelected(ideaInfo);
            selectedIdeaIconImage.sprite = ideaInfo.Icon;
            selectedIdeaNameText.text = ideaInfo.Name;
            categoryDetailsPanel.gameObject.SetActive(true);
            categorySelectionPanel.gameObject.SetActive(false);
        }

        public override void Dispose()
        {
        }
    }
}