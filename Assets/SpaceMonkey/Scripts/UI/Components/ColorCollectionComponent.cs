using System.Collections.Generic;
using System.Linq;
using R3;
using SpaceMonkey.Scripts.UI.Asset.Database;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Components
{
    public class ColorCollectionComponent : MonoBehaviour
    {
        [SerializeField] private ColorCollectionItemComponent colorItemPrefab;
        [SerializeField] private RectTransform content;
        [SerializeField] private ToggleGroup toggleGroup;
        private readonly List<ColorCollectionItemComponent> _items = new List<ColorCollectionItemComponent>();
        public ColorCollectionItemComponent FirstItem => _items.First();
        
        public Observable<ColorVisualAsset> Setup(ColorVisualAsset[] assets)
        {
            while (_items.Count > 0)
            {
                Destroy(_items[0].gameObject);
                _items.RemoveAt(0);
            }
            var observables = new List<Observable<ColorVisualAsset>>();
            foreach (ColorVisualAsset asset in assets)
            {
                var item = Instantiate(colorItemPrefab, content);
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