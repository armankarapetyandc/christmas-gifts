using SpaceMonkey.Scripts.UI.Views.Product;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Views.Marketing
{
    public class MarketingSlider : AbstractPriceSlider
    {
        [SerializeField] private GameObject fillArea;
        [SerializeField] private GameObject handleSlideArea;

        public void ChangeSliderActivation(bool isActive)
        {
            fillArea.SetActive(isActive);
            handleSlideArea.SetActive(isActive);
        }
    }
}