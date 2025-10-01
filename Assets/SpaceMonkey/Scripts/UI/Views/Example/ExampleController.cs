using SpaceMonkey.Scripts.UI.Views.BusinessExamples;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Example
{
    public class ExampleController : BasePresenterController
    {
        public ExampleController(PresenterService presenterService) : base(presenterService)
        {
        }

        public void OnBack( string category)
        {
            PresenterService.Show<BusinessExamplesView>(new BusinessExamplesView.Data()
            {
                Category = category
            });
        }
    }
}