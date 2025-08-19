using System.Linq;
using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.UI.Asset.Database;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.ProductionCapacity
{
    public class ProductionCapacityView : BasePresenterWithController<ProductionCapacityViewController>
    {
        [SerializeField] private Button backButton;
        [SerializeField] private EquipmentComponent equipmentComponent;

        public override UniTask Initialize(IPresenterData data = null)
        {
            backButton.OnClickAsObservable().Subscribe(_ => Controller.OnBack()).AddTo(this);
            equipmentComponent.Setup(Controller.ResolveVisualAssets<LevelVisualAsset>()
                .ToArray(), Controller.RetrieveInfo());
            return UniTask.CompletedTask;
        }

        public override void Dispose()
        {
        }
    }
}