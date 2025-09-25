using R3;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Profile;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Marketing
{
    public class MarketingItem : MonoBehaviour
    {
        [SerializeField] private Toggle toggle;
        [SerializeField] private MarketingSlider marketingSlider;

        public MarketingSlider MarketingSlider => marketingSlider;
        public bool IsSelected => toggle.isOn;

        public MarketingInfo MarketingItemInfo { get; private set; }

        private MarketingFeature _marketingFeature;

        public void Initialize(MarketingInfo info,int level, MarketingFeature feature)
        {
            _marketingFeature = feature;
            MarketingItemInfo = info;
            if (feature.Id != null)
            {
                toggle.isOn = true;
            }
            
            if (toggle != null)
                toggle.OnValueChangedAsObservable().Subscribe(SliderValueChanged).AddTo(this);
            
            marketingSlider.Setup(MarketingItemInfo.MinMult, MarketingItemInfo.MaxMult,
                level, MarketingItemInfo.Id, feature.CurrentPrice, feature.Id != null );
            
        }

        private void SliderValueChanged(bool selected)
        {
            if (marketingSlider != null)
                marketingSlider.ChangeSliderActivation(selected);
        }
    }
}