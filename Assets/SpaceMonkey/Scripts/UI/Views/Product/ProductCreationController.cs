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

        public void AddNewProduct(ProductData productData)
        {
            AccountService.Model.Account.Products.Add(new Profile.Product
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
                BackgroundColor = ColorUtility.ToHtmlStringRGBA(productData.BackgroundColor)
            });
            AccountService.SaveAsync().Forget();
        }

        public void DeleteDataFromAcount(ProductData prodData)
        {
            var productToDelete = AccountService.Model.Account.Products.Find(data => data.Name == prodData.Name);
            if (productToDelete != null)
            {
                AccountService.Model.Account.Products.Remove(productToDelete);
            }

        }
    }
}