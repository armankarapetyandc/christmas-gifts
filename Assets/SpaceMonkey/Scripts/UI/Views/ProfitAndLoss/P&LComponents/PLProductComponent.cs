using System.Collections.Generic;

namespace SpaceMonkey.Scripts.UI.Views.ProfitAndLoss.P_LComponents
{
    public class PlProductComponent : PlComponent
    {
        public override void SetData(object data)
        {
            var pair = (KeyValuePair<Profile.Product, int>)data;
            SetComponentName(pair.Key.Name);
            ItemsList[0].SetItemData(pair.Value, pair.Key.MaterialPrice);
            ItemsList[1].SetItemData(pair.Value, pair.Key.MaterialPackagingPrice);
            ItemsList[2].SetItemData(1, pair.Key.ShippingCost);
            base.SetData(data);
        }
    }
}