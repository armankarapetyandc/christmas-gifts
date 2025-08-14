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

        public Observable<Unit> SelectMore => moreButton.OnClickAsObservable();
        
        public void Setup(HashtagInfo[] availableTags, Hashtag[] selectedTags)
        {
            countText.text = $"{selectedTags.Length}/{availableTags.Length}";
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
            }
        }
    }
}