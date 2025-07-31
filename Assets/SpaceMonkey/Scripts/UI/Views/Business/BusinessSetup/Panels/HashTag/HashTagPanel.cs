using System;
using System.Collections.Generic;
using System.Linq;
using ObservableCollections;
using R3;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Business.BusinessSetup.Panels.HashTag
{
    public class HashTagPanel : MonoBehaviour
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

        public class Result
        {
            public string[] Tags { get; }

            public Result(string[] tags)
            {
                Tags = tags;
            }
        }

        [SerializeField] private HashTagsComponent hashTagsComponent;
        [SerializeField] private Slider meterSlider;
        [SerializeField] private Image meterSliderFill;
        [SerializeField] private TextMeshProUGUI meterText;
        [SerializeField] private Button saveButton;

        [Inject] private HashTagsConfig _hashTagsConfig;

        private readonly Dictionary<int, MeterState> _meterMap = new Dictionary<int, MeterState>
        {
            { 6, new MeterState("Meh...", "#FFB200") },
            { 12, new MeterState("Warmer...", "#FFB200") },
            { 19, new MeterState("AWESOME!", "#01C73D") },
        };

        private readonly ObservableHashSet<string> _selectedTags = new ObservableHashSet<string>();
        private readonly ReactiveCommand<Result> _saveCommand = new ReactiveCommand<Result>();
        internal Observable<Result> SaveCommand => _saveCommand;

        private void Start()
        {
            hashTagsComponent.Populate(_hashTagsConfig.Tags);
            hashTagsComponent.OnValueChanged.Subscribe(HashTagSelected).AddTo(this);
            _selectedTags.ObserveCountChanged().StartWithValue(_selectedTags.Count).Subscribe(SelectedTagsCountChanged)
                .AddTo(this);
            saveButton.OnClickAsObservable().Subscribe(_ => _saveCommand?.Execute(new Result(_selectedTags.ToArray())))
                .AddTo(this);
        }


        private void HashTagSelected((string label, bool state) tuple)
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

            // Find the appropriate range
            foreach (var key in sortedKeys)
            {
                if (value <= key)
                {
                    return (_meterMap[key], (float)value / hashTagsComponent.ItemsCount);
                }
            }

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
    }
}