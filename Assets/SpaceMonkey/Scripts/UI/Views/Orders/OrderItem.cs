using SpaceMonkey.Scripts.UI.Components;
using TMPro;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Views.Orders
{
    public class OrderItem : MonoBehaviour
    {
        [SerializeField] private IconComponent iconComponent;
        [SerializeField] private TextMeshProUGUI customerName;
        [SerializeField] private TextMeshProUGUI moneyText;
        [SerializeField] private TextMeshProUGUI productionCapacityText;
    }
}