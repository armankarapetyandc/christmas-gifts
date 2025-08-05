using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Views.Product;

namespace SpaceMonkey.Scripts.UI.Extensions
{
    public static class ProductExtensions
    {
        public static (float, float) PriceCoefficients(SliderType sliderType)
        {
            switch (sliderType)
            {
                case SliderType.Materials:
                    return (Constants.MaterialsCoefficientsMin,  Constants.MaterialsCoefficientsMax);
                case SliderType.Packaging:
                    return (Constants.PackagingCoefficientsMin,   Constants.PackagingCoefficientsMax);
                default:
                    return (0, 0);
            }
        }
    }
}