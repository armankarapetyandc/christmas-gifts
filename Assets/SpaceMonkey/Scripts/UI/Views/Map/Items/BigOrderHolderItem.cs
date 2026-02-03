using System.Linq;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Configs.Map;
using SpaceMonkey.Scripts.Profile;
using TMPro;
using UnityEngine;
using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Map.Items
{
    public class BigOrderHolderItem : MapPlaceHolderItem
    {
        [SerializeField] private TextMeshProUGUI moneyText;
        [SerializeField] private TextMeshProUGUI productCostText;
        [SerializeField] private TextMeshProUGUI customerText;
        [SerializeField] private Color profitColor;
        [SerializeField] private Color costColor;
        private AccountService _accountService;
        private GameConfig _gameConfig;

        [Inject]
        private void Inject(AccountService accountService, GameConfig gameConfig)
        {
            _gameConfig = gameConfig;
            _accountService = accountService;
        }

        public override void Init(PlaceType placeType)
        {
            base.Init(placeType);
            var bigOrderData = _accountService.Model.Account.BigOrderGameData;
            SetLocked(!bigOrderData.IsActive);

            float profit = 0;
            float prodCost = 0;
            
            if (bigOrderData.IsActive)
            {
                var order = _accountService.Model.Account.GetOrders()[0];
                prodCost = order.OrderProdCost;
                profit = order.OrderProfit;
                
                var characterId = _accountService.Model.Account.BigOrderGameData.CharacterId;
                var character = _gameConfig.Characters.FirstOrDefault(c => c.Id.Equals(characterId));
                customerText.text = $"Customer: <b>{character.Name}</b>";
            }
            
            moneyText.text =
                $"Profit per week:<color=#{ColorUtility.ToHtmlStringRGB(profitColor)}>+${profit:F2}</color>";

            productCostText.text =
                $"Production cost:<color=#{ColorUtility.ToHtmlStringRGB(costColor)}>-{Mathf.RoundToInt(prodCost)} hrs</color>";
        }
    }
}