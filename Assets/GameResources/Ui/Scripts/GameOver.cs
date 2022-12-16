using UnityEngine;

namespace GameResources.Ui.Scripts
{
    using System;
    using System.Threading.Tasks;
    using DG.Tweening;
    using GameResources.Health.Scripts;
    using GameResources.Pause.Scripts;
    using GameResources.Slime.Scripts;
    using UnityEngine.UI;

    public class GameOver : MonoBehaviour
    {
        public event Action OnOpen;
        public event Action OnClose;
        
        [SerializeField]
        private CanvasGroup group;

        [SerializeField]
        private GameObject uiContainer;

        [SerializeField]
        private Button restart; 
        
        [SerializeField]
        private Button heal;
        
        [SerializeField]
        private Slime slime;

        [SerializeField]
        private float fadeDuration = 0.5f;

        [SerializeField]
        private float invincibleAfterHealAll = 0.5f;

        private bool _isOpened;

        private void OnEnable()
        {
            uiContainer.SetActive(false);
            _isOpened = false;
            
            slime.OnDeath += Open;
            
            restart.onClick.AddListener(CloseAndRestart);
            heal.onClick.AddListener(CloseAndHeal);
        }
        
        private void OnDisable()
        {
            slime.OnDeath -= Open;
            
            restart.onClick.RemoveListener(CloseAndRestart);
            heal.onClick.RemoveListener(CloseAndHeal);
        }
        
        private void OpenWithFade()
        {
            PauseController.Pause();
            
            group.alpha = 0;
            uiContainer.SetActive(true);

            group.DOFade(1, fadeDuration).SetUpdate(true);
        }

        private async Task CloseWithFade()
        {
            await group.DOFade(1, fadeDuration).SetUpdate(true).AsyncWaitForCompletion();
            
            uiContainer.SetActive(false);
        }

        private async void CloseAndHeal()
        {
            if (_isOpened == false)
            {
                return;
            }

            _isOpened = false;
            
            await CloseWithFade();
         
            PauseController.Continue();
            
            OnClose?.Invoke();
            
            slime.HealAll();
            slime.AddInvincible(invincibleAfterHealAll);
        }
        
        private async void CloseAndRestart()
        {
            if (_isOpened == false)
            {
                return;
            }

            _isOpened = false;
            
            await CloseWithFade();
            
            PauseController.Continue();
            
            OnClose?.Invoke();
            
            slime.HealAll();
            slime.AddInvincible(invincibleAfterHealAll);
        }
        
        private void Open(IDamagable damagable)
        {
            if (_isOpened)
            {
                return;
            }

            _isOpened = true;
            
            OnOpen?.Invoke();
            
            OpenWithFade();
        }
    }
}
