using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Views.Product
{
    public class ProductData
    {
        public string Name { get; set; }
        public float TotalCost { get; set; }
        public float ShippingCost { get; set; }
        public float Price { get; set; }
        public float TtpCost { get; set; }
        public float PackagingCost { get; set; }
        public float MaterialCost { get; set; }
        public float Profit {get; set;}
        public Sprite Icon { get; set; }
        public Color BackgroundColor { get; set; }
        public float TimeToProduceIndex { get; set; }
        
        public void Reset()
        {
            TotalCost = 0;
            ShippingCost = 0;
            Price = 0;
            TtpCost = 0;
            Profit = 0;
            Name = string.Empty;
            Icon = null;
            BackgroundColor = Color.white;
            TimeToProduceIndex = 0;
            PackagingCost = 0;
            MaterialCost = 0;
        }
    }
}