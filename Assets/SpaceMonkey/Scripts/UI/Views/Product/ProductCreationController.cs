using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Navigation.Bottom;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Views.Product
{
    public class ProductCreationController : BasePresenterController
    {
        internal AccountService AccountService { get; }
        private readonly NavigationPresenterService _navigationPresenterService;

        public ProductCreationController(PresenterService presenterService, AccountService accountService,
            NavigationPresenterService navigationPresenterService) : base(presenterService)
        {
            AccountService = accountService;
            _navigationPresenterService = navigationPresenterService;
        }

        public void OnBack()
        {
            _navigationPresenterService.Show<MainNavigation>();
        }

        public string AddNewProduct(ProductData productData)
        {
            var newProduct = new Profile.Product()
            {
                Name = productData.Name,
                Price = productData.Price,
                TtpCost = productData.TtpCost,
                TotalCost = productData.TotalCost,
                ShippingCost = productData.ShippingCost,
                Profit = productData.Profit,
                Icon = productData.Icon.ToString(),
                TimeToProduceIndex = productData.TimeToProduceIndex,
                PackagingCost = productData.PackagingCost,
                MaterialCost = productData.MaterialCost,
                BackgroundColor = ColorUtility.ToHtmlStringRGBA(productData.BackgroundColor),
            };
            newProduct.GenerateID();
            AccountService.Model.Account.Products.Add(newProduct);
            AccountService.SaveAsync().Forget();
            return newProduct.ID;
        }

        public void DeleteDataFromAccount(ProductData prodData)
        {
            var productToDelete = AccountService.Model.Account.Products.Find(data => data.ID == prodData.ID);
            if (productToDelete != null)
            {
                AccountService.Model.Account.Products.Remove(productToDelete);
            }
            AccountService.SaveAsync().Forget();
        }
    }
}