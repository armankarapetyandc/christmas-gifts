using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using ObservableCollections;
using R3;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Views.Business.BusinessSetup.Panels.HashTag;
using SpaceMonkey.Scripts.Utilities;
using TMPro;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Business.BusinessHashtagsSelection
{
    public class BusinessHashtagsSelectionView : BasePresenterWithController<BusinessHashtagsSelectionViewController>
    {
        private struct MeterState
        {
            public string Label { get; set; }
            public Color Color { get; set; }

            public MeterState(string label, string htmlString)
            {
                Label = label;
                Color = ColorUtility.TryParseHtmlString(htmlString, out var color) ? color : Color.black;
            }
        }

        [SerializeField] private HashTagsComponent hashTagsComponent;
        [SerializeField] private Slider meterSlider;
        [SerializeField] private Image meterSliderFill;
        [SerializeField] private TextMeshProUGUI meterText;
        [SerializeField] private Button backButton;
        [SerializeField] private Button saveButton;

        private readonly Dictionary<int, MeterState> _meterMap = new Dictionary<int, MeterState>
        {
            { 1, new MeterState("Meh...", "#FFB200") },
            { 4, new MeterState("Warmer...", "#FFB200") },
            { 5, new MeterState("AWESOME!", "#01C73D") },
        };

        private readonly ObservableHashSet<Hashtag> _selectedTags = new ObservableHashSet<Hashtag>();

        public override UniTask Initialize(IPresenterData data = null)
        {
            backButton.OnClickAsObservable().Subscribe(_ => Controller.OnBack()).AddTo(this);
            saveButton.OnClickAsObservable().Subscribe(_ => Controller.Save(_selectedTags.ToArray())).AddTo(this);
            _selectedTags.ObserveCountChanged().StartWithValue(_selectedTags.Count).Subscribe(SelectedTagsCountChanged)
                .AddTo(this);
            SetupDefaults();
            return UniTask.CompletedTask;
        }

        private void SetupDefaults()
        {
            var account = Controller.GetAccount();
            var hashtags = Controller.GetAvailableHashtags(account.Company.Category);
            hashTagsComponent.Populate(hashtags).Subscribe(HashTagSelected).AddTo(this);

            var selectedTags = Controller.GetSelectedHashtags();
            hashTagsComponent.SetSelected(selectedTags, true);
        }

        private void HashTagSelected((Hashtag label, bool state) tuple)
        {
            if (tuple.state)
            {
                _selectedTags.Add(tuple.label);
                return;
            }

            _selectedTags.Remove(tuple.label);
        }

        private void SelectedTagsCountChanged(int value)
        {
            var state = GetMeterState(value);
            SetMeterValue(state.Item2, state.Item1.Label, state.Item1.Color);
        }

        private (MeterState, float) GetMeterState(int value)
        {
            if (value == 0)
                return (new MeterState(string.Empty, "#FFFFFF"), 0f);

            // Get the keys in ascending order
            var sortedKeys = _meterMap.Keys.OrderBy(k => k).ToList();

            if (value == 1) return (_meterMap[1], (float)value / hashTagsComponent.ItemsCount);
            if (value >= 2 && value <= 4) return (_meterMap[2], (float)value / hashTagsComponent.ItemsCount);
            return (_meterMap[5], (float)value / hashTagsComponent.ItemsCount);

            // If value is greater than the highest key (17+), return the last element
            return (_meterMap[sortedKeys.Last()], (float)value / hashTagsComponent.ItemsCount);
        }

        private void SetMeterValue(float progress, string label, Color color)
        {
            meterSlider.value = progress;
            meterText.text = label;
            meterText.color = color;
            meterSliderFill.color = color;
        }

        public override void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }
}