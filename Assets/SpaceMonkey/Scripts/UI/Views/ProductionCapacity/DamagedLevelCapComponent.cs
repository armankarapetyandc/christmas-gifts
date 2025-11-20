using R3;
using SpaceMonkey.Scripts.Profile;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.ProductionCapacity
{
    public class DamagedLevelCapComponent : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI repairCostText;
        [SerializeField] private Button repairButton;

        private LevelProdCap _levelProdCap;
        private float _repairCost;

        public Observable<(LevelProdCap, float)> OnRepairClicked =>
            repairButton.onClick.AsObservable().Select(_ => (_levelProdCap, _repairCost));

        public void UpdateUi(LevelProdCap level, float repairCost)
        {
            _levelProdCap = level;
            _repairCost = repairCost;
            
            repairCostText.text = $"${repairCost:F0}";
        }
    }
}
