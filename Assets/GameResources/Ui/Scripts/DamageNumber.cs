using UnityEngine;

namespace GameResources.Ui.Scripts
{
    using System;
    using System.Threading.Tasks;
    using TMPro;

    public class DamageNumber : MonoBehaviour
    {
        public event Action<DamageNumber> OnFade;
        
        [SerializeField]
        private TextMeshProUGUI text;

        [SerializeField]
        private int lifeTime = 500;
        
        private async void OnEnable()
        {
            await Task.Delay(lifeTime);
            
            gameObject.SetActive(false);
            
            OnFade?.Invoke(this);
        }

        public void SetValue(int value)
        {
            text.text = value.ToString();
        }
    }
}
