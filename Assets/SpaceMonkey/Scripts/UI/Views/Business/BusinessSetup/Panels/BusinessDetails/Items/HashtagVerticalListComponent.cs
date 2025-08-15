using System;
using System.Collections.Generic;
using System.Linq;
using R3;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Profile;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Business.BusinessSetup.Panels.BusinessDetails.Items
{
    public class HashtagVerticalListComponent : MonoBehaviour
    {
        [SerializeField] private HashtagListItem listItemPrefab;
        [SerializeField] private RectTransform container;
        [SerializeField] private TextMeshProUGUI countText;
        [SerializeField] private Button moreButton;

        private readonly List<HashtagListItem> _items = new List<HashtagListItem>();

        public Observable<Unit> SelectMore => moreButton.OnClickAsObservable();

        public Observable<bool> Fulfilled =>
            Observable.CombineLatest(_items.Select(item => item.Selected))
                .Select(selectedArray => selectedArray.Count(isSelected => isSelected) >= 1)
                .DistinctUntilChanged();

        public void Setup(HashtagInfo[] availableTags, Hashtag[] selectedTags)
        {
            countText.text = $"{selectedTags.Length}/{availableTags.Length}";
            while (_items.Count > 0)
            {
                Destroy(_items[0].gameObject);
            }

            _items.Clear();
            foreach (HashtagInfo tag in availableTags)
            {
                var item = Instantiate(listItemPrefab, container);
                item.Set(tag.Tag);
                var selectedTag = selectedTags.FirstOrDefault(t => t.Tag.Equals(tag.Tag));
                if (selectedTag != null)
                {
                    item.Select();
                }
                else
                {
                    item.Deselect();
                }

                _items.Add(item);
            }
        }
    }
}