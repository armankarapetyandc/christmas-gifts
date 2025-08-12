using System;
using System.Collections.Generic;
using System.Linq;
using R3;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Asset.Product;
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

        private List<ProductItem> _products = new List<ProductItem>();
        private ReactiveProperty<ProductData> _selectedProductData = new ReactiveProperty<ProductData>();
        
        public ReadOnlyReactiveProperty<ProductData> SelectedProductData => _selectedProductData;


        internal Observable<Unit> OnAddButtonClicked => addButton.OnClickAsObservable();
        [Inject] private AccountService _accountService;
        [Inject] private ProductIconBuilderConfig _iconBuilderConfig;

        private void Start()
        {
            foreach (var product in _accountService.Model.Account.Products)
            {
                AddProduct(new ProductData
                {
                    Name = product.Name,
                    Price = product.Price,
                    TtpCost = product.TtpCost,
                    TotalCost = product.TotalCost,
                    ShippingCost = product.ShippingCost,
                    PackagingCost = product.PackagingCost,
                    MaterialCost = product.MaterialCost,
                    Profit = product.Profit,
                    TimeToProduceIndex = product.TimeToProduceIndex,
                    Icon = Icon(product.Icon),
                    BackgroundColor = BackgroundColor(product.BackgroundColor)
                });
            }
        }

        public void DeleteProduct(ProductData productData)
        {
            var productToDelete = _products.Find(p => p.ProductData.ID == productData.ID);
            if (productToDelete != null)
            {
                if (productToDelete.gameObject != null)
                {
                    Destroy(productToDelete.gameObject);
                }
                    
                _products.Remove(productToDelete);
            }
        }


        private Color BackgroundColor(string backColor)
        {
            return ColorUtility.TryParseHtmlString("#" + backColor, out var color)
                ? color
                : Color.white;
        }

        public void AddProduct(ProductData productData)
        {
            var item = Instantiate(productPrefab, productsContainer);
            item.Initialize(productData);
            item.Selected.Subscribe(_ =>
            {
                _selectedProductData.Value = productData;
            });
            _products.Add(item);
        }

        private Sprite Icon(string iconName)
        {
            try
            {
               return _iconBuilderConfig.GetIconSprite(iconName);
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}