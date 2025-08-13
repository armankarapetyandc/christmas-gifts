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
        [SerializeField] private SpriteVisualAsset[] iconsAssets;


        public Observable<SpriteVisualAsset> Setup()
        {
            var observables = new List<Observable<SpriteVisualAsset>>();
            foreach (SpriteVisualAsset asset in iconsAssets)
            {
                var item = Instantiate(iconItemPrefab, content);
                item.Setup(asset);
                observables.Add(item.OnSelected);
            }

            return observables.Merge();
        }
    }
}