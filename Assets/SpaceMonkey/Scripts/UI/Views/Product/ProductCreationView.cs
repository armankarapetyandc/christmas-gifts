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
            //productSetupPanel.OnIconButtonClicked.Subscribe(_ => NavigateToProductIconBuilderPanel()).AddTo(this);
            return UniTask.CompletedTask;
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