using TMPro;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Views.ProductionCapacity
{
    public class CurrentLevelCapComponent : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI capacityText;

        public void UpdateUi(int capacity)
        {
            capacityText.text = $"{capacity} hr";
        }
    }
}