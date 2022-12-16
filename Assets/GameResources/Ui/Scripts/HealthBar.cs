using UnityEngine;

namespace GameResources.Ui.Scripts
{
    using GameResources.Health.Scripts;
    using UnityEngine.UI;

    public class HealthBar : MonoBehaviour
    {
        [SerializeField]
        private Image fill;

        [SerializeField]
        private Color enemyColor = Color.red;
        
        [SerializeField]
        private Color playerColor = Color.green;
        
        public IDamagable Damagable { get; private set; }

        private void OnEnable()
        {
            Subscribe();
        }

        private void OnDisable()
        {
            Unsubscribe();
        }

        public void SetData(IDamagable damagable, bool isEnemy)
        {
            Unsubscribe();
            
            Damagable = damagable;

            Subscribe();

            fill.color = isEnemy ? enemyColor : playerColor;

            SetValue();
        }

        private void SetValue()
        {
            fill.fillAmount = Mathf.Clamp01((float)Damagable.Health / Damagable.MaxHealth);
        }

        private void Subscribe()
        {
            if (Damagable != null)
            {
                Damagable.OnAmountChange += SetValue;
            }
        }
        
        private void Unsubscribe()
        {
            if (Damagable != null)
            {
                Damagable.OnAmountChange -= SetValue;
            }
        }
    }
}
