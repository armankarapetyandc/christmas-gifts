using System.Collections.Generic;
using System.Linq;
using R3;
using SpaceMonkey.Scripts.UI.Views.Business.BusinessSetup.Panels.HashTag.Items;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Business.BusinessSetup.Panels.HashTag
{
    public class HashTagsComponent : MonoBehaviour
    {
        [SerializeField] private HashTagItem itemPrefab;
        [SerializeField] private RectTransform container;
        [SerializeField] private HorizontalFlowLayoutGroup flowLayoutGroup;

        public Observable<(string, bool)> OnValueChanged => _items.Select(item => item.OnValueChanged).Merge();
        
        private readonly List<HashTagItem> _items = new List<HashTagItem>();

        public int ItemsCount => _items.Count;
        
        public void Populate(string[] tags)
        {
            foreach (var hashtag in tags)
            {
                var item = Instantiate(itemPrefab, container);
                item.SetText(hashtag);
                _items.Add(item);
            }
            AdjustItems();
        }

        private void AdjustItems()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(container);

            var contentWidth = container.rect.width;
            var horizontalSpacing = flowLayoutGroup.spacing;

            var groups = GroupByThreshold(_items, contentWidth, horizontalSpacing);
            foreach (List<HashTagItem> group in groups)
            {
                var groupTotalWidth = group.Select(item => item.RectTransform.rect.width).Sum();
                var remainingTotal = contentWidth - groupTotalWidth - horizontalSpacing * (group.Count - 1);
                var remaining = Mathf.Floor(remainingTotal / group.Count);
                group.ForEach(item => item.LayoutElement.preferredWidth = item.RectTransform.rect.width + remaining);
            }
        }

        private List<List<HashTagItem>> GroupByThreshold(List<HashTagItem> items, float threshold, float spacing)
        {
            var result = new List<List<HashTagItem>>();
            var currentGroup = new List<HashTagItem>();
            float currentSum = 0f;
            int itemCount = 0;

            foreach (var item in items)
            {
                float itemWidth = item.RectTransform.rect.width;

                // Calculate the potential sum if we add this item
                // total width = sum of item widths + spacing * (itemCount)  (spacing between items)
                float potentialSum = currentSum + itemWidth + (itemCount > 0 ? spacing : 0f);

                if (potentialSum <= threshold)
                {
                    currentGroup.Add(item);
                    currentSum += itemWidth + (itemCount > 0 ? spacing : 0f);
                    itemCount++;
                }
                else
                {
                    if (currentGroup.Count > 0)
                        result.Add(currentGroup);

                    currentGroup = new List<HashTagItem> { item };
                    currentSum = itemWidth;
                    itemCount = 1;
                }
            }

            if (currentGroup.Count > 0)
                result.Add(currentGroup);

            return result;
        }
    }
}