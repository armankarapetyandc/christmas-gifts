using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.Configs;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.BusinessExamples
{
    public class BusinessExamplesView : BasePresenterWithController<BusinessExamplesController>
    {
        public class Data : IPresenterData
        {
            public string Category { get; internal set; }
        }

        [SerializeField] private Button backButton;
        [SerializeField] private BusinessExamplesItem businessExamplesItemPrefab;
        [SerializeField] private RectTransform container;
        [SerializeField] private ExampleTypeTab[] exampleTypeTabs;

        private readonly List<BusinessExamplesItem> _businessExamplesItems = new List<BusinessExamplesItem>();
        private readonly List<Observable<BusinessExample>> _observables = new List<Observable<BusinessExample>>();
        private Data _data;

        public override UniTask Initialize(IPresenterData data = null)
        {
            _data = data as Data;
            backButton.OnClickAsObservable().Subscribe(_ => Controller.OnBack());
            Reset();
            InitBusinessExamples();

            exampleTypeTabs
                .Select(tab => tab.OnSelected)
                .Merge()
                .Subscribe(SelectedBusinessExamples)
                .AddTo(this);

            exampleTypeTabs
                .Where(category =>
                    _data != null ? category.Category == _data.Category : category.Category == "Cooking")
                .ToList()
                .ForEach(category => category.toggle.isOn = true);

            return UniTask.CompletedTask;
        }

        private void Reset()
        {
            foreach (var item in _businessExamplesItems)
            {
                Destroy(item);
            }

            _businessExamplesItems.Clear();
        }

        private void InitBusinessExamples()
        {
            var businessExamples = Controller.GetBusinessExamples();
            if (businessExamples == null || businessExamples.Length == 0) return;
            foreach (var example in businessExamples)
            {
                var item = Instantiate(businessExamplesItemPrefab, container);
                item.Setup(example);
                _businessExamplesItems.Add(item);
            }
        }

        private void UpdateExamples(string selected)
        {
            _observables.Clear();
            foreach (var example in _businessExamplesItems)
            {
                if (example.Category.Equals(selected))
                {
                    _observables.Add(example.OnSelected);
                    example.gameObject.SetActive(true);
                }
                else
                {
                    example.gameObject.SetActive(false);
                }
            }
        }


        private void SelectedBusinessExamples(string selectedCategory)
        {
            UpdateExamples(selectedCategory);
            _observables.Merge().Subscribe(example => Controller.OnMoreClick(example, selectedCategory));
        }

        public override void Dispose()
        {
        }
    }
}