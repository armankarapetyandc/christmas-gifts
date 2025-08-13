using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.UI.Views.Product.Panels;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Product
{
    public class ProductCreationView : BasePresenterWithController<ProductCreationController>
    {
        [SerializeField] private Button backButton;
        [SerializeField] private Button infoButton;
        [SerializeField] private ProductsPanel productsPanel;
        [SerializeField] private ProductSetupPanel productSetupPanel;
        [SerializeField] private ProductIconBuilderPanel productIconBuilderPanel;
        
        public override UniTask Initialize(IPresenterData data = null)
        {
            backButton.OnClickAsObservable().Subscribe(_ =>
            {
                if(productSetupPanel.gameObject.activeSelf) productSetupPanel.Reset();
                if(productIconBuilderPanel.gameObject.activeSelf) productIconBuilderPanel.Reset();
                // Controller.OnBack();
            }).AddTo(this);
            productsPanel.OnAddButtonClicked.Subscribe(_ => NavigateToProductSetupPanel()).AddTo(this);
            productSetupPanel.OnIconButtonClicked.Subscribe(_ => NavigateToProductIconBuilderPanel()).AddTo(this);
            productIconBuilderPanel.SaveCommand.Subscribe(OnIconSelected).AddTo(this);
            productSetupPanel.OnDeleteButtonClicked.Subscribe(data => DeleteSelectedProduct(data)).AddTo(this);
            
            productSetupPanel.OnSaveButtonClicked.Subscribe(pair =>
            {
                if (pair.Item1)
                {
                    productsPanel.UpdateProduct(pair.Item2);
                    Controller.UpdateProduct(pair.Item2);
                }
                else
                {
                    var product = pair.Item2;
                    var newProductId = Controller.AddNewProduct(product);
                    product.ID = newProductId;
                    productsPanel.AddProduct(product);
                }
                NavigateToProductPanel();
                productIconBuilderPanel.Reset();
            }).AddTo(this);
            productsPanel.SelectedProductData.Subscribe(data =>
            {
                Debug.LogError($"Selected product: {data?.ID}");
                if (data != null)
                {
                    SetSetupPanel(data);
                }
            }).AddTo(this);
            return UniTask.CompletedTask;
        }

        private void DeleteSelectedProduct(ProductData data)
        {
            productsPanel.DeleteProduct(data);
            Controller.DeleteDataFromAccount(data);
            NavigateToProductPanel();
        }

        private void SetSetupPanel(ProductData productData)
        {
            NavigateToProductSetupPanel();
            productSetupPanel.SetCurrentData(productData);
        }

        private void NavigateToProductPanel()
        {
            productSetupPanel.gameObject.SetActive(false);
            productsPanel.gameObject.SetActive(true);
            productSetupPanel.ChangeDeleteButtonState(false);
        }

        private void OnIconSelected(ProductIconBuilderPanel.Result result)
        {
            //ToDo need to set product data in account info 
            productSetupPanel.gameObject.SetActive(true);
            
            productIconBuilderPanel.gameObject.SetActive(false);
            productSetupPanel.SetProductIcon(result.IconSprite, result.BackgroundColor);
        }

        private void NavigateToProductIconBuilderPanel()
        {
            productSetupPanel.gameObject.SetActive(false);
            productIconBuilderPanel.gameObject.SetActive(true);
            productSetupPanel.ChangeDeleteButtonState(false);
        }

        private void NavigateToProductSetupPanel()
        {
            productsPanel.gameObject.SetActive(false);
            productSetupPanel.gameObject.SetActive(true);
            productSetupPanel.ChangeDeleteButtonState(false);
        }
        
        public override void Dispose()
        {
        }
    }
}