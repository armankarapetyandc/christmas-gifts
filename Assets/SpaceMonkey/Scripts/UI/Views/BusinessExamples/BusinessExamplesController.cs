using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.UI.Views.Business.BusinessSetupCelebration;
using SpaceMonkey.Scripts.UI.Views.Example;
using SpaceMonkey.Scripts.Utilities;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.BusinessExamples
{
    public class BusinessExamplesController : BasePresenterController
    {
        [Inject] private GameConfig _gameConfig;

        public BusinessExamplesController(PresenterService presenterService) : base(presenterService)
        {
        }

        public BusinessExample[] GetBusinessExamples()
        {
            return _gameConfig.BusinessExamples;
        }

        public void OnMoreClick(BusinessExample example, string category)
        {
            PresenterService.Show<ExampleView>(new ExampleView.Data
            {
                Name = example.Name,
                Description = example.Description,
                Title = example.Title,
                Category = category
            });
        }

        public void OnBack()
        {
            PresenterService.HidePreviousAndShow<BusinessSetupCelebrationView>().Forget();
        }
    }
}