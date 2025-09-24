using SpaceMonkey.Scripts.UI.Views.ProductionCapacity;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.LevelUpdate
{
    public class LevelUpdateController : BasePresenterController
    {
        public LevelUpdateController(PresenterService presenterService) : base(presenterService)
        {
        }

        public void OnNext(int levelNumber)
        {
            PresenterService.Show<ProductionCapacityView>(new ProductionCapacityView.Data
            {
                LevelNumber = levelNumber
            });
        }
    }
}