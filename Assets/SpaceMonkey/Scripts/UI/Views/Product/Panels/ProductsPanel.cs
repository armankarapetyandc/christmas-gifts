
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Product.Panels
{
    public class ProductsPanel : MonoBehaviour
    {
        [SerializeField] private ProductItem productPrefab;
        [SerializeField] private RectTransform productsContainer;
        [SerializeField] private Button addButton;

        // private readonly ObservableList<ProductItem> _products = new ObservableList<ProductItem>();
        //
        // private Observable<ProductData> _selectedProductDataObservable = Observable.Empty<ProductData>();
        // public Observable<ProductData> SelectedProductData => _selectedProductDataObservable;
        //
        // internal Observable<Unit> OnAddButtonClicked => addButton.OnClickAsObservable();
        // [Inject] private AccountService _accountService;
        // [Inject] private ProductIconBuilderConfig _iconBuilderConfig;
        //
        // private void Start()
        // {
        //     foreach (var product in _accountService.Model.Account.Products)
        //     {
        //         AddProduct(new ProductData
        //         {
        //             ID = product.Id,
        //             Name = product.Name,
        //             Price = product.Price,
        //             TtpCost = product.TtpCost,
        //             TotalCost = product.TotalCost,
        //             ShippingCost = product.ShippingCost,
        //             PackagingCost = product.PackagingCost,
        //             MaterialCost = product.MaterialCost,
        //             Profit = product.Profit,
        //             TimeToProduceIndex = product.TimeToProduceIndex,
        //             Icon = Icon(product.IconVisualAssetId),
        //             BackgroundColor = BackgroundColor(product.BackgroundColor)
        //         });
        //     }
        // }
        //
        // public void DeleteProduct(ProductData productData)
        // {
        //     // var productToDelete = _products.FirstOrDefault(p => p.ProductData.ID == productData.ID);
        //     // if (productToDelete != null)
        //     // {
        //     //     if (productToDelete.gameObject != null)
        //     //     {
        //     //         Destroy(productToDelete.gameObject);
        //     //     }
        //     //
        //     //     _products.Remove(productToDelete);
        //     // }
        // }
        //
        //
        // private Color BackgroundColor(string backColor)
        // {
        //     return ColorUtility.TryParseHtmlString("#" + backColor, out var color)
        //         ? color
        //         : Color.white;
        // }
        //
        // public void AddProduct(ProductData productData)
        // {
        //     var item = Instantiate(productPrefab, productsContainer);
        //     // item.Initialize(productData);
        //     _products.Add(item);
        //     // _selectedProductDataObservable = Observable.Merge(_selectedProductDataObservable, item.Selected);
        // }
        //
        // public void UpdateProduct(ProductData productData)
        // {
        //     // var product = _products.FirstOrDefault(p => p.ProductData.ID == productData.ID);
        //     // if (product != null)
        //     // {
        //     //      product.Initialize(productData);
        //     // }
        // }
        //
        // private Sprite Icon(string iconName)
        // {
        //     try
        //     {
        //         return _iconBuilderConfig.GetIconSprite(iconName);
        //     }
        //     catch (Exception e)
        //     {
        //         return null;
        //     }
        // }
    }
}