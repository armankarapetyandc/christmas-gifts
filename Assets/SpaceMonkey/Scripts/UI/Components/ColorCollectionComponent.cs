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

        public Observable<ColorVisualAsset> Setup(ColorVisualAsset[] assets)
        {
            var observables = new List<Observable<ColorVisualAsset>>();
            foreach (ColorVisualAsset asset in assets)
            {
                var item = Instantiate(colorItemPrefab, content);
                item.Setup(asset);
                observables.Add(item.OnSelected);
            }

            return observables.Merge();
        }
    }
}