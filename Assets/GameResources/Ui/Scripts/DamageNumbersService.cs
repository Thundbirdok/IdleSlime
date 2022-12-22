using UnityEngine;

namespace GameResources.Ui.Scripts
{
    using System.Collections.Generic;
    using GameResources.Enemies.Scripts;
    using GameResources.Health.Scripts;
    using GameResources.Slime.Scripts;
    using UnityEngine.Pool;

    public class DamageNumbersService : MonoBehaviour
    {
        [SerializeField]
        private Slime player;
        
        [SerializeField]
        private EnemiesHandler enemiesHandler;
        
        [SerializeField]
        private DamageNumber prefab;

        [SerializeField]
        private Transform container;

        [SerializeField]
        private Camera locationCamera;

        [SerializeField]
        private RectTransform localRect;

        [SerializeField]
        private GameOver gameOver;
        
        private readonly List<DamageNumber> _views = new List<DamageNumber>();

        private int _viewsActiveIndex;
        
        private ObjectPool<DamageNumber> _pool;

        private void Awake()
        {
            InitPool();
        }

        private void OnEnable()
        {
            Subscribe();
        }

        private void OnDisable() => Unsubscribe();

        private void OnDestroy() => DestroyPool();

        private void InitPool()
        {
            _pool = new ObjectPool<DamageNumber>
            (
                CreateFunc,
                ActionOnGet,
                ActionOnRelease,
                ActionOnDestroy,
                false
            );
        }

        private void DestroyPool()
        {
            _pool.Clear();

            foreach (var view in _views)
            {
                if (view != null && view.gameObject != null)
                {
                    Destroy(view.gameObject);   
                }
            }
            
            _views.Clear();

            _pool = null;
        }

        private static void ActionOnDestroy(DamageNumber number)
        {
            Destroy(number.gameObject);
        }

        private void ActionOnRelease(DamageNumber number)
        {
            _views.Remove(number);
            
            number.OnFade -= Release;
            
            number.gameObject.SetActive(false);
        }

        private void ActionOnGet(DamageNumber number)
        {
            _views.Add(number);

            number.OnFade += Release;
            
            number.gameObject.SetActive(true);
        }

        private DamageNumber CreateFunc()
        {
            var number = Instantiate(prefab, container);
            
            number.gameObject.SetActive(false);

            return number;
        }

        private void Subscribe()
        {
            enemiesHandler.OnUpdateObjects += SubscribeOnEnemies;

            player.OnAmountChange += OnHealthChange;
            
            gameOver.OnOpen += Hide;
            gameOver.OnClose += Show;
        }

        private void Unsubscribe()
        {
            enemiesHandler.OnUpdateObjects -= SubscribeOnEnemies;
            
            player.OnAmountChange -= OnHealthChange;
            
            gameOver.OnOpen -= Hide;
            gameOver.OnClose -= Show;
        }

        private void SubscribeOnEnemies()
        {
            foreach (var enemy in enemiesHandler.Enemies)
            {
                enemy.OnAmountChange += OnHealthChange;
                enemy.OnDeath += UnsubscribeFromEnemy;
            }
        }

        private void UnsubscribeFromEnemy(IDamagable damagable)
        {
            damagable.OnAmountChange -= OnHealthChange;
            damagable.OnDeath -= UnsubscribeFromEnemy;
        }
        
        private void OnHealthChange(IDamagable damagable, int value)
        {
            var worldToScreenPoint = RectTransformUtility.WorldToScreenPoint
            (
                locationCamera,
                damagable.HealthBarPosition
            );

            RectTransformUtility.ScreenPointToLocalPointInRectangle
            (
                localRect,
                worldToScreenPoint,
                null,
                out var anchoredPosition
            );

            var view = _pool.Get();
            
            view.SetValue(value);
            
            var viewRectTransform = view.transform as RectTransform;

            if (viewRectTransform != null)
            {
                viewRectTransform.anchoredPosition = anchoredPosition;
            }
        }

        private void Show()
        {
            container.gameObject.SetActive(true);
        }
        
        private void Hide()
        {
            container.gameObject.SetActive(false);

            for (var i = 0; i < _views.Count;)
            {
                _pool.Release(_views[0]);
            }
        }

        private void Release(DamageNumber number)
        {
            _pool.Release(number);
        }
    }
}
