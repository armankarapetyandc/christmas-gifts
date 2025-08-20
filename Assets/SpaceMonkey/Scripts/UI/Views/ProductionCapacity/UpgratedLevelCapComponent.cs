using R3;
using SpaceMonkey.Scripts.Profile;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.ProductionCapacity
{
    public class UpgratedLevelCapComponent : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI capacityText;
        [SerializeField] private TextMeshProUGUI addCapText;
        [SerializeField] private TextMeshProUGUI costCapText;
        [SerializeField] private Button upgradeButton;

        private LevelProdCap _levelProdCap;

        public Observable<LevelProdCap> OnUpgradedLevelUp =>
            upgradeButton.onClick.AsObservable().Select(_ => _levelProdCap);

        public void UpdateUi(LevelProdCap level, int currentCap)
        {
            _levelProdCap = level;
            capacityText.text = $"{currentCap} hr";
            addCapText.text = $"(+{level.ProdCapAdd})";
            costCapText.text = $"${level.ProdCapCost}";
        }
    }
}