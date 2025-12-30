using AudioPlayer;
using AudioPlayerService.Runtime;
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
        [SerializeField] private TextMeshProUGUI swipeLevelInfoText;
        [SerializeField] private TextMeshProUGUI infoText;

        private LevelProdCap _levelProdCap;
        private float _repairCost;

        public Observable<(LevelProdCap, float)> OnRepairClicked =>
            repairButton.onClick.AsObservable().Select(_ =>
            {
                SfxPlayer.Play(Sounds.Button_Tap);
                return (_levelProdCap, _repairCost);
            });

        public void UpdateUi(LevelProdCap level, float repairCost,int hrs)
        {
            _levelProdCap = level;
            _repairCost = repairCost;
            swipeLevelInfoText.text = $"Swipe to see Level {level.LevelNumber+2}!";
            infoText.text = $"Production capacity is down -{hrs} hr due to damaged equipment.";
            repairCostText.text = $"${repairCost:F0}";
        }
    }
}
