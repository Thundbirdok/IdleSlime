using UnityEngine;

namespace GameResources.Ui.Scripts
{
    using GameResources.Money.Scripts;
    using GameResources.Stats.Scripts;
    using TMPro;
    using UnityEngine.UI;

    public class UpgradeView : MonoBehaviour
    {
        [SerializeField]
        private MoneyHandler moneyHandler;

        [SerializeField]
        private StatHandler upgrade;
        
        [SerializeField]
        private Button buy;

        [SerializeField]
        private TextMeshProUGUI cost;

        [SerializeField]
        private TextMeshProUGUI description;
        
        private void OnEnable()
        {
            buy.onClick.AddListener(BuyUpgrade);

            cost.text = upgrade.NextLevelCost.ToString();
            description.text = upgrade.Description;
        }

        private void OnDisable()
        {
            buy.onClick.RemoveListener(BuyUpgrade);
        }

        private void BuyUpgrade()
        {
            if (moneyHandler.Spend(upgrade.NextLevelCost) == false)
            {
                return;
            }

            upgrade.AddLevel(1);
            
            cost.text = upgrade.NextLevelCost.ToString();
        }
    }
}
