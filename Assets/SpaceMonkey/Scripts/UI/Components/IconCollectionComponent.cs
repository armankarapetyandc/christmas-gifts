using System.Collections.Generic;
using R3;
using SpaceMonkey.Scripts.UI.Asset.Database;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Components
{
    public class IconCollectionComponent : MonoBehaviour
    {
        [SerializeField] private IconCollectionItemComponent iconItemPrefab;
        [SerializeField] private RectTransform content;
        [SerializeField] private ToggleGroup toggleGroup;

        private readonly List<IconCollectionItemComponent> _items = new List<IconCollectionItemComponent>();

        public Observable<SpriteVisualAsset> Setup(SpriteVisualAsset[] assets)
        {
            while (_items.Count > 0)
            {
                Destroy(_items[0].gameObject);
                _items.RemoveAt(0);
            }

            var observables = new List<Observable<SpriteVisualAsset>>();
            foreach (SpriteVisualAsset asset in assets)
            {
                var item = Instantiate(iconItemPrefab, content);
                observables.Add(item.OnSelected);
                item.SetToggleGroup(toggleGroup);
                item.Setup(asset);
                _items.Add(item);
            }

            return observables.Merge();
        }

        public void EnsureValidState()
        {
            toggleGroup.EnsureValidState();
        }

        public void Select(string visualAssetId, bool forceNotify)
        {
            if (string.IsNullOrWhiteSpace(visualAssetId))
            {
                return;
            }

            var item = _items.Find(i => i.VisualAsset.Id.Equals(visualAssetId));
            if (item != null)
            {
                if (forceNotify)
                {
                    item.SetState(true);
                }
                else
                {
                    item.SetStateWithoutNotify(true);
                }
            }
        }
    }
}