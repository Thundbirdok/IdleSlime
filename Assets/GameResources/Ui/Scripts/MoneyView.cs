using UnityEngine;

namespace GameResources.Ui.Scripts
{
    using GameResources.Money.Scripts;
    using TMPro;

    public class MoneyView : MonoBehaviour
    {
        [SerializeField]
        private MoneyHandler handler;

        [SerializeField]
        private TextMeshProUGUI text;
        
        private void OnEnable()
        {
            SetText(0);
            
            handler.OnAmountChange += SetText;
        }
        
        private void OnDisable()
        {
            handler.OnAmountChange -= SetText;
        }

        private void SetText(int value)
        {
            text.text = handler.Amount.ToString();
        }
    }
}
