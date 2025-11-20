using SpaceMonkey.Scripts.UI.Asset.Database;
using SpaceMonkey.Scripts.UI.Components;
using TMPro;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.BigOrder
{
    public class BigOrderItem:MonoBehaviour
    {
        [SerializeField] private IconComponent iconComponent;
        [SerializeField] private TextMeshProUGUI amountText;
        
        public void Set(SpriteVisualAsset iconVisualAsset,ColorVisualAsset colorVisualAsset)
        {
            iconComponent.SetIcon(iconVisualAsset);
            iconComponent.SetColor(colorVisualAsset);
        }

        public void SetAmount(float value)
        {
            amountText.text = value.ToString();
        }
    }
}