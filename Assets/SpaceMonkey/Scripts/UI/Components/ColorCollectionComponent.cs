using System.Collections.Generic;
using R3;
using SpaceMonkey.Scripts.UI.Asset.Database;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Components
{
    public class ColorCollectionComponent : MonoBehaviour
    {
        [SerializeField] private ColorCollectionItemComponent colorItemPrefab;
        [SerializeField] private RectTransform content;
        private readonly List<ColorCollectionItemComponent> _items = new List<ColorCollectionItemComponent>();

        public Observable<ColorVisualAsset> Setup(ColorVisualAsset[] assets)
        {
            while (_items.Count > 0)
            {
                Destroy(_items[0].gameObject);
            }

            _items.Clear();
            var observables = new List<Observable<ColorVisualAsset>>();
            foreach (ColorVisualAsset asset in assets)
            {
                var item = Instantiate(colorItemPrefab, content);
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