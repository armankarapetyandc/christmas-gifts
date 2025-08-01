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
            productsPanel.OnAddButtonClicked.Subscribe(_ => NavigateToProductSetupPanel()).AddTo(this);
            productSetupPanel.OnIconButtonClicked.Subscribe(_ => NavigateToProductIconBuilderPanel()).AddTo(this);
            productIconBuilderPanel.SaveCommand.Subscribe(OnIconSelected).AddTo(this);
            
            return UniTask.CompletedTask;
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
        }

        private void NavigateToProductSetupPanel()
        {
            productsPanel.gameObject.SetActive(false);
            productSetupPanel.gameObject.SetActive(true);
        }


        public override void Dispose()
        {
        }
    }
}