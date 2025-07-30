using System;
using R3;
using SpaceMonkey.Scripts.UI.Asset.BusinessIdeas;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Business.CategorySelection
{
    public class IdeaItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private Image iconImage;
        [SerializeField] private Button button;
        
        private BusinessIdeaAsset _ideaAsset;

        public Observable<BusinessIdeaAsset> Selected => button.OnClickAsObservable().Select(_ => _ideaAsset);

        public bool HasIdea => _ideaAsset != null;


        internal void Set(BusinessIdeaAsset ideaAsset)
        {
            _ideaAsset = ideaAsset;
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (_ideaAsset == null)
            {
                throw new Exception("IdeaInfo is null");
            }

            titleText.text = _ideaAsset.BusinessIdeaName;
            iconImage.sprite = _ideaAsset.BusinessIdeaItemSprite;
        }
    }
}