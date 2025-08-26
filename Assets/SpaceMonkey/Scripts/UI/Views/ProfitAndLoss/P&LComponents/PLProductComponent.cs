namespace SpaceMonkey.Scripts.UI.Views.ProfitAndLoss.P_LComponents
{
    public class PlProductComponent : PlComponent
    {
        public override void SetData(object data)
        {
            var product = (Profile.Product)data;
            SetComponentName(product.Name);
            ItemsList[0].SetItemData(1, product.MaterialPrice);
            ItemsList[1].SetItemData(1, product.MaterialPackagingPrice);
            ItemsList[2].SetItemData(1, product.ShippingCost);
            base.SetData(data);
        }
    }
}