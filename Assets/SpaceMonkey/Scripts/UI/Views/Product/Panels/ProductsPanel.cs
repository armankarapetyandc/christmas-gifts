using System.Collections.Generic;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Product.Panels
{
    public class ProductsPanel : MonoBehaviour
    {
        [SerializeField] private ProductItem productPrefab;
        [SerializeField] private RectTransform productsContainer;
        [SerializeField] private Button addButton;
        
        private List<ProductItem> _products = new List<ProductItem>();
        
        internal Observable<Unit> OnAddButtonClicked => addButton.OnClickAsObservable();

        public void AddProduct(ProductData productData)
        {
            var item = Instantiate(productPrefab, productsContainer);
            item.Initialize(productData);
            _products.Add(item);
        }
    }
}