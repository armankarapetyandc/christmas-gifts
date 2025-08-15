using System.Collections.Generic;
using R3;
using SpaceMonkey.Scripts.UI.Asset.Database;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Components
{
    public class IconCollectionComponent : MonoBehaviour
    {
        [SerializeField] private IconCollectionItemComponent iconItemPrefab;
        [SerializeField] private RectTransform content;

        private readonly List<IconCollectionItemComponent> _items = new List<IconCollectionItemComponent>();

        public Observable<SpriteVisualAsset> Setup(SpriteVisualAsset[] assets)
        {
            while (_items.Count > 0)
            {
                Destroy(_items[0].gameObject);
            }

            _items.Clear();
            var observables = new List<Observable<SpriteVisualAsset>>();
            foreach (SpriteVisualAsset asset in assets)
            {
                var item = Instantiate(iconItemPrefab, content);
                item.Setup(asset);
                observables.Add(item.OnSelected);
                _items.Add(item);
            }

            return observables.Merge();
        }

        public void Select(string visualAssetId)
        {
            if (string.IsNullOrWhiteSpace(visualAssetId))
            {
                return;
            }

            var item = _items.Find(i => i.VisualAsset.Id.Equals(visualAssetId));
            if (item != null)
            {
                item.SetStateWithoutNotify(true);
            }
        }
    }
}